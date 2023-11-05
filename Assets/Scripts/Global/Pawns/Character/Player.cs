using Unity;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;

namespace STS{
    public class Player: MoveablePawn
    {
        override protected void SetTargerPosition()
        {
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPosition.z = transform.position.z;
            state = GlobalPawnStates.MOVING;
        }
    }

}