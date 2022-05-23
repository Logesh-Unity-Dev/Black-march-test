using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    //this class controls and holds the reference of the player and enemy...
    public static bool entityIsMoving = false;
    List<List<GridCell>> gridCells;

    [SerializeField] GridMovement playerPrefab, enemyPrefab;

    static bool playerSpawned = false;
    static bool enemySpawned = false;

    [HideInInspector]
    public static GridMovement playerMovement;
    EnemyAI enemyMovement;

    public static bool playerSpawnSelected = false;
    public static bool enemySpawnSelected = false;
    Vector3 positionToSpawn = Vector3.zero;
    
    private void Start()
    {
       gridCells = ObstacleManager.manager.GetGridMatrix();
    }

    private void Update()
    {
        // this is to override the spawnfunctions for testing purpose..

        //if(Input.GetKeyDown(KeyCode.P) && !playerSpawned)
        //{
        //    // spawn player...
        //    StartCoroutine(GetRandomNumbers(playerPrefab));
        //    UIManager.instance.OnSpawnDeactivate(1);
        //    playerSpawnSelected = false;
        //    playerSpawned = true;
        //}
        //if(Input.GetKeyDown(KeyCode.E) && playerSpawned && !enemySpawned)
        //{
        //    // spawn player...
        //    StartCoroutine(GetRandomNumbers(enemyPrefab));
        //    UIManager.instance.OnSpawnDeactivate(12);
        //    enemySpawnSelected = false;
        //    enemySpawned = true;
        //}
    }
    int randomX;
    int randomZ;
    public void SpawnUnit(GridMovement unit)
    {
        GridMovement temp = Instantiate(unit.gameObject, /*new Vector3(randomX - 0.5f, 0.55f, randomZ - 0.5f)*/ positionToSpawn, Quaternion.identity).GetComponent<GridMovement>();
        if(temp.TryGetComponent<EnemyAI>(out EnemyAI ai))
        {
            enemyMovement = ai;
            temp.isPlayer = false;
        }
        else
        {
            temp.isPlayer = true;
        }
        temp.eManager = this;

        AudioManager.instance.PlayAudio("ObstacleSpawn");
    }

    //IEnumerator GetRandomNumbers(GridMovement unit)
    //{
    //    bool canSpawn = true;
    //    randomX = Random.Range(1, gridCells.Count - 1);
    //    randomZ = Random.Range(1, gridCells[0].Count - 1);
    //    yield return new WaitForEndOfFrame();

    //    if(gridCells[randomX][randomZ].hasObstacle || gridCells[randomX][randomZ].entityOccupied)
    //    {
    //        canSpawn = false;
    //        StartCoroutine(GetRandomNumbers(unit));
    //        print($"Grid occupied in ({randomX} , {randomZ})");
    //        yield return null;
    //    }
    //    else
    //    {
    //        canSpawn = true;
    //    }
    //    if (canSpawn)
    //        SpawnUnit(unit);
    //}

    public void OnPlayerStopMoving()
    {
        if (enemyMovement)
            enemyMovement.CalculateMovement();
    }


    // this functions call when the spawn selection is called through ui manager...
    public static void SelectPlayerUnit()
    {
        playerSpawnSelected = !playerSpawnSelected;
        print("player spawn selected");
    }
    public static void SelectEnemyUnit()
    {
        if (!playerSpawned)
            return;
        enemySpawnSelected = !enemySpawnSelected;
        print("enemy spawn selected");
    }

    public static bool GetSpawnSelected()
    {
        if(playerSpawnSelected || enemySpawnSelected)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // while selecting spawn option when we click on a grid its position is passed throught to spawn an unit on that specific grid...
    public void SpawnUnitOnClick(Vector3 position)
    {
        positionToSpawn = position;
        if(playerSpawnSelected && playerSpawned == false)
        {
            SpawnUnit(playerPrefab);
            UIManager.instance.OnSpawnDeactivate(1);
            playerSpawnSelected = false;
            playerSpawned = true;
        }
        else if(enemySpawnSelected && enemySpawned == false)
        {
            SpawnUnit(enemyPrefab);
            UIManager.instance.OnSpawnDeactivate(99);
            enemySpawnSelected = false;
            enemySpawned = true;
        }
    }

    // when we click on empty space the selections will be nullified...
    public static void NothingSelected()
    {
        playerSpawnSelected = false;
        enemySpawnSelected = false;
    }
}
