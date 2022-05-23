using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    List<List<GridCell>> grid;

    List<GridCell> openGrid;
    List<GridCell> closedGrid;

    const int DIAG_COST = 999999;
    const int STRA_COST = 10;

    GridCell endGridCell;
    public GridCell EndGrid()
    {
        return endGridCell;
    }

    // this function will iterate and return a group of grids which does not have obstacles on them...
    public List<GridCell> FindPathIterator(int startX,int startZ,int endX,int endZ)
    {
        grid = ObstacleManager.manager.GetGridMatrix();
        GridCell startGrid = grid[startX][startZ];
        endGridCell = grid[endX][endZ];



        openGrid = new List<GridCell>() { startGrid};
        closedGrid = new List<GridCell>();

        for (int x = 0; x < grid.Count; x++)
        {
            for (int z = 0; z < grid[x].Count; z++)
            {
                GridCell _cell = grid[x][z];
                _cell.gCost = 999999;
                _cell.CalculateFCost();
                _cell.lastGrid = null;
            }
        }

        startGrid.gCost = 0;
        startGrid.hCost = CalculateDistanceCost(startGrid, endGridCell);
        startGrid.CalculateFCost();

        while(openGrid.Count > 0)
        {
            GridCell currentGrid = GetLowestFcostNode(openGrid);
            if(currentGrid == endGridCell)
            {
                return CalculatePath(endGridCell);
            }

            openGrid.Remove(currentGrid);
            closedGrid.Add(currentGrid);

            foreach (GridCell neighbourGrid in GetNeighbourGrids(currentGrid))
            {
                if (closedGrid.Contains(neighbourGrid))
                {
                    continue;
                }

                if(neighbourGrid.hasObstacle ||neighbourGrid.entityOccupied)
                {
                    closedGrid.Add(neighbourGrid);
                    continue;
                }

                int unCertainGcost = currentGrid.gCost + CalculateDistanceCost(currentGrid, neighbourGrid);

                if(unCertainGcost < neighbourGrid.gCost)
                {
                    neighbourGrid.lastGrid = currentGrid;
                    neighbourGrid.gCost = unCertainGcost;
                    neighbourGrid.hCost = CalculateDistanceCost(neighbourGrid, endGridCell);
                    neighbourGrid.CalculateFCost();

                    if(!openGrid.Contains(neighbourGrid))
                    {
                        openGrid.Add(neighbourGrid);
                    }
                }
            }
        }

        return null;
    }
    // this function will return a list of neigbour cells... ( diagnally too)...
    private List<GridCell> GetNeighbourGrids(GridCell currentgrid)
    {
        List<GridCell> neighbourGrid = new List<GridCell>();

        if (currentgrid.x - 1 >= 0) 
        {
            neighbourGrid.Add(grid[currentgrid.x - 1][currentgrid.z]);
            #region Nada
            //if(currentgrid.z -1 <= 0 )
            //{
            //    if(neighbourGrid.Contains(grid[currentgrid.x-1][currentgrid.z-1]))
            //    {
            //        neighbourGrid.Remove(grid[currentgrid.x - 1][currentgrid.z - 1]);
            //    }
            //}
            //if(currentgrid.z + 1 > grid.Count)
            //{
            //    if(neighbourGrid.Contains(grid[currentgrid.x-1][currentgrid.z + 1]))
            //    {
            //        neighbourGrid.Remove(grid[currentgrid.x - 1][currentgrid.z + 1]);
            //    }
            //}
            #endregion
            if (currentgrid.z - 1 >= 0) 
            { 
                neighbourGrid.Add(grid[currentgrid.x - 1][currentgrid.z - 1]);
            }
            if (currentgrid.z + 1 < grid.Count) 
            {
                neighbourGrid.Add(grid[currentgrid.x - 1][currentgrid.z + 1]);
            }
        }

        if (currentgrid.x + 1 < grid.Count) 
        {
            neighbourGrid.Add(grid[currentgrid.x + 1][currentgrid.z]);
            #region Nada
            //if (currentgrid.z - 1 <= 0)
            //{
            //    if (neighbourGrid.Contains(grid[currentgrid.x + 1][currentgrid.z - 1]))
            //    {
            //        neighbourGrid.Remove(grid[currentgrid.x + 1][currentgrid.z - 1]);
            //    }
            //}

            //if(currentgrid.z + 1> grid[0].Count)
            //{
            //    if (neighbourGrid.Contains(grid[currentgrid.x + 1][currentgrid.z + 1]))
            //    {
            //        neighbourGrid.Remove(grid[currentgrid.x + 1][currentgrid.z + 1]);
            //    }
            //}
            #endregion
            if (currentgrid.z - 1 >= 0) 
            {
                neighbourGrid.Add(grid[currentgrid.x + 1][currentgrid.z - 1]);
            }
            if (currentgrid.z + 1 < grid.Count) 
            {
                neighbourGrid.Add(grid[currentgrid.x + 1][currentgrid.z + 1]);
            }
        }

        if (currentgrid.z - 1 >= 0)
        {
            neighbourGrid.Add(grid[currentgrid.x][currentgrid.z - 1]);
        }

        if (currentgrid.z + 1 < grid[0].Count)
        {
            neighbourGrid.Add(grid[currentgrid.x ][currentgrid.z + 1]);
        }
        return neighbourGrid;
    }

    private List<GridCell> CalculatePath(GridCell endGrid)
    {
        List<GridCell> gridPath = new List<GridCell>();
        gridPath.Add(endGrid);

        GridCell currentGrid = endGrid;
        while (currentGrid.lastGrid != null)
        {
            gridPath.Add(currentGrid.lastGrid);
            currentGrid = currentGrid.lastGrid;
        }
        gridPath.Reverse();
        return gridPath;       
    }

    int CalculateDistanceCost(GridCell a,GridCell b)
    {
        int xDist = Mathf.Abs(a.x - b.x);
        int zDist = Mathf.Abs(a.z - b.z);
        int remaining = Mathf.Abs(xDist - zDist);

        return DIAG_COST * Mathf.Min(xDist, zDist) + STRA_COST * remaining;
    }

    // this will return the cell with lowest cost...
    GridCell GetLowestFcostNode(List<GridCell> gridList)
    {
        GridCell lowestFcostnode = gridList[0];

        for (int i = 0; i < gridList.Count; i++)
        {
            if(gridList[i].fCost<lowestFcostnode.fCost)
            {
                lowestFcostnode = gridList[i];
            }
        }

        return lowestFcostnode;
    }
    public void GetXZ(Vector3 mousePosition, out int x, out int z)
    {
        x = Mathf.FloorToInt(mousePosition.x); z = Mathf.FloorToInt(mousePosition.z);
    }
    public List<List<GridCell>> GetGrid()
    {
        return grid;
    }
    // after finding the walkable grids this function will give a list of positions through which the player can walk through
    // to reach the destination...
    public List<Vector3> PathVectors(Vector3 startPosition,Vector3 endPosition)
    {
        GetXZ(startPosition, out int startX, out int startZ);
        GetXZ(endPosition, out int endX, out int endZ);

        List<GridCell> gridPath = FindPathIterator(startX, startZ, endX, endZ);
        if(gridPath == null)
        {
            return null;
        }
        else
        {
            List<Vector3> vectors = new List<Vector3>();
            foreach (GridCell grid in gridPath)
            {
                vectors.Add(grid.transform.position);
            }
            return vectors;
        }
        
    }
}
