using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    // this class holds the cell data of each grid's data...

    public bool hasObstacle = false;
    public bool entityOccupied = false;
    
    public Transform obstacleTransform = null;

    
    public int x, z;


    // using a*...

    [HideInInspector]
    public int gCost, hCost, fCost;

    public GridCell lastGrid;

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    // this function will return the neighor cells...
    public List<GridCell> GetNeighborGrids()
    {
        List<List<GridCell>> gridmatrix = ObstacleManager.manager.GetGridMatrix();
        List<GridCell> playerNeighborgrids = new List<GridCell>();

        if (x + 1 < gridmatrix.Count)
        {
            playerNeighborgrids.Add(gridmatrix[x + 1][z]);
        }
        if (x - 1 > 0)
        {
            playerNeighborgrids.Add(gridmatrix[x - 1][z]);
        }
        if (z + 1 < gridmatrix[0].Count)
        {
            playerNeighborgrids.Add(gridmatrix[x][z + 1]);
        }
        if (z - 1 > 0)
        {
            playerNeighborgrids.Add(gridmatrix[x][z - 1]);
        }
        return playerNeighborgrids;
    }
}
