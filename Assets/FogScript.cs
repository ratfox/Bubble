using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogScript : MonoBehaviour
{
    public bool lethal;
    public bool dense;
    List<FogScript> touching = new List<FogScript>();
    FogScript startedTouching = null;
    float touchingDistance = 1.0f;
    float timeStartedTouching;
    public float timeUntilCondense = 5f;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Renderer>().enabled = dense;
        GameObject[] fogs = GameObject.FindGameObjectsWithTag("Fog");
        foreach (GameObject fog in fogs) {
            if (gameObject.name == fog.name) {
                continue;
            }
            if ((fog.transform.position - transform.position).magnitude < touchingDistance) {
                Debug.Log("Adding to " + gameObject.name + ": " + fog.name);
                touching.Add(fog.GetComponent<FogScript>());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (dense) {
            return;
        }
        if (startedTouching != null) {
            if (!startedTouching.dense) {
                startedTouching = null;
                return;
            }
            if (Time.time - timeStartedTouching > timeUntilCondense) {
                dense = true;
                gameObject.GetComponent<Renderer>().enabled = true;
                startedTouching = null;
            }
            return;
        }
        foreach (FogScript fog in touching) {
            if (fog.dense) {
                Debug.LogWarning("Fog " + gameObject.name + " touching " + fog.gameObject.name);
                startedTouching = fog;
                timeStartedTouching = Time.time;
                break;
            }
        }
    }

    public void Hit() {
        if (!dense) {
            return;
        }
        if (dense) {
            dense = false;
            gameObject.GetComponent<Renderer>().enabled = false;
        }
    }
}
