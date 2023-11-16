using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationGrid : MonoBehaviour
{

    [SerializeField]
    public int gridScale;
    [SerializeField]
    public Grid otherGrid;
    // Start is called before the first frame update
    void Start()
    {
        if (gridScale < 1)
            gridScale = 1; 
    }

    private void Awake() {
        var navGrid = gameObject.GetComponent<Grid>();
        
        navGrid.cellSize = otherGrid.cellSize / gridScale;
        navGrid.cellLayout = otherGrid.cellLayout;
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
