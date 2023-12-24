using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Tilemaps;
using UnityEngine.Tilemaps;
using System.Security.Cryptography;
using System.Reflection.Emit;

public class TileData
{
    public TileData(GridTileData data)
    {
        moveResistance=data.moveResistance;
        visualOpacity=data.visualOpacity;
        surfaceSolidity=data.surfaceSolidity;
        walkable=data.walkable;
    }
    public TileData(float moveResistance, float visualOpacity, float surfaceSolidity, bool walkable)
    {
        this.moveResistance=moveResistance;
        this.visualOpacity=visualOpacity;
        this.surfaceSolidity=surfaceSolidity;
        this.walkable=walkable;
    }
    public TileData()
    {
    }
    public float moveResistance;
    public float visualOpacity;
    public float surfaceSolidity;
    public bool walkable;
}

[CreateAssetMenu]
public class GridTileData : ScriptableObject
{
    public TileBase[] tiles;

    public float moveResistance;
    public float visualOpacity;
    public float surfaceSolidity;
    public bool walkable;

}

