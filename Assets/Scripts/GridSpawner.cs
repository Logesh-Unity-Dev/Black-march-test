using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GridSpawner : MonoBehaviour
{
    //this class is used to spawn the 10*10 grid...

    [Header("Grid Spawn Properties")]
    [SerializeField] Vector3 startPoint;
    [SerializeField] int gridXsize = 10, gridZsize = 10;
    [SerializeField] float heightOffsetY = 0f;
    const float gridCellSize = 1f;
    [SerializeField] GridCell gridPrefab = null;

    bool gridSpawned = false;
    int gridNumber = 0;

    public static bool SpawnFinished = false;

    [SerializeField] ObstacleManager obstacleMan = null;


    void Start()
    {
        if (gridSpawned == false)
            StartCoroutine(SpawnGrid());

        gridNumber = 1;
    }

    // this funtion will create a 10*10 grid...
    IEnumerator SpawnGrid()
    {
        gridSpawned = true;
        SpawnFinished = false;
        int yRot = 0;
        for (int x = 0; x < gridXsize; x++)
        {
            for (int z = 0; z < gridZsize; z++)
            {
                GridCell temp = Instantiate(gridPrefab, transform).transform.GetComponent<GridCell>();
                temp.x = x;
                temp.z = z;
                temp.transform.position = new Vector3(startPoint.x + x, startPoint.y + heightOffsetY, startPoint.z + z) * gridCellSize;
                temp.transform.Rotate(0, yRot, 0);
                gridNumber++;
                // every time a grid spawns its gets added in to a coloumn for an matrix...
                obstacleMan.AddGridCellOnSpawn(x, temp);
                yRot += 90;
                yield return new WaitForEndOfFrame();
                temp.name = gridNumber.ToString();
            }
        }
        SpawnFinished = true;
    }

}
