using Unity;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;

namespace STS{
    public class Player: MoveablePawn
    {
        override protected void SetTargerPosition()
        {
            var Pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Pos.z = transform.position.z;
            PlanRoute(Pos);
            state = GlobalPawnStates.MOVING;
        }
    }

}