using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : GridMovement,AI
{
    // this class is inherited from both GRIDMOVEMENT class and AI interface... 

    [SerializeField] GridCell playerGrid;

    private void Start()
    {
        pathVectorList = null;
        GetStartGrid();
    }
    private void Update()
    {
        if (GridSpawner.SpawnFinished)
        {
            HandleMovement();
        }
    }

    public void CalculateMovement()
    {
        playerGrid = EntityManager.playerMovement.EndGrid;
        List<GridCell> playerNeighborgrids = playerGrid.GetNeighborGrids();
        float distanceToClosestEnemy = Mathf.Infinity;
        GridCell closestGrid = null;

        foreach (GridCell cg in playerNeighborgrids)
        {
            if (cg.hasObstacle || cg.entityOccupied)
                continue;

            float distanceToEnemy = (cg.transform.position - this.transform.position).sqrMagnitude;
            if (distanceToEnemy < distanceToClosestEnemy)
            {
                distanceToClosestEnemy = distanceToEnemy;
                closestGrid = cg;
            }
        }

        SetGridPosition(closestGrid.transform.position);
    }

    public void SetGridPosition(Vector3 gridPosition)
    {
        OnClickGrid(gridPosition, 0, 0);
    }


}
