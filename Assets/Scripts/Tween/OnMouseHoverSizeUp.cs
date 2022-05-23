using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMouseHoverSizeUp : MonoBehaviour
{
    // this script is to scale up the grid which we select through the game...
    [SerializeField] Vector3 scaleUp = new Vector3(1,1.5f,1);
    [SerializeField] float timeToScaleUp;
    [SerializeField] LeanTweenType easetype;
    [SerializeField] LeanTweenType easetypeRaycast;
    public void OnMouseHover()
    {
        transform.LeanScale(scaleUp, timeToScaleUp).setEase(easetype).setOnComplete(()=> 
        {
            ScaleDown(timeToScaleUp);
        });
    }
    public void OnMouseHover(float scaleUptime,float scaleDownTime)
    {
        transform.LeanScale(scaleUp, scaleUptime).setEase(easetypeRaycast).setOnComplete(()=> 
        {
            ScaleDown(scaleDownTime);
        });
    }    

    void ScaleDown(float scaleDownTime)
    {
        transform.LeanScale(Vector3.one, scaleDownTime).setEase(easetypeRaycast);
    }
}
