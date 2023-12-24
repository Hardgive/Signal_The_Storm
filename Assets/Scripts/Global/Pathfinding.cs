using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Aoiti.Pathfinding;
using Unity.VisualScripting;
using System.Numerics;
using System.Linq;
using System;

namespace STS{

public class Pathfinding
{

    public Pathfinder<Vector3Int> pathfinder;
    public MovementObject _movement;
    private enum Direction { l, lu, ru, r, rd, ld};
    public Pathfinding(MovementObject movement)
    {
        _movement = movement;
        pathfinder = new Pathfinder<Vector3Int>(DistanceFunc, connectionsAndCosts);
    }

    public float DistanceFunc(Vector3Int a, Vector3Int b)
    {
        return (a-b).sqrMagnitude;
    }

    public Dictionary<Vector3Int,float> connectionsAndCosts(Vector3Int a)
    {
        bool mod = ((a.x+a.y)%2) == 1;

        Dictionary<Vector3Int, float> result = new Dictionary<Vector3Int, float>();

        Dictionary<Direction, Vector3Int> directions = new Dictionary<Direction, Vector3Int>(){
            {Direction.r, new Vector3Int(1, 0)+a},
            {Direction.l, new Vector3Int(-1, 0)+a},
            {Direction.ru, new Vector3Int(Convert.ToInt32(mod), 1)+a},
            {Direction.rd, new Vector3Int(Convert.ToInt32(mod), -1)+a},
            {Direction.lu, new Vector3Int(Convert.ToInt32(!mod), 1)+a},
            {Direction.ld, new Vector3Int(Convert.ToInt32(!mod), -1)+a},
        };        
        Dictionary<Direction, float> costs = new Dictionary<Direction, float>();
        
        var currentDif = _movement.GetMovementDifficulty(MapManager.instance.getTileDataFromNav(a));

        for(int i = 0; i < directions.Count; ++i)
        {   
            var direction = directions.ElementAt(i);
            var tileData = MapManager.instance.getTileDataFromNav(direction.Value);
            var cost = _movement.GetMovementDifficulty(tileData);
            costs.Add( direction.Key, cost);
            result.Add( direction.Value, cost+currentDif);
        }


        var up = new Vector3Int(a.x, a.y+2);
        var down = new Vector3Int(a.x, a.y-2);
        var up_cost = _movement.GetMovementDifficulty(MapManager.instance.getTileDataFromNav(up));
        var down_cost = _movement.GetMovementDifficulty(MapManager.instance.getTileDataFromNav(down));
        result.Add(up, ((costs[Direction.ru]+costs[Direction.lu]+2*currentDif) + (costs[Direction.ru]+costs[Direction.lu]+2*up_cost))*1.723f*0.25f);
        result.Add(down, ((costs[Direction.ru]+costs[Direction.lu]+2*currentDif) + (costs[Direction.ru]+costs[Direction.lu]+2*down_cost))*1.723f*0.25f);
        
        return result;
    }


    public List<Vector3Int> GetPath(Vector3Int start, Vector3Int destination)
    {
        List<Vector3Int> path = new List<Vector3Int>();

        pathfinder.GenerateAstarPath(start, destination, out path);

        return path;
    }

    //Start and Destination are world coordinates
    public List<UnityEngine.Vector3> GetPath(UnityEngine.Vector3 start, UnityEngine.Vector3 destination)
    {
        List<Vector3Int> path = new List<Vector3Int>();
        Vector3Int startNode = MapManager.instance.WorldToNavCoords(start);
        Vector3Int endNode = MapManager.instance.WorldToNavCoords(destination);
        pathfinder.GenerateAstarPath(startNode, endNode, out path);

        List<UnityEngine.Vector3> ret = path.Select(x=> MapManager.instance.NavToWorldCoords(x)).ToList();

        return ret;
    }
}
}