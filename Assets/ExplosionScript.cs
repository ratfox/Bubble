using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    float creationTime;
    float duration = 2f;
    // Start is called before the first frame update
    void Start()
    {
        creationTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - creationTime > duration) {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collider) {
        Debug.Log("Crash of " + this.gameObject.name + " and " + collider.gameObject.name);
        if (collider.gameObject.CompareTag("Shots")) {
            Destroy(collider.gameObject);
        } else if (collider.gameObject.name.StartsWith("Walls")) {
            if (collider.gameObject.name.Contains("Breakable")) {
                collider.gameObject.GetComponent<BreakableScript>().Hit();
                collider.gameObject.GetComponent<BreakableScript>().Hit();
                collider.gameObject.GetComponent<BreakableScript>().Hit();
            }
        } else if (collider.gameObject.name.StartsWith("Fog")) {
            if (collider.gameObject.GetComponent<FogScript>().dense) {
                collider.gameObject.GetComponent<FogScript>().Hit();
            }
        }
    }
}
