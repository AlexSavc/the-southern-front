using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commander : MonoBehaviour, IGarrison
{
    public Sprite sprite;
    public string commaderName;
    public CommanderDataObject commanderDataObject;

    public bool legendary;

    public enum Rank { ranksless, officer, captain, major, general }
    public Rank rank;
    public Sprite rankSprite;

    [SerializeField]
    private List<Unit> units;
    public List<Unit> Units
    {
        get
        {
            return units;
        }
    }

    public List<Unit> Garrison
    {
        get { return units; }
        set { /*units = value;*/ }
    }


    public void Start()
    {
        rank = new Rank();
        commanderDataObject = FindObjectOfType<CommanderDataObject>();
        units = new List<Unit>();
    }

    public void AddUnit(Unit unit)
    {
        if (units.Contains(unit)) return;

        units.Add(unit);
    }

    public void RemoveUnit(Unit unit)
    {
        units.Remove(unit);
    }

    public Sprite GetRankSprite()
    {
        return commanderDataObject.rankSprites[(int)rank];
    }
}

[System.Serializable]
public class CommanderInfo
{

}

