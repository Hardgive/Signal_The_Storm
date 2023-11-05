using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace STS{

    [CreateAssetMenu]
    public class MovementParameters : ScriptableObject 
    {
        [SerializeField]
        float acceleration;
        [SerializeField]
        float baseVelocity;
        [SerializeField]
        float maxVelocity;
        [SerializeField]
        float baseEndurance;
        [SerializeField]
        float maxEndurance;
        
        public float curVelocity;


        public void UpdateParameters(List<GridTIleData> gridTIleDatas)
        {
            float moveResistance = 1.0f;
            float visualOpacity = 1.0f;
            float surfaceSolidity = 1.0f;
            bool walkable = true;
            foreach (var data in gridTIleDatas)
            {
                if(data is null)
                    continue;
                moveResistance *= data.moveResistance;
                visualOpacity *= data.visualOpacity;
                surfaceSolidity *= data.surfaceSolidity;
                walkable &= data.walkable;
            }
            float limVelocity = Math.Min(maxVelocity/moveResistance, baseVelocity*surfaceSolidity/moveResistance);  
            float incVelocity = curVelocity < limVelocity? surfaceSolidity*acceleration: 0;
            curVelocity = Math.Min(curVelocity+incVelocity, limVelocity);
        }
    }

}