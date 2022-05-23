using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
public class ObstacleGenerator : EditorWindow
{
    // this class is used to create the custom editor to draw an grid on the editor window and to spawn obstacle on the game...

    List<List<Tile>> tilesMatrix;

    GUIStyle emptyStyle;
    GUIStyle tickStyle;
    Vector2 tilePosition;
    TileData tileData;
    [MenuItem("Tools/Obstacle Generator")]
    public static void ShowGeneratorWindow()
    {
       GetWindow(typeof(ObstacleGenerator));
        //ObstacleGenerator window = GetWindow<ObstacleGenerator>();
       // EditorApplication.EnterPlaymode();
    }
    float cellSize =50;
    #region Nope
    // int buttoniD = 0;
    //void OnGUI()
    //{
    //    GUI.BeginGroup(new Rect(Screen.width * 0.5f, Screen.height * 0.5f, 400, 400));

    //    GUI.Box(new Rect(0, 0, 230, 230), "tile positions");

    //    var off = 20f;
    //    var px = 20f;
    //    var py = 20f;
    //    //GUI.Button(new Rect(50 + px + off, 0 + off, 50, 50), "1");
    //    //GUI.Button(new Rect(0 + off, 50 + py + off, 50, 50), "2");
    //    //GUI.Button(new Rect(50 + px + off, 50 + py + off, 50, 50), "3");
    //    //GUI.Button(new Rect(100 + px * 2f + off, 50 + py + off, 50, 50), "4");
    //    //GUI.Button(new Rect(50 + px + off, 100 + py * 2 + off, 50, 50), "5");
    //    buttoniD = 0;
    //    for (int i = 0; i < 10; i++)
    //    {
    //        for (int j = 0; j < 10; j++)
    //        {
    //            buttoniD++;
    //            if( GUI.Button(new Rect(0 + px + off + (i * 24), 0 + off + (j * 24), 20, 20), buttoniD.ToString()))
    //            {

    //            }
    //        }
    //    }
    //    GUI.EndGroup();
    //}
    #endregion
    private void OnEnable()
    {

        
        cellSize = 20;
        emptyStyle = new GUIStyle();
        tickStyle = new GUIStyle();

        Texture2D emptyIcon = Resources.Load("Sprites/Cir") as Texture2D;
        emptyStyle.normal.background = emptyIcon;

        Texture2D tickIcon = Resources.Load("Sprites/Hex") as Texture2D;
        tickStyle.normal.background = tickIcon;
        SetupTiles();

        // tile data is a scriptable object which holds which grid has obstacles on them...
        // this is where we initialize the scriptable objects data matrix...
        tileData = Resources.Load("ScriptableObjects/TileData") as TileData;
        if (tileData)
        {
            tileData.gridDatas = new List<List<GridData>>();

            for (int i = 0; i < 10; i++)
            {
                tileData.gridDatas.Add(new List<GridData>());

                for (int j = 0; j < 10; j++)
                {
                    tileData.gridDatas[i].Add(new GridData());
                }
            }
        }

        //tileData.gridDatas.Reverse();
    }


    private void SetupTiles()
    {// this tile class is used as a grid in the CUSTOM EDITOR to store the details...
        tilesMatrix = new List<List<Tile>>();
        for (int i = 0; i < 10; i++)
        {
            tilesMatrix.Add(new List<Tile>());  
            for (int j = 0; j < 10; j++)
            {
                tilePosition.Set(i * 20, j * 20);
                tilesMatrix[i].Add(new Tile(tilePosition + new Vector2(5f,5f) ,10,10,emptyStyle));
            }
        }
    }
    private void OnGUI()
    {
        DrawGrid();
        DrawTiles();
        //ProcessGridDrag(Event.current);
        ProcessGridClick(Event.current);
        if (GUI.changed)
        {
            Repaint();
        }
    }

    private void DrawTiles()
    {
        for (int i = 0; i < 10; i++)
        {
            tilesMatrix.Add(new List<Tile>());
            for (int j = 9; j >=0; j--)
            {
                tilesMatrix[i][j].DrawOnCall();
            }
        }
    }

    Vector3 offset;
    //Vector2 drag;
    //private void ProcessGridDrag(Event e)
    //{
    //    drag = Vector2.zero;

    //    switch (e.type)
    //    {
    //        case EventType.MouseDrag:
    //            if(e.button == 0)
    //            {
    //                OnMouseDrag(e.delta);
    //            }
    //            break;
    //    }
    //}
    //private void OnMouseDrag(Vector2 delta)
    //{
    //    drag = delta;
    //    GUI.changed = true;
    //}

    // this is where we calculate which grid is selected when we use the CUSTOM EDITOR to map out the obstacle positions...
    private void ProcessGridClick(Event e)
    {
        int Row = (int)((e.mousePosition.x - offset.x) / cellSize);
        int Col = (int)((e.mousePosition.y - offset.y) / cellSize);

        if (GridSpawner.SpawnFinished == false)
            return;

        if (EntityManager.entityIsMoving)
            return;

        if(e.type == EventType.MouseDown && Row < 10 && Col < 10)
        {
            if (ObstacleManager.manager.GetGridMatrix()[Row][GetInversedValue(Col)].entityOccupied == true)
               return;
            int prevCol = Col;
            int inverseCol = GetInversedValue(Col); 

            Col = inverseCol;

            tilesMatrix[Row][Col].hasClicked = !tilesMatrix[Row][Col].hasClicked;
            if (tilesMatrix[Row][Col].hasClicked)
            {
                tilesMatrix[Row][prevCol].SetStyle(emptyStyle,Color.red);
            }
            else
            {
                tilesMatrix[Row][prevCol].SetStyle(emptyStyle,new Color(0,0,0,0));
            }
            GUI.changed = true;
            tileData.gridDatas[Row][Col].hasObstacle = tilesMatrix[Row][Col].hasClicked;
            if(ObstacleManager.manager)
            {
                ObstacleManager.manager.OnClickOnCellGUI();
            }
        }
    }

    // drawing the grid to visualize the grid position in game world...
    public void DrawGrid()
    {
        Handles.BeginGUI();
        Vector3 newOffset = new Vector3(offset.x % cellSize, offset.y % cellSize, 0);
        for (int i = 0; i < 11; i++)
        {
            Handles.DrawLine(new Vector3(i * cellSize, -cellSize, 0) + newOffset, new Vector3(i * cellSize,cellSize*10, 0) + newOffset);
            for (int j = 0; j < 11; j++)
            {
                Handles.DrawLine(new Vector3(-cellSize, j * cellSize, 0) + newOffset, new Vector3(cellSize * 10, cellSize * j, 0) + newOffset);
            }
        }
    }

    public int GetInversedValue(int input)
    {
        int valtoreturn = 9 - input;
        if(valtoreturn < 0)
        {
            valtoreturn *= -1;
        }
        return valtoreturn;
    }
    
}
