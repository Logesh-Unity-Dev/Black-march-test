using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSpawnSizeUp : MonoBehaviour
{
    [SerializeField] Vector3 size;
    [SerializeField] float time;
    [SerializeField] LeanTweenType LeanTweenType;

    // this is for the bounciness of the grid while spawning...
    private void OnEnable()
    {
        
        transform.localScale = Vector3.zero;
        transform.LeanScale(size, time).setEase(LeanTweenType).setOnComplete(() => { this.enabled = false; });
    }

}
