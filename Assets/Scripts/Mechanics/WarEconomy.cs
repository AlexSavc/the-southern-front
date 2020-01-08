using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarEconomy : MonoBehaviour
{
    private static WarEconomy _instance;
    public static WarEconomy Instance
    {
        get
        {
            return _instance;
        }
    }
    
    public Unit militia;

    public void Awake()
    {
        _instance = this;
    }

    public int GetPriceOfNewCommander()
    {
        return 1;
    }

    public int GetPriceOfNewUnit()
    {
        return 1;
    }
}
