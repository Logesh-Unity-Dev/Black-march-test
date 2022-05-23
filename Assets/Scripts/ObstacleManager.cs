using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    // this class is responsible for spawning the obstacle on a grid when we select the CUSTOM TOOL to create obstacle...

    public TileData tileData;

    public static ObstacleManager manager;
    List<List<GridCell>> spawnedCells;

    [SerializeField] Transform ObstaclePrefab;

    private void Awake()
    {
        if (!manager)
        {
            manager = this;
        }
        else
        {
            Destroy(this);
            return;
        }

        tileData = Resources.Load("ScriptableObjects/TileData") as TileData;
        SetupGridMatrix();
    }

    // here we are initializing the grid matrix...
    void SetupGridMatrix()
    {
        spawnedCells = new List<List<GridCell>>();
        for (int x = 0; x < 10; x++)
        {
            spawnedCells.Add(new List<GridCell>());
        }
    }

    // adding the newly spawned gridcell component to the initially initialized grid matrix...
    public void AddGridCellOnSpawn(int row,GridCell gridCell)
    {
        spawnedCells[row].Add(gridCell);
    }

    // When the CUSTOM EDITOR is used the function here will get called which will take a read on what tiles are selected.
    // and it will spawn and remove obstacle on that specific grid...
    public void OnClickOnCellGUI()
    {
        for (int i = 0; i < spawnedCells.Count; i++)
        {
            for (int j = 0; j < spawnedCells[i].Count; j++)
            {
               // spawnedCells[i][j].gameObject.SetActive(!tileData.gridDatas[i][j].hasObstacle);
                spawnedCells[i][j].hasObstacle = tileData.gridDatas[i][j].hasObstacle;

                if (tileData.gridDatas[i][j].hasObstacle)
                {
                    if (spawnedCells[i][j].obstacleTransform == null)
                    {
                        spawnedCells[i][j].obstacleTransform = Instantiate(ObstaclePrefab, spawnedCells[i][j].transform.position
                           + new Vector3(0, 1, 0), spawnedCells[i][j].transform.rotation, spawnedCells[i][j].transform);

                        AudioManager.instance.PlayAudio("ObstacleSpawn");
                    }

                }
                else
                {
                    if (spawnedCells[i][j].obstacleTransform != null)
                    {
                        Destroy(spawnedCells[i][j].obstacleTransform.gameObject);
                        AudioManager.instance.PlayAudio("ObstacleDespawn");
                    }
                }
            }
        }
    }

    public List<List<GridCell>> GetGridMatrix()
    {
        return spawnedCells;
    }
}
