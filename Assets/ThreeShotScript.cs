using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeShotScript : MonoBehaviour
{
    public Vector3 direction;  // Normalized
    private float mul = 3.0f;
    public float speed;
    private float minSpeed = 0.1f;
    private float slowDownFactor = 0.96f;
    private float rotationSpeed = 40f;
    public int shotId;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        var pos = transform.position;
        pos += new Vector3(mul * direction.x, mul * direction.y, 0);
        transform.position = Vector2.Lerp(transform.position, pos, Time.deltaTime * speed);
        var rot = transform.rotation;
        rot *= Quaternion.Euler(0, 0, rotationSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime);
    }

    void FixedUpdate() {
        speed = Mathf.Max(minSpeed, speed * slowDownFactor);
//        Debug.Log("speed " + speed);
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        Debug.Log("Crash of " + this.gameObject.name + " and " + collider.gameObject.name);
        if (collider.gameObject.CompareTag("Shots")) {
            var item = collider.gameObject;
            if (item.name.StartsWith("ThreeShot")) {
                Destroy(item);
                Destroy(this.gameObject);
            }
        } else if (collider.gameObject.name.StartsWith("Walls")) {
            Destroy(this.gameObject);
        }
    }
}
