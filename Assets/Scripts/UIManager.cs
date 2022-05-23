using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI ;
using TMPro;

public class UIManager : MonoBehaviour
{
    // uimanager holds all the reference and functions to all the UI throught the game scene...

    public static UIManager instance = null;
    [SerializeField] RectTransform displayTextObj;
    [SerializeField] RectTransform canvas;
    [SerializeField] TextMeshProUGUI gridPositionHolder = null;

    [SerializeField] Button playerbtn = null;
    [SerializeField] Button enemybtn = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    // this function calls to update the UI, it will update specific grid's id to the ui...
    public void SetDisplayGridValues(Vector3 pos)
    {
        gridPositionHolder.text = $"{pos.x} , {pos.z}" ;
        // setting the ui elements postion and text...
        if(displayTextObj.gameObject.activeInHierarchy == false)
            displayTextObj.gameObject.SetActive(true);

        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, pos + new Vector3(0.5f, 0.5f, 0.5f));
        displayTextObj.anchoredPosition = transform.InverseTransformPoint(screenPoint) ;
    }


    // this function calls once we stop pointing the mouse cursor at any grid...
    public void CloseDisplayGridValues()
    {
        if (displayTextObj.gameObject.activeInHierarchy == true)
            displayTextObj.gameObject.SetActive(false);
    }

    // this function is called through buttons to change the spawner type...
    public void Button_SelectUnitType(int id)
    {
        if(id == 0)
        {
            EntityManager.SelectPlayerUnit();
        }
        else
        {
            EntityManager.SelectEnemyUnit();
        }
    }

    // this function will deactivate the Player and enemy selection in ui...
    public void OnSpawnDeactivate(int id)
    {
        if(id == 1)
        {
            playerbtn.gameObject.SetActive(false);
        }
        else
        {
            enemybtn.gameObject.SetActive(false);
        }
    }
}
