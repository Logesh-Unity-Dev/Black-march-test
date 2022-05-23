using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// tile data is a scriptable object , it holds the matrix data of obstacle locations on the main grid...
// it can be accessed and obstacles can be spawn through the obstacle manager...

[CreateAssetMenu(fileName = "ScriptableObject/GridData")]
public class TileData : ScriptableObject
{
    public List<RowData> RowList;


    public List<List<GridData>> gridDatas;

    [System.Serializable]
    public class RowData
    {
        public List<ColoumnData> ColoumnList;
    }

    [System.Serializable]
    public class ColoumnData
    {
        public bool hasObstacle = false;
    }

}

[System.Serializable]
public class GridData
{
    public bool hasObstacle = false;
}
