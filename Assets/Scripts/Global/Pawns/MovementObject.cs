using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace STS{

    [Serializable]
    public class MovementParameters
    {
        [SerializeField]
        public float acceleration;
        [SerializeField]
        public float baseVelocity;
        [SerializeField]
        public float maxVelocity;
        [SerializeField]
        public float baseEndurance;
        [SerializeField]
        public float maxEndurance;
        
        public float curVelocity;

    }

    [CreateAssetMenu]
    public class MovementObject : ScriptableObject 
    {
        [SerializeField]
        public MovementParameters elements;
        public void Start()
        {

        }


        public void UpdateParameters(List<GridTileData> gridTileDatas)
        {
            float moveResistance = 1.0f;
            float visualOpacity = 1.0f;
            float surfaceSolidity = 1.0f;
            bool walkable = true;
            foreach (var data in gridTileDatas)
            {
                if(data is null)
                    continue;
                moveResistance *= data.moveResistance;
                visualOpacity *= data.visualOpacity;
                surfaceSolidity *= data.surfaceSolidity;
                walkable &= data.walkable;
            }
            float limVelocity = Math.Min(elements.maxVelocity/moveResistance, elements.baseVelocity*surfaceSolidity/moveResistance);  
            float incVelocity = elements.curVelocity < limVelocity? surfaceSolidity*elements.acceleration: 0;
            elements.curVelocity = Math.Min(elements.curVelocity+incVelocity, limVelocity);
        }
    }

}