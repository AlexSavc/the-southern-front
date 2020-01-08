using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSpriteCollider : MonoBehaviour, ISelectable
{
    public Road parentObj;

    void Awake()
    {
        if(parentObj == null) parentObj = GetComponentInParent<Road>();
    }

    public void OnSelection()
    {
        parentObj.OnSelection();
    }

    public void OnDeSelection()
    {
        parentObj.OnDeSelection();
    }
}
