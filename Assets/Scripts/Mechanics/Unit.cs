using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    public Sprite sprite;
    public Commander commander;

    public SpriteRenderer rend;

    [SerializeField]
    private int health;
    public int Health {  get { return health; } }

    [SerializeField]
    private int totalHealth;
    public int TotalHealth {  get { return totalHealth; }  }

    public int strength;
    
    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    public void SetTotalHealth(int total)
    {
        totalHealth = total;
    }

    public void SetHealth(int value)
    {
        health = value;
    }

    public void SetData(UnitInfo info)
    {

    }
}

[System.Serializable]
public class UnitInfo
{
    public void SetData(Unit unit)
    {

    }
}