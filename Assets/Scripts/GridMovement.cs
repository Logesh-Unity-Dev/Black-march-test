using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    // this same class is used for both player and enemyAi movement...

    [SerializeField] private PathFinding PathFinding;

    protected List<Vector3> pathVectorList = new List<Vector3>();
    int currentPathIndex;
    [SerializeField] float speed;

    public bool isMoving = false;

    GridCell StartGrid = null;
    
    public GridCell EndGrid = null;

    public EntityManager eManager;

    public bool isPlayer;

    AnimationUpdater animationUpdater = null;
    void Start()
    {
        if(EntityManager.playerMovement == null)
        {
            EntityManager.playerMovement = this;
        }

        pathVectorList = null;
        GetStartGrid();
    }


    // this funcion will set the start grid, where player initially stands when spawned...
    public virtual void GetStartGrid()
    {
        PathFinding.GetXZ(transform.position, out int x, out int z);
        StartGrid = ObstacleManager.manager.GetGridMatrix()[x][z];
        Debug.Log($"x value : {x}, z value : {z}");
        StartGrid.entityOccupied = true;
        animationUpdater = GetComponent<AnimationUpdater>();
    }

    private void Update()
    {
        if (GridSpawner.SpawnFinished)
        {
            HandleMovement();
        }
    }

    // when we click on a grid this function will be called throuh the entity manager...
    // this function will use PATHFINDING class to create a walkable path...
    // and it will get a list of position in which the player will walkthough to reach destination...
    public virtual void OnClickGrid(Vector3 position, int _x, int _z)
    {
        PathFinding.GetXZ(position, out int x, out int z);
        if (isPlayer == false)
        {
            print($"x is {x} ,z is {z}");
        }
        List<Vector3> vectorsList = PathFinding.PathVectors(transform.position, new Vector3(x, 1, z));
        EndGrid = ObstacleManager.manager.GetGridMatrix()[x][z];
        if (vectorsList != null)
        {
            for (int i = 0; i < vectorsList.Count; i++)
            {
                vectorsList[i] += Vector3.up * 0.55f;
            }
            SetPathPositions(vectorsList);
        }
        else
        {
            Debug.LogWarning("Cannot find path to the point");
        }
        #region nada
        //if (path != null)
        //{
        //    print(path.Count);

        //    //    if (lineRenderer)
        //    //    {
        //    //        lineRenderer.positionCount = path.Count;
        //    //    }

        //    //    for (int i = 0; i < path.Count - 1; i++)
        //    //    {
        //    //        //if (lineRenderer)
        //    //        //{
        //    //        //    lineRenderer.SetPosition(i, new Vector3(path[i].x, 0, path[i].z) + Vector3.one * 0.5f);
        //    //        //}
        //    //        Debug.DrawLine(new Vector3(path[i].x, 0, path[i].z) + Vector3.one * 0.5f,
        //    //            new Vector3(path[i + 1].x, 0, path[i + 1].z) + Vector3.one * 0.5f, Color.red, 2f);
        //    //    }
        //    //}
        //    //List<GridCell> gridPath = PathFinding.FindPathIterator(0, 0, x, z);
        //    //if (gridPath != null)
        //    //{
        //    //    print(gridPath.Count);
        //    //    for (int i = 0; i < gridPath.Count; i++)
        //    //    {
        //    //       // Debug.DrawLine(new Vector3(gridPath[i].x, gridPath[i].z) * 10f + Vector3.one * 5f, new Vector3(gridPath[i + 1].x, gridPath[i + 1].z) * 10f + Vector3.one * 5f, Color.red,100f);
        //    //        if(i == 0)
        //    //        {
        //    //            Debug.DrawRay(new Vector3(gridPath[i].x,0,gridPath[i].z), Vector3.up * 2f, Color.red, 100f);
        //    //        }
        //    //        if(i == gridPath.Count-1)
        //    //        {
        //    //            Debug.DrawRay(new Vector3(gridPath[i].x,0, gridPath[i].z), Vector3.up * 2f, Color.red, 100f);
        //    //        }
        //    //    }
        //    //}

        //    List<Vector3> _vectorsList = PathFinding.PathVectors(transform.position, new Vector3(x, 1, z));

        //    if (_vectorsList != null) 
        //    {
        //        for (int i = 0; i < _vectorsList.Count; i++)
        //        {
        //            _vectorsList[i] += Vector3.up * 0.55f;
        //        }
        //        SetPathPositions(_vectorsList);
        //    }
        //    else
        //    {
        //        Debug.LogWarning("Cannot find path to the point");
        //    }
        //}
        //else
        //{
        //    Debug.LogWarning("Cannot find path to the point");
        //}
        #endregion
    }

    public void SetPathPositions(List<Vector3> _vectors_List)
    {
        currentPathIndex = 0;
        pathVectorList = _vectors_List;
        if (pathVectorList != null && pathVectorList.Count > 1) 
        {
            pathVectorList.RemoveAt(0);
        }

        PathFinding.GetXZ(transform.position, out int x, out int z);
        StartGrid = ObstacleManager.manager.GetGridMatrix()[x][z];
        StartGrid.entityOccupied = false;
    }
    // this function will be called through update to update the player's position...
    public virtual void HandleMovement()
    {
        if (pathVectorList != null)
        {
            Vector3 targetPosition = pathVectorList[currentPathIndex];
            if (Vector3.Distance(transform.position, targetPosition) > 0.05f)
            {
                Vector3 moveDir = (targetPosition - transform.position).normalized;
                transform.position = transform.position + moveDir * speed * Time.deltaTime;
                if(isMoving == false)
                {
                    isMoving = true;
                    EntityManager.entityIsMoving = true;
                    if (animationUpdater)
                    {
                        animationUpdater.UpdateAnimations(true);
                    }
                }
                transform.forward = moveDir;
                Debug.DrawRay(transform.position, moveDir * 0.2f, Color.red, 1f);
            }
            else
            {
                currentPathIndex++;
                if (currentPathIndex >= pathVectorList.Count)
                {
                    StopMoving();
                }
            }
        }
    }
    // this will called once the player reaches its destination on the grid...
    private void StopMoving() 
    {
        pathVectorList = null;
        isMoving = false;
        if (animationUpdater)
        {
            animationUpdater.UpdateAnimations(false);
        }
        EntityManager.entityIsMoving = false;
        if(EndGrid)
        {
            EndGrid.entityOccupied = true;
        }

        if(isPlayer)
        {
            eManager.OnPlayerStopMoving();
        }
    }
}
