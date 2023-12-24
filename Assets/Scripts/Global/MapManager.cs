using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace STS{


public class MapManager : MonoBehaviour
{    
    public static MapManager instance = null; // Экземпляр объекта

    // Метод, выполняемый при старте игры
    void Start () {
        // Теперь, проверяем существование экземпляра
	    if (instance == null) { // Экземпляр менеджера был найден
	        instance = this; // Задаем ссылку на экземпляр объекта
	    } else if(instance == this){ // Экземпляр объекта уже существует на сцене
	        Destroy(gameObject); // Удаляем объект
	    }
	    DontDestroyOnLoad(gameObject);
	    InitializeManager();
    }

    // Метод инициализации менеджера
    private void InitializeManager(){
        /* TODO: Здесь мы будем проводить инициализацию */
    }

    [SerializeField]
    public Grid mapGrid;
    [SerializeField]
    public Grid navigationGrid;
    [SerializeField]
    public List<Tilemap> mapLayers;

    [SerializeField]
    private List<GridTileData> tileData;

    private Dictionary<TileBase, GridTileData> mapTileData;

    void Awake()
    {
        mapTileData = new Dictionary<TileBase, GridTileData>();

        foreach(var data in tileData)
        {
            foreach(var tile in data.tiles)
            {
                mapTileData.Add(tile, data);
            }
        }
    }

    public List<TileData> getTileData(Vector3 worldPos)
    {
            Vector3Int gridPos = mapGrid.WorldToCell(worldPos);
            var tileData = getTileDataGrid(gridPos).Select(x=>new TileData(x)).ToList();
            return tileData;
    }
    public TileData getTileDataMerged(Vector3 worldPos)
    {
            Vector3Int gridPos = mapGrid.WorldToCell(worldPos);
            var tileData = getTileDataGrid(gridPos);
            return this.mergeTileData(tileData);
    }

    public Vector3Int WorldToNavCoords(Vector3 worldPos)
    {
        var ret = navigationGrid.WorldToCell(worldPos);
        return ret;
    }

    public Vector3Int WorldToMapCoords(Vector3 worldPos)
    {
        var ret = mapGrid.WorldToCell(worldPos);
        return ret;
    }

    public Vector3 NavToWorldCoords(Vector3Int navPos)
    {
        var ret = navigationGrid.CellToWorld(navPos);
        return ret;
    }

    public Vector3 MapToWorldCoords(Vector3Int mapPos)
    {
        var ret = mapGrid.CellToWorld(mapPos);
        return ret;
    }

    public Vector3Int NavToMapCoords(Vector3Int navPos)
    {
        return mapGrid.WorldToCell(navigationGrid.CellToWorld(navPos));
    }

    public Vector3Int MapToNavCoords(Vector3Int mapPos)
    {
        return navigationGrid.WorldToCell(mapGrid.CellToWorld(mapPos));
    }


    public TileData mergeTileData(List<GridTileData> dataList)
    {
        float moveResistance = 1.0f;
        float visualOpacity = 1.0f;
        float surfaceSolidity = 1.0f;
        bool walkable = true;
        foreach (var data in dataList)
        {
            if(data is null)
                continue;
            moveResistance *= data.moveResistance;
            visualOpacity *= data.visualOpacity;
            surfaceSolidity *= data.surfaceSolidity;
            walkable &= data.walkable;
        }
        return new TileData(){moveResistance=moveResistance,
                                  visualOpacity=visualOpacity,
                                  surfaceSolidity=surfaceSolidity,
                                  walkable=walkable};

    }

    public TileData getTileDataFromWorld(Vector3 worldPos)
    {
        Vector3Int gridPos = mapGrid.WorldToCell(worldPos);
        var tileData = getTileDataGrid(gridPos);
        return mergeTileData(tileData);
    }

    public TileData getTileDataFromNav(Vector3Int navPos)
    {
        Vector3Int mapPos = this.NavToMapCoords(navPos);
        var tileData = getTileDataGrid(mapPos);
        return mergeTileData(tileData);
    }
        public TileData getTileDataFromMap(Vector3Int mapPos)
    {
        var tileData = getTileDataGrid(mapPos);
        return mergeTileData(tileData);
    }

    public List<GridTileData> getTileDataGrid(Vector3Int mapPos)
    {
        var ret = new List<GridTileData>();
        foreach(var tilemap in mapLayers)
        {
            var tile = tilemap.GetTile(mapPos);
            if (tile is null)
            {
                ret.Add(null);
                continue;
            }
            GridTileData data;
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
            Vector3Int gridPos = mapGrid.WorldToCell(mousePos);
            Vector3Int navPos = navigationGrid.WorldToCell(mousePos);

            var tileData = getTileDataGrid(gridPos);
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
            print("Clicked "+ mousePos + " = ("+ gridPos +"):(" + navPos + "); " + "tile " + tileStateMsg);   

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
}