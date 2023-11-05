using System.Collections;
using System.Collections.Generic;
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

    public List<GridTIleData> getTileData(Vector3 worldPos)
    {
            Vector3Int gridPos = mapCoordinateGrid.WorldToCell(worldPos);
            var tileData = getTileDataGrid(gridPos);
            return tileData;
    }

    public List<GridTIleData> getTileDataGrid(Vector3Int gridPos)
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
            GridTIleData data;
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
