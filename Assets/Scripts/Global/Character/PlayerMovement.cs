using Unity;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;

namespace STS{
    public class PlayerMovement: MoveablePawn
    {
        override protected void SetTargerPosition()
        {
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPosition.z = transform.position.z;
            state = GlobalPawnStates.MOVING;
        }
    }

}