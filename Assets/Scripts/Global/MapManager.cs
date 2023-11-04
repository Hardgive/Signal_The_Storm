using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    [SerializeField]
    public Grid mapCoordinateGrid;
    [SerializeField]
    public List<Tilemap> mapLayers;

    [SerializeField]
    private List<GridTIleData> tileData;

    private Dictionary<TileBase, GridTIleData> mapTileData;

    void Awake()
    {
        mapTileData = new Dictionary<TileBase, GridTIleData>();

        foreach(var data in tileData)
        {
            foreach(var tile in data.tiles)
            {
                mapTileData.Add(tile, data);
            }
        }
    }

    private List<GridTIleData> getTileData(Vector3Int gridPos)
    {
        var ret = new List<GridTIleData>();
        foreach(var tilemap in mapLayers)
        {
            var tile = tilemap.GetTile(gridPos);
            if (tile is null)
            {
                ret.Add(null);
                continue;
            }
            GridTIleData data = new GridTIleData();
            if (mapTileData.TryGetValue(tile, out data))
            {
                ret.Add(data);
            }
            else
            {
                ret.Add(null);
            }
        }
        return ret;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPos = mapCoordinateGrid.WorldToCell(mousePos);

            var tileData = getTileData(gridPos);
            string tileStateMsg = "";

            foreach(var data in tileData)
            {
                if (data is null) continue;
                tileStateMsg = tileStateMsg + 
                    (string.Join(
                        "; ", new List<string>(){data.name,
                                                 data.moveResistance.ToString(),
                                                 data.visualOpacity.ToString()}
                        )
                        +' '
                        );
            }
            print("Clicked "+ mousePos + " tile "+ tileStateMsg);   

            // GridTIleData tiledata = new GridTIleData();
            // if(mapTileData.TryGetValue(tile, out tiledata))
            // {
            //     print("Clicked "+ mousePos + " tile "+ tiledata);   
            // }
            // else
            // {
            //     print("Clicked "+ mousePos + " tile not found ");   
            // }
            
            
        }
    }
}
