using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastTouch : MonoBehaviour
{
    // this class is used to detect which tile the mouse cursor is currently at...

    Ray ray;
    RaycastHit rayHit ;
    float distance = 100f;

    public bool canSelectGridCell = true;

    [SerializeField] UIManager uiManager;
    [SerializeField] GridMovement testScript;
    private void Update()
    {
        if (canSelectGridCell )
        {
            CalCulateTouchPosition();
        }
    }

    void CalCulateTouchPosition()
    {
        // calculating Mouse Position...
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out rayHit, distance))
        {
            GridCell cell = rayHit.transform.GetComponent<GridCell>();
            if (cell)
            {
                uiManager.SetDisplayGridValues(new Vector3(cell.x, 0, cell.z));

                Vector3 hitPosition = rayHit.transform.position + new Vector3(0, 0.5f, 0);
                if (Input.GetMouseButtonDown(0) && EntityManager.entityIsMoving == false) 
                {
                    if(EntityManager.GetSpawnSelected())
                    {
                        //this is where we 
                        GameObject.FindObjectOfType<EntityManager>().SpawnUnitOnClick(hitPosition);
                        return;
                    }
                    // passing the position of the grid which is selected in runtime...
                    // this will call the pathfinding to reach that grid's destination...
                    EntityManager.playerMovement?.OnClickGrid(hitPosition, cell.x, cell.z);
                    if (rayHit.transform.GetComponent<OnMouseHoverSizeUp>())
                    {
                        rayHit.transform.GetComponent<OnMouseHoverSizeUp>().OnMouseHover();
                    }
                    if (cell.entityOccupied || cell.hasObstacle)
                    {
                        AudioManager.instance.PlayAudio("TileOccupied");
                        return;
                    }
                    AudioManager.instance.PlayAudio("TileClick");
                }

            }
            
        }
        else
        {
            uiManager.CloseDisplayGridValues();
            // when we select off grid it will nullify the spawn selection...
            if (Input.GetMouseButtonDown(0))
            {
                if (EntityManager.GetSpawnSelected())
                {
                    EntityManager.NothingSelected();
                }
            }
        }
    }
}
