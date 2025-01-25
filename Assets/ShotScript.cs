using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotScript : MonoBehaviour
{
    public Vector3 direction;  // Normalized
    private float mul = 3.0f;
    private float speed = 5.0f;  // Initial speed, then current speed.
    private float minSpeed = 0.1f;
    private float slowDownFactor = 0.96f;
    public GameObject twoShotPrefab;
    public GameObject threeShotPrefab;
    public int shotId;
    public float creationTime;

    // Start is called before the first frame update
    void Start()
    {
        creationTime = Time.time;
        Vector2 S = gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size;
        Debug.Log("SPRITE SIZE " + S);
        Debug.Log("GAMEO SIZE " + gameObject.GetComponent<BoxCollider2D>().size);
        Debug.Log("GAMEO CENT " + gameObject.GetComponent<BoxCollider2D>().offset);
//        gameObject.GetComponent<BoxCollider2D>().size = S;
 //       gameObject.GetComponent<BoxCollider2D>().center = new Vector2 ((S.x / 2), 0);
    }

    // Update is called once per frame
    void Update() {
        var pos = transform.position;
        pos += new Vector3(mul * direction.x, mul * direction.y, 0);
        transform.position = Vector2.Lerp(transform.position, pos, Time.deltaTime * speed);        
    }
    void FixedUpdate() {
        speed = Mathf.Max(minSpeed, speed * slowDownFactor);
//        Debug.Log("speed " + speed);
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        Debug.Log("Crash of " + this.gameObject.name + " and " + collider.gameObject.name);
        if (collider.gameObject.CompareTag("Shots")) {
            var item = collider.gameObject;
            if (item.name.StartsWith("SingleShot")) {
                var other = item.GetComponent<ShotScript>();
                if (other.shotId < shotId) {
                    return;
                }
                var newItem = Instantiate(twoShotPrefab);
                newItem.gameObject.name = "TwoShot: " + this.gameObject.name + " & " + item.name;
                newItem.transform.position = (item.transform.position + transform.position) / 2;
                var newMovement = (direction * speed + other.direction * other.speed) / 2;
                newItem.GetComponent<TwoShotScript>().direction = newMovement.normalized;
                newItem.GetComponent<TwoShotScript>().speed = newMovement.magnitude;
                Destroy(item);
                Destroy(this.gameObject);
            } else if (item.name.StartsWith("TwoShot")) {
                var other = item.GetComponent<TwoShotScript>();
                var newItem = Instantiate(threeShotPrefab);
                newItem.gameObject.name = "ThreeShot: " + this.gameObject.name + " & " + item.name;
                newItem.transform.position = (item.transform.position * 2 + transform.position) / 3;
                var newMovement = (direction * speed + other.direction * other.speed * 2) / 3;
                newItem.GetComponent<ThreeShotScript>().direction = newMovement.normalized;
                newItem.GetComponent<ThreeShotScript>().speed = newMovement.magnitude;
                Destroy(item);
                Destroy(this.gameObject);
            } else if (item.name.StartsWith("ThreeShot")) {
                Destroy(item);
                Destroy(this.gameObject);
            }
        } else if (collider.gameObject.name.StartsWith("Walls")) {
            if (collider.gameObject.name.Contains("Breakable")) {
                collider.gameObject.GetComponent<BreakableScript>().Hit();
            }
            Destroy(this.gameObject);
        } else if (collider.gameObject.name.StartsWith("Fog")) {
            if (collider.gameObject.GetComponent<FogScript>().dense) {
                collider.gameObject.GetComponent<FogScript>().Hit();
                Destroy(this.gameObject);
            }
        }
    }    
}
