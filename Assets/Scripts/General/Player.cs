﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private bool isPlaying;
    public bool IsPlaying() { return isPlaying; }

    public string playerName;
    public string team;
    public Color playerColor;
    public string type;

    [SerializeField]
    private int gold;
    public int CurrentGold() { return gold; }
    [SerializeField]
    private int starterGold = 10;

    [Header("Nodes")]
    [SerializeField]
    private List<Node> ownedNodes;
    public List<Node> GetNodesOwned() { return ownedNodes; }
    [Header("Commanders")]
    [SerializeField]
    private List<Commander> commanders;
    public List<Commander> Commanders { get { return commanders; } }
    public GameObject commanderParent;
    [Header("Units")]
    public GameObject UnitPrefab;

    public void Awake()
    {
        //Checks if it's null in case The node adds itself, you dont wanna overwrite it
        if(ownedNodes == null ) ownedNodes = new List<Node>();
        commanders = new List<Commander>();
        commanderParent = new GameObject("Commander Parent");
        commanderParent.transform.parent = transform;
        TempAddCommander();
        gold = 10;
    }

    void TempAddCommander()
    {
        CreateCommander();
        CreateCommander();
    }

    public void SetData(PlayerData data)
    {
        playerName = data.playerName;
        playerColor = data.playerColor;
        team = data.team;
        type = data.type;
        gold = data.gold;
    }

    public void SetPlaying(bool isHisTurn)
    {
        isPlaying = isHisTurn;
    }

    public void AddGold(int amount)
    {
        gold += amount;
    }

    public void RemoveGold(int amount)
    {
        gold -= amount;
    }
    public void CreateCommander()
    {
        GameObject commanda = new GameObject("Commander");
        commanda.transform.parent = commanderParent.transform;
        commanda.AddComponent<Commander>();
        Commander com = commanda.GetComponent<Commander>();
        Sprite temp = CommanderDataObject.Instance.faces[Random.Range(0, CommanderDataObject.Instance.faces.Count)];
        com.commaderName = temp.name;
        com.sprite = temp;
        com.rank = Commander.Rank.general;

        commanders.Add(com);
    }
}

[System.Serializable]
public class PlayerData
{
    public PlayerData() { }

    public string playerName;
    public string team;
    public Color playerColor;
    public string type;
    public int gold;

    public PlayerData(Player player)
    {
        playerName = player.playerName;
        team = player.team;
        playerColor = player.playerColor;
        type = player.type;
        gold = player.CurrentGold();
    }
    
    public void SetData(Player player)
    {
        playerName = player.playerName;
        team = player.team;
        playerColor = player.playerColor;
        type = player.type;
        gold = player.CurrentGold();
    }
}