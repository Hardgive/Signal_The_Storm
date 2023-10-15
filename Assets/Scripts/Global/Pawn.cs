using Unity;
using UnityEditor;
using UnityEngine;
using STS;

namespace STS {
public class Pawn: MonoBehaviour
{
    //! Pawn is a basic object on a map.
    
    public GlobalPawnStates state = GlobalPawnStates.IDLE;
}
}