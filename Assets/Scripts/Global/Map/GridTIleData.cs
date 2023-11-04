using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Tilemaps;
using UnityEngine.Tilemaps;
using System.Security.Cryptography;

[CreateAssetMenu]
public class GridTIleData : ScriptableObject
{
    public TileBase[] tiles;

    public float moveResistance;
    public float visualOpacity;
    public float surfaceSolidity;
    public bool walkable;
}
