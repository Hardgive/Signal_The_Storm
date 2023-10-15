using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace STS{

public abstract class MoveablePawn : Pawn
{
    protected Vector3 targetPosition;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            SetTargerPosition();
        }

        if (state == GlobalPawnStates.MOVING)
        {
            Move();
        }
    }


    protected void Move()
    {
        if(this.state != GlobalPawnStates.MOVING)
            return;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, 1.0f*Time.deltaTime);
        if(transform.position == targetPosition)
        {
            state = GlobalPawnStates.PLANNING;        
        }
    }

    protected abstract void SetTargerPosition();
}
}