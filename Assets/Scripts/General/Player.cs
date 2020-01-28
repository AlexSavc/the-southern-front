using System.Collections;
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
    [Header("Economy")]
    [SerializeField]
    private List<BuyableInfo> assets;


    public delegate void AddedCommanderDelegate(Commander added);
    public event AddedCommanderDelegate onCommanderAdded;


    public void Awake()
    {
        //Checks if it's null in case The node adds itself, you dont wanna overwrite it
        if(ownedNodes == null ) ownedNodes = new List<Node>();
        commanders = new List<Commander>();
        commanderParent = new GameObject("Commander Parent");
        commanderParent.transform.parent = transform;
        assets = new List<BuyableInfo>();
    }

    void Start()
    {
        TurnManager.Instance.onTurnStart += OnTurnStart;
        TurnManager.Instance.onTurnEnd += OnTurnEnd;

        //gold = starterGold;
        TempAddCommander();
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

    public float GetProjectedTaxes()
    {
        return CountRawTaxes();
    }

    public float GetProjectedExpenses()
    {
        return CountRawExpenses();
    }

    private float CountRawTaxes()
    {
        float tax = 0;
        foreach (BuyableInfo info in assets)
        {
            tax += info.revenue;
        }
        return tax;
    }

    private float CountRawExpenses()
    {
        float spend = 0;
        foreach (BuyableInfo info in assets)
        {
            spend += info.maintenance;
        }

        return spend;
    }

    public void RemoveGold(int amount)
    {
        gold -= amount;
    }
    private void CreateCommander()
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

        //ALL THIS IS TRASH TEST CODE CHANGE IT
        if(ownedNodes != null && ownedNodes.Count >= 1)
        GarrisonCommander(com, ownedNodes[0]);
    }

    private void GarrisonCommander(Commander comm, Node node)
    {
        if(node.GetOwner() == this)
        {
            node.GarrisonedCommanders.Add(comm);
            onCommanderAdded?.Invoke(comm);
        }
    }

    public void AddCommander()
    {
        CreateCommander();
    }

    public void OnTurnStart(Player player)
    {
        if (player != this) return;
        NodeTurnUpdate(true);
    }

    public void OnTurnEnd(Player player)
    {
        if (player != this) return;
        NodeTurnUpdate(false);
    }

    private void NodeTurnUpdate(bool turnStart)
    {
        if(turnStart)
        {
            foreach(Node node in ownedNodes)
            {
                node.OnTurnStart();
            }
        }
        else
        {
            foreach (Node node in ownedNodes)
            {
                node.OnTurnEnd();
            }
        }
    }

    public void AddBuyable(BuyableInfo info)
    {
        assets.Add(info);
        Economy.Instance.Refresh();
    }

    public void RemoveBuyable(BuyableInfo info)
    {
        assets.Remove(info);
        Economy.Instance.Refresh();
    }

    public void AddBuyableNoRefresh(BuyableInfo info)
    {
        assets.Add(info);
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