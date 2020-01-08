using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class TurnManager : MonoBehaviour
{
    private static TurnManager _instance;
    public static TurnManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public Player[] players;
    public Map map;
    public Player currentPlayer;
    public GameObject NextPlayerScreen;
    public Economy economy;
    public NodeMenu nodeMenu;
    public TextMeshProUGUI nextPlayerTurn;

    public delegate void TurnEndDelegate(Player player);
    public event TurnEndDelegate onTurnEnd;

    public delegate void TurnStartDelegate(Player player);
    public event TurnStartDelegate onTurnStart;

    public delegate void FirstTurnDelegate(Player player);
    public event TurnStartDelegate onFirstTurn;

    public int turn = 0;

    void Awake()
    {
        _instance = this;
    }

    public void Start()
    {
        if(map == null) map = FindObjectOfType<Map>();
        if(economy == null) economy = FindObjectOfType<Economy>();
        if(nodeMenu == null) nodeMenu = FindObjectOfType<NodeMenu>();

        onTurnEnd += economy.OnTurnEnd;
        onTurnStart += economy.OnTurnStart;
    }

    /*HERE MAY BE DRAGONS*/
    //OnNodesAddedToOwners triggers TurnManager.FirstTurn. Do NOT call it ANYWHERE except once in Map.

    public void NextTurn()
    {
        map.SaveMap();
        EndTurn();
        if (currentPlayer == null) currentPlayer = players[0];
        else currentPlayer = NextPlayer();
        ShowNextPlayerScreen();
    }

    public void FirstTurn()
    {
        onFirstTurn?.Invoke(currentPlayer);
        CenterCameraOnPlayer();
        map.SaveMap();
        ShowNextPlayerScreen();
    }

    public void FirstTurnOnLoad(Player player)
    {
        map.SaveMap();
        currentPlayer = player;
        ShowNextPlayerScreen();
    }

    public void OnPlayersGenerated()
    {
        GetPlayers();
    }

    public void OnNodesAddedToOwners()
    {
        //This, I think, is called after OnPlayerGenerated is triggered by an event in Map.
        if (currentPlayer == null) currentPlayer = players[0];
        SetEconomyPlayers();
        FirstTurn();
    }

    public void ShowNextPlayerScreen()
    {
        nodeMenu.OpenNextPlayerWindow();
        nextPlayerTurn.text = "It's " + currentPlayer.playerName + "'s turn";
    }

    public void BeginTurn()
    {
        turn++;
        CenterCameraOnPlayer();
        nodeMenu.CloseNextPlayerWindw();
        onTurnStart?.Invoke(currentPlayer);
    }

    public void EndTurn()
    {
        if (currentPlayer == null) return;
        onTurnEnd?.Invoke(currentPlayer);
    }

    public Player NextPlayer()
    {
        int i = Array.IndexOf(players, currentPlayer);
        if (i >= players.Length - 1)
        {
            i = 0;
        }
        else i++;
        return players[i];
    }

    public void GetPlayers()
    {
        List<GameObject> childern = Utility.GetChildren(transform);
        List<Player> newPlayers = new List<Player>();
        foreach(GameObject obj in childern)
        {
            newPlayers.Add(obj.GetComponent<Player>());
        }
        players = newPlayers.ToArray();
    }

    public void SetEconomyPlayers()
    {
        economy.SetPlayers(players);
    }

    public void CenterCameraOnPlayer()
    {
        /*if(currentPlayer.GetNodesOwned() == null)
        {
            Camera.main.transform.position = new Vector3(1, 1, -10);
            return;
        }
        else*/
        try
        {
            int x = currentPlayer.GetNodesOwned()[0].X;
            int y = currentPlayer.GetNodesOwned()[0].Y;
            Camera.main.transform.position = new Vector3(x, y, -10);
        }
        catch(System.ArgumentOutOfRangeException)
        {
            Camera.main.transform.position = new Vector3(1, 1, -10);
            return;
        }
    }
}
