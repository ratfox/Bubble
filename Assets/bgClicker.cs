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
