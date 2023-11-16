using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

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

    public List<GridTileData> getTileData(Vector3 worldPos)
    {
            Vector3Int gridPos = mapGrid.WorldToCell(worldPos);
            var tileData = getTileDataGrid(gridPos);
            return tileData;
    }

    public GridTileData getNavTileData(Vector3 worldPos)
    {
        Vector3Int gridPos = mapGrid.WorldToCell(worldPos);
        var tileData = getTileDataGrid(gridPos);

        float moveResistance = 1.0f;
        float visualOpacity = 1.0f;
        float surfaceSolidity = 1.0f;
        bool walkable = true;
        foreach (var data in tileData)
        {
            if(data is null)
                continue;
            moveResistance *= data.moveResistance;
            visualOpacity *= data.visualOpacity;
            surfaceSolidity *= data.surfaceSolidity;
            walkable &= data.walkable;
        }
        return new GridTileData(){moveResistance=moveResistance,
                                  visualOpacity=visualOpacity,
                                  surfaceSolidity=surfaceSolidity,
                                  walkable=walkable};
    }

    public List<GridTileData> getTileDataGrid(Vector3Int gridPos)
    {
        var ret = new List<GridTileData>();
        foreach(var tilemap in mapLayers)
        {
            var tile = tilemap.GetTile(gridPos);
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
