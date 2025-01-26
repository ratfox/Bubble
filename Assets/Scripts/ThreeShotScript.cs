using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeShotScript : MonoBehaviour
{
    public Vector3 direction;  // Normalized
    private float mul = 3.0f;
    public float speed;
    private float minSpeed = 0.1f;
    private float slowDownFactor = 1.01f;
    private float rotationSlowDownFactor = 1.06f;
    private float rotationSpeed = 40f;
    public int shotId;
    private float creationTime;
    private float duration = 10f;
    public GameObject explosionPrefab;
    private Vector3 influenceSum;
    private float influenceRate = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        creationTime = Time.time;
    }

    // Update is called once per frame
    void Update() {
        var pos = transform.position;
        pos += new Vector3(mul * direction.x, mul * direction.y, 0);
        transform.position = Vector2.Lerp(transform.position, pos, Time.deltaTime * speed);
        var rot = transform.rotation;
        rot *= Quaternion.Euler(0, 0, rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, 1);
        if (Time.time - creationTime > duration) {
            Explode();
        }
//        Debug.Log("" + rotationSpeed + " - " + transform.rotation);
    }

    void Explode() {
        var newItem = Instantiate(explosionPrefab);
        newItem.transform.position = transform.position;
        Destroy(gameObject);
    }

    void FixedUpdate() {
        var newMovement = direction * speed + influenceSum * influenceRate;
        direction = newMovement.normalized;
        speed = newMovement.magnitude;
        speed = Mathf.Max(minSpeed, speed * slowDownFactor);
        rotationSpeed = rotationSpeed * rotationSlowDownFactor;
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        Debug.Log("Crash of " + this.gameObject.name + " and " + collider.gameObject.name);
        if (collider.gameObject.CompareTag("Shots")) {
            var item = collider.gameObject;
            if (item.name.StartsWith("ThreeShot")) {
                Destroy(item);
                Explode();
            }
        } else if (collider.gameObject.name.StartsWith("Walls")) {
            Explode();
        } else if (collider.gameObject.name.StartsWith("Fog")) {
            if (collider.gameObject.GetComponent<FogScript>().dense) {
                collider.gameObject.GetComponent<FogScript>().Hit();
            }
        } else if (collider.gameObject.name.StartsWith("Fan")) {
            var influence = collider.gameObject.GetComponent<FanScript>().movement;
            influenceSum += influence;
            Debug.Log("Start Influence is " + influenceSum);
        }
    }
    private void OnTriggerExit2D(Collider2D collider) {
        if (collider.gameObject.name.StartsWith("Fan")) {
            var influence = collider.gameObject.GetComponent<FanScript>().movement;
            influenceSum -= influence;
            Debug.Log("End Influence is " + influenceSum);
        }
    }
}
