using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace STS{

public abstract class MoveablePawn : Pawn
{
    [SerializeField]
    public MovementObject movement;
    protected Vector3 targetPosition;
    protected Queue<Vector3> route;

    Pathfinding pathfinder;

    void Start()
    {
        pathfinder = new Pathfinding(movement);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateParameters();
        if(Input.GetMouseButton(0))
        {
            SetTargerPosition();
            
        }

        if (state == GlobalPawnStates.MOVING)
        {
            Move();
        }
    }

    protected void UpdateParameters()
    {
        movement.UpdateParameters(MapManager.instance.getTileDataMerged(transform.position));
    }

    protected void Move()
    {
        if(this.state != GlobalPawnStates.MOVING)
            return;

        transform.position = Vector3.MoveTowards(
                                                    transform.position, 
                                                    targetPosition, 
                                                    movement.elements.curVelocity*Time.deltaTime
                                                );
        // print("CurVelocity "+ movementParameters.curVelocity+ " per time "+ Time.deltaTime);
        if(transform.position == targetPosition)
        {
            state = GlobalPawnStates.PLANNING;
            ChangeWaypoint();        
        }
    }

    protected void PlanRoute(Vector3 destination)
    {
        var dest = pathfinder.GetPath(transform.position, destination);
        var msg = String.Join("; ", dest.Select(x=>x.ToString()).ToArray());
        print( msg );
        if(dest.Count()>0)
            route = new Queue<Vector3>(dest);
    }

    protected void ChangeWaypoint()
    {
        if(route.Count()>0)
        {
            targetPosition = route.Dequeue();
            state = GlobalPawnStates.MOVING;
        }
    }

    protected abstract void SetTargerPosition();
}
}