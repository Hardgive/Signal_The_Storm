using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOrdering : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetMouseButton(1))
        {
            SetWaypointPosition();
        }
    }

    protected void SetWaypointPosition()
    {
        var point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // TODO: set next waypoint and route to it on a map
    }
}
