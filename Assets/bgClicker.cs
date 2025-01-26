using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgClicker : MonoBehaviour
{
    GameObject player = null;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        var rec = gameObject.GetComponent<SpriteRenderer>().bounds;
        Debug.Log("RECTANGLE " + rec);
        var bottomLeft = new Vector2(rec.min[0], rec.min[1]);
        var topLeft =  new Vector2(rec.min[0], rec.max[1]);
        var topRight = new Vector2(rec.max[0], rec.max[1]);
        var bottomRight = new Vector2(rec.max[0], rec.min[1]);

        var walls = transform.GetChild(0).gameObject;
        var collider = walls.GetComponent<EdgeCollider2D>();
        var edgePoints = new [] {bottomLeft,topLeft,topRight,bottomRight, bottomLeft};
        Debug.Log("Bounds " + edgePoints[0] + " " + edgePoints[1] + " " + edgePoints[2] + " " + edgePoints[3] + " " + edgePoints[4] + " ");
        collider.points = edgePoints;
    }

    // Update is called once per frame
    void Update()
    {        
        if(Input.GetMouseButtonDown(0)) {
            var tapPos  = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log("Tap position " + tapPos);
            tapPos.z = 0;
            player.GetComponent<circleControl>().Shoot(tapPos);
        }
    }
}
