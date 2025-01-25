using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableScript : MonoBehaviour
{
    private int health = 4;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Hit() {
        health--;
        switch (health) {
            case 4:
                GetComponent<SpriteRenderer>().color = new Color(0.9f, 0.9f, 0.9f);
                break;
            case 3:
                GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f);
                break;
            case 2:
                GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f);
                break;
            case 1:
                GetComponent<SpriteRenderer>().color = new Color(0.6f, 0.6f, 0.6f);
                break;
            case 0:
            default:
                Destroy(gameObject);
                break;
        }   
    }

    // Update is called once per frame
    void Update()
    {
    }
}
