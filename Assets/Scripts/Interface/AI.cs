using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface AI 
{
    public void SetGridPosition(Vector3 gridPosition);

    public void CalculateMovement();
}
