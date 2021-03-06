﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour, ISelectable, IInteractable, IGarrison
{
    [HideInInspector]
    public int X;
    [HideInInspector]
    public int Y;

    [HideInInspector]
    public Vector2 pos;
    

    public Animator animator;
    public AudioManager sound;
    public Map map;

    public SpriteRenderer rend;
    public GameObject nodeSpriteParent;
    [Header("Structure")]
    [SerializeField]
    private Structure structure;
    public Structure Structure { get { return structure; } }
    [Header("Technical")]
    [SerializeField]
    private Player owner;
    public Player GetOwner() { return owner; }
    [SerializeField]
    private bool IsSmallNode;

    public bool GetIsSmallNode() { return IsSmallNode; }
    [SerializeField]
    private float smallNodeScale = 0.55f;

    public string nodeName;
    public string nodeDescription;

    public float ownerSetDelay = 0.001f;
    [Header("Roads")]
    public GameObject roadPrefab;
    public GameObject roadParent;
    [SerializeField]
    public Road[] allRoads;
    public Road northRoad;
    public Road eastRoad;
    public Road southRoad;
    public Road westRoad;
    public Vector4 roads; /*roads(N, E, S, W)*/

    [Header("Garrison")]
    [SerializeField]
    private List<Commander> garrisonedCommanders;
    public List<Commander> GarrisonedCommanders { get { return garrisonedCommanders; } set { } }
    public List<Unit> Garrison {  get { return lordProtector.Units; }  set {  } }

    [SerializeField]
    private Commander lordProtector;
    public Commander LordProtector { get { return lordProtector; } set { } }

    public delegate void NodeSelectionDelegate(bool selected);
    public event NodeSelectionDelegate onNodeSelection;

    

    void Awake()
    {
        garrisonedCommanders = new List<Commander>();
        map = Map.Instance;
    }

    void Start()
    {
        ClearOwner();
        animator = GetComponentInChildren<Animator>();
        sound = FindObjectOfType<AudioManager>();
        if(rend == null)rend = GetComponentInChildren<SpriteRenderer>();
        ShowRoads();
        SetUpLordProtector();
    }

    public void OnSelection()
    {
        animator.SetTrigger("Selection");
        sound.PlaySelectionSound();
        SuggestRoadBuild();
        onNodeSelection?.Invoke(true);
    }

    public void OnDeSelection()
    {
        HideBuildSuggestion();
        onNodeSelection?.Invoke(false);
    }

    public void OnTurnStart()
    {
        SetCommanderVisibility(true);
    }

    public void OnTurnEnd()
    {
        SetCommanderVisibility(false);
    }

    public void OnInteraction(GameObject toInteract)
    {

    }

    public void SetOwner(Player player)
    {
        owner = player;
        rend.color = player.playerColor;
        AddSelfToOwner();
    }

    public void SetColor(Color color)
    {
        rend.color = color;
    }

    private void ClearOwner()
    {
        owner = null;
    }

    public void SetAsSmall()
    {
        IsSmallNode = true;
        Vector3 scale = nodeSpriteParent.transform.localScale;
        nodeSpriteParent.transform.localScale = scale * smallNodeScale;
    }

    public void SetData(NodeData data)
    {
        X = data.x;
        Y = data.y;
        transform.localPosition = new Vector3(data.pos.x, data.pos.y, transform.position.z);
        IsSmallNode = data.IsSmallNode;

        if(IsSmallNode)
        {
            roads = data.roads;
            for (int i = 0; i < 4; i++)
            {
                if(!string.IsNullOrEmpty(data.roadOwners[i]))
                {
                    Player player = TurnManager.Instance.GetPlayerByName(data.roadOwners[i]);
                    Road tempRoad = allRoads[i];
                    player.AddBuyableNoRefresh(tempRoad.buyableInfo.buyInfo);
                    tempRoad.SetParent(this);
                    tempRoad.SetOwner(player);
                }
            }
        }

        //initialStructName = data.structure;
        structure = new Structure(data.structure);

        if (data.owner.playerName != "")
        SetPlayersInitial(data.owner.playerName);
    }

    private void PostPlayerAddedInit()
    {
        AddStructure(structure.StructureName);
    }

    private void AddStructure(string structName)
    {
        if (!string.IsNullOrEmpty(structName))
        {
            Builder builder = GetComponent<Builder>();
            if (builder != null)
            {
                structure = null; // To not interfere with SetStructure
                builder.BuildByName(structName);
            }
        }
    }

    public void SetPlayersInitial(string playerName)
    {
        Map map = FindObjectOfType<Map>();

        foreach(Player player in map.players)
        {
            if (player.playerName == playerName)
            {
                StartCoroutine(SetOwnerWithDelay(player));
            }
        }
    }

    IEnumerator SetOwnerWithDelay(Player player)
    {
        //WTF IDK WHY I NEED TO DO THIS
        yield return new WaitForSeconds(ownerSetDelay);
        SetOwner(player);
        AddSelfToOwner();
        GetComponent<DisplayGenerals>().SetUpListeners();
        PostPlayerAddedInit();
        yield return null;
    }

    public float GetRevenue()
    {
        float revenue = 0;
        if (structure != null)
        {
            if (structure.buyableInfo == null) throw new System.Exception(nodeName + ": structure.BuyableInfo is Null");
            revenue = structure.buyableInfo.buyInfo.revenue;
        }
        return revenue;
    }

    public float GetExpenses()
    {
        float revenue = 0;
        if (structure != null)
        {
            if (structure.buyableInfo == null) throw new System.Exception(nodeName + ": structure.BuyableInfo is Null");
            revenue = structure.buyableInfo.buyInfo.maintenance;
        }
        return revenue;
    }

    public void AddSelfToOwner()
    {
        List<Node> ownersNodes = owner.GetNodesOwned();
        if (ownersNodes == null) ownersNodes = new List<Node>();

        if(ownersNodes.Contains(this))
        {
            return;
        }

        ownersNodes.Add(this);
    }

    public void SetRoads()
    {
        if (!IsSmallNode) { return; }
        GetAdjacents();
        ShowRoads();
        SetRoadArray();
    }

    public void ShowRoads()
    {
        if (roads.x == 1 && northRoad != null) northRoad.DisplayRoad();
        if (roads.y == 1 && eastRoad != null) eastRoad.DisplayRoad();
        if (roads.z == 1 && southRoad != null) southRoad.DisplayRoad();
        if (roads.w == 1 && westRoad != null) westRoad.DisplayRoad();
    }

    public void HideBuildSuggestion()
    {
        HideRoads();
    }

    public void HideRoads()
    {
        if(!IsSmallNode)
        {
            List<Node> toSuggest = AdjacentNodes();

            for (int i = 0; i < 4; i++)
            {
                Node suggest = toSuggest[i];
                if (suggest == null) continue;
                suggest.GetDirection(this).HideBuildableRoad();
            }
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                Road road = allRoads[i];
                if (road == null) continue;
                road.HideBuildableRoad();
            }
        }
    }

    public void GetAdjacents()
    {
        
        Node[,] mapNodes = map.GetNodes();
        GameObject obj;
        

        if (Y + 1 < map.sizeY)
        {
            obj = Instantiate(roadPrefab, roadParent.transform);
            northRoad = obj.GetComponent<Road>();
            northRoad.SetTarget(mapNodes[X, Y + 1]);
            northRoad.SetParent(this);
            northRoad.name = "North Road from " + northRoad.parentNode.gameObject.name + " to " + mapNodes[X, Y + 1].gameObject.name;
        }
        if (X + 1 < map.sizeX)
        {
            obj = Instantiate(roadPrefab, roadParent.transform);
            eastRoad = obj.GetComponent<Road>();
            eastRoad.SetTarget(mapNodes[X + 1, Y]);
            eastRoad.SetParent(this);
            eastRoad.name = "East Road from " + eastRoad.parentNode.gameObject.name + " to " + mapNodes[X + 1, Y].gameObject.name;
        }
        if (Y - 1 >= 0)
        {
            obj = Instantiate(roadPrefab, roadParent.transform);
            southRoad = obj.GetComponent<Road>();
            southRoad.SetTarget(mapNodes[X, Y - 1]);
            southRoad.SetParent(this);
            southRoad.name = "South Road from " + southRoad.parentNode.gameObject.name + " to " + mapNodes[X, Y - 1].gameObject.name;
        }
        if (X - 1 >= 0) 
        {
            obj = Instantiate(roadPrefab, roadParent.transform);
            westRoad = obj.GetComponent<Road>();
            westRoad.SetTarget(mapNodes[X - 1, Y]);
            westRoad.SetParent(this);
            westRoad.name = "West Road from " + westRoad.parentNode.gameObject.name + " to " + mapNodes[X - 1, Y].gameObject.name;
        }
    }

    public void SuggestRoadBuild()
    {
        List<Node> toSuggest = AdjacentNodes();
        

        if(IsSmallNode)
        {
            for(int i = 0; i < 4; i++)
            {
                TryDisplayBuildableRoad(allRoads[i], toSuggest[i]);
            }
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                Node suggest = toSuggest[i];
                if (suggest != null) { suggest.TryDisplayBuildableRoad(suggest.GetDirection(this), this); }
            }
        }
    }

    public void TryDisplayBuildableRoad(Road road, Node node)
    {
        if (node == null || road == null) return;
        if (HasRoadTo(node)) return;
        else road.DisplayBuildableRoad(node);
    }

    public Road GetDirection(Node node)
    {
        if (!IsSmallNode) return null;

        if (node.X > X) return eastRoad;
        else if(node.X < X) return westRoad;
        if (node.Y > Y) return northRoad;
        else return southRoad;
    }

    public List<Node> AdjacentNodes()
    {
        List<Node> toReturn = new List<Node>();

        Node[,] mapNodes = map.GetNodes();

        //North
        if (Y + 1 < map.sizeY)
        {
            toReturn.Add(mapNodes[X, Y + 1]);
        }
        else toReturn.Add(null);
        //East
        if (X + 1 < map.sizeX)
        {
            toReturn.Add(mapNodes[X + 1, Y]);
        }
        else toReturn.Add(null);
        //South
        if (Y - 1 >= 0)
        {
            toReturn.Add(mapNodes[X, Y - 1]);
        }
        else toReturn.Add(null);
        //West
        if (X - 1 >= 0)
        {
            toReturn.Add(mapNodes[X - 1, Y]);
        }
        else toReturn.Add(null);

        return toReturn;
    }

    public bool HasRoadTo(Node node)
    {
        Road road = GetDirection(node);
        if (road.IsBuildable(node))
        {
            int isBuilt = RoadToRoadsInfo(GetDirection(node));
            //Debug.Log(road + " is connected to " + node + " = " + isBuilt);
            if (isBuilt != 1) return false;
            else return true;
        }
        else return false;
    }

    public int RoadToIndex(Road road)
    {
        if (road == northRoad) return 0;
        if (road == eastRoad) return 1;
        if (road == southRoad) return 2;
        if (road == westRoad) return 3;
        else return -1;
    }

    public int RoadToRoadsInfo(Road road)
    {
        if (road == northRoad) return (int)roads.x;
        if (road == eastRoad) return (int)roads.y;
        if (road == southRoad) return (int)roads.z;
        if (road == westRoad) return (int)roads.w;
        else return -1;
    }

    public void SetRoadArray()
    {
        allRoads = new Road[] { northRoad, eastRoad, southRoad, westRoad };
    }
    
    public void OnBoughtRoad(Road bought)
    {
        roads[RoadToIndex(bought)] = 1;
        ShowRoads();
    }

    public void OnDestroyRoad(Road destoyed)
    {
        roads[RoadToIndex(destoyed)] = 0;
        ShowRoads();
    }
    
    public void OnOpenGarrison()
    {
        //GarrisonMenu.Instance.
    }

    private void SetUpLordProtector()
    {
        lordProtector = gameObject.AddComponent<Commander>();
        lordProtector.commaderName = "Lord Protector";
        Sprite temp = CommanderDataObject.Instance.faces[Random.Range(0, CommanderDataObject.Instance.faces.Count)];
        lordProtector.sprite = temp;
        lordProtector.rank = Commander.Rank.general;
    }

    private void SetCommanderVisibility(bool visible)
    {
        if(GetComponent<DisplayGenerals>())
        {
            GetComponent<DisplayGenerals>().SetVisible(visible);
        }
    }

    public void SetStructure(Structure struc)
    {
        if(structure == null && struc != null)
        structure = struc;
    }

    public void RemoveStructure()
    {
        structure = null;
    }
}

[System.Serializable]
public class NodeData
{
    public NodeData() { }
    public bool IsSmallNode;
    public int x;
    public int y;
    public Vector2 pos;
    public Vector4 roads;
    public string[] roadOwners;
    public string structure;
    
    public PlayerData owner;

    public void SetData(Node node)
    {
        x = node.X;
        y = node.Y;

        if (node.Structure != null)
            structure = node.Structure.StructureName;

        pos = node.pos;
        IsSmallNode = node.GetIsSmallNode();
        if(IsSmallNode)
        {
            roads = node.roads;
            roadOwners = new string[4];

            Road[] Roads = node.allRoads;
            for (int i = 0; i < 4; i++)
            {
                //NORTH, EAST, SOUTH, WEST
                if(Roads[i] != null && Roads[i].owner != null)
                roadOwners[i] = Roads[i].owner.playerName;
            }
        }
        
        PlayerData player = new PlayerData();
        if (node.GetOwner() != null) { player.SetData(node.GetOwner()); }
        
        owner = player;
    }
}