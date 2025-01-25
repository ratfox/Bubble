using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class circleControl : MonoBehaviour
{
    public float maxSpeed = 5f;
    private float recoil = 0.1f;
    private float speedMul = 3.0f;
    private float keyCheatFactor = 0.008f;
    private float slowDownFactor = 0.9996f;
    private float minSize = 0.1f;
    private float maxSize = 1.0f;
    private float size = 1.0f;
    private float shotSize = 0.1f;
    private float sizeRegeneration = 0.001f;
    public GameObject shotPrefab;
    private Vector3 movement;
    private int shotCounter = 0;
    private bool alive = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) {
            SceneManager.LoadScene("Stage 1");
            gameObject.GetComponent<Renderer>().enabled = true;
            alive = true;
        }
        if (!alive) {
            return;
        }
        var x = Input.GetAxisRaw("Horizontal");
        var y = Input.GetAxisRaw("Vertical");
        movement.x += x * keyCheatFactor;
        movement.y += y * keyCheatFactor;
        if (Input.GetKeyDown(KeyCode.Space)) {
            movement *= 0;
        }
        Move(movement);
    }

    private void GameOver() {
        Debug.LogError("Game over!");
        alive = false;
        movement *= 0;
        gameObject.GetComponent<Renderer>().enabled = false;
    }
    private void Move(Vector3 dir) {
        var pos = transform.position + dir * speedMul;
        transform.position = Vector2.Lerp(transform.position, pos, Time.deltaTime);
        transform.localScale = new Vector3(size, size, size);
        gameObject.GetComponent<CircleCollider2D>().radius = 0.5f * size;
    }

    void FixedUpdate() {
        if (size < maxSize) {
            size = Mathf.Min(maxSize, size += sizeRegeneration);
        }
        movement = movement * slowDownFactor;
    }

    public void Shoot(Vector3 target) {
        if (!alive) {
            return;
        }
        if (size <= shotSize + minSize) {
            return;
        }
        var item_ = Instantiate(shotPrefab);
        item_.gameObject.name = "SingleShot " + shotCounter;
        item_.GetComponent<ShotScript>().shotId = shotCounter;
        shotCounter++;
        item_.transform.position = transform.position;
        var trajectory = (target - transform.position).normalized;
        item_.GetComponent<ShotScript>().direction = trajectory;
        Debug.Log("Shoot direction " + trajectory);
        movement -= trajectory * recoil;
        if (movement.magnitude > maxSpeed) {
            movement = movement.normalized * maxSpeed;
        }
        size -= shotSize;
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        Debug.Log("Player crash in " + collider.gameObject.name);
        if (collider.gameObject.CompareTag("Shots")) {
            var item = collider.gameObject;
            Debug.Log("Crash of " + this.gameObject.name + " and " + item.name);
            if (item.name.StartsWith("SingleShot")) {
                if (Time.time - item.GetComponent<ShotScript>().creationTime < 0.1) {
                    return;
                }
                size = Mathf.Min(maxSize, size += shotSize);
                Destroy(item);
            } else if (item.name.StartsWith("TwoShot")) {
                size = Mathf.Min(maxSize, size += 2 * shotSize);
                Destroy(item);
            } else if (item.name.StartsWith("ThreeShot")) {
                size = Mathf.Min(maxSize, size += 3 * shotSize);
                Destroy(item);
            }
        } else if (collider.gameObject.name.StartsWith("Walls")) {
            GameOver();
        } else if (collider.gameObject.name.StartsWith("Fog")) {
            if (collider.gameObject.GetComponent<FogScript>().dense) {
                GameOver();
            }
        } else if (collider.gameObject.name.StartsWith("Goal")) {
            if (collider.gameObject.name.StartsWith("Goal go to ")) {
                var nextScene = collider.gameObject.name.Substring("Goal go to ".Length);
                SceneManager.LoadScene(nextScene);
            }
        }
    }    
}
