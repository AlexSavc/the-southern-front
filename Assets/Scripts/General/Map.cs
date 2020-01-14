using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class Map : MonoBehaviour
{
    public Color uncoccupiedColor;
    public int sizeX;
    public int sizeY;
    public int zPos = 0;

    public float yOffset = 1;

    public GameObject nodeParent;
    public GameObject nodeTemplate;

    [SerializeField]
    private Node[,] nodes;
    public Node[,] GetNodes() { return nodes; }

    public Player[] players;
    public GameObject playerParent;
    public TurnManager turnManager;

    public string jsonSave;
    public string savePath;

    public MapCreateInfo info;
    public MapSaveInfo saveInfo;

    public delegate void MapGenerateDelegate();
    public event MapGenerateDelegate OnGenerateMap;

    public delegate void PlayersGenerateDelegate();
    public event PlayersGenerateDelegate OnGeneratePlayers;

    //OnNodesAddedToOwners triggers TurnManager.FirstTurn. Do NOT call it ANYWHERE except once in Map.
    public delegate void NodesAddedToOwner();
    public event NodesAddedToOwner OnNodesAddedToOwner;

    public float playerLayoutSpeed = 0.1f;

    public void Start()
    {
        if (uncoccupiedColor == null) uncoccupiedColor = new Color(183, 149, 118);
        nodeParent = new GameObject { name = "nodeParent" };
        nodeParent.transform.parent = transform;

        if(turnManager == null) turnManager = FindObjectOfType<TurnManager>();
        if (playerParent == null) playerParent = turnManager.gameObject;
        OnGeneratePlayers += turnManager.OnPlayersGenerated;
        OnNodesAddedToOwner += turnManager.OnNodesAddedToOwners;
    }

    public void GenerateMap(string path)
    {
        savePath = path;
        info = JsonUtility.FromJson<MapCreateInfo>(System.IO.File.ReadAllText(path));

        sizeX = info.sizeX;
        sizeY = info.sizeY;

        nodes = new Node[sizeX, sizeY];
        players = GeneratePlayers(info.players);
        GenerateNodes(null);
        LayoutNewBases(info);
        StartCoroutine(OnNodesAddedDelay());

        //Save Map in TurnManager
    }

    public void GenerateNodes(MapSaveInfo info)
    {
        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                Vector3 pos = LayoutNodes(x, y);
                if (pos.x == -1) continue;

                GameObject nodeObj = Instantiate(nodeTemplate, nodeParent.transform);

                nodeObj.name = "node-" + x + "_" + y;
                
                //nodeObj.transform.localPosition = new Vector3(x, y, zPos);

                Node node = nodeObj.GetComponent<Node>();
                
                if (pos.z == 111) node.SetAsSmall();
                nodes[x, y] = node;
                node.X = x;
                node.Y = y;

                //EXPERIMENTAL>

                //float brownian = Utility.BrownianMotion(x, y, info.brownianMotionData);

                //<EXPERIMENTAL
                node.roads = new Vector4(0, 0, 0, 0);

                float factor = 0.2f;
                Vector3 neuPos;
                if(info == null)
                {
                    neuPos = new Vector3(Random.Range(x - factor, x + factor), Random.Range(y - factor, y + factor), transform.localPosition.z);
                    node.pos = new Vector2(neuPos.x, neuPos.y);
                }
                else
                {
                    int index = Utility.CoordsToArrayPos(x, y, info.sizeY);
                    neuPos = new Vector3(info.nodes[index].pos.x, info.nodes[index].pos.y, 0);
                    node.pos = info.nodes[index].pos;
                }

                node.SetColor(uncoccupiedColor);

                node.transform.localPosition = neuPos;
            }
        }

        OnGenerateMap?.Invoke();
    }


    public Vector3 LayoutNodes(int x, int y)
    {
        Vector3 coords = new Vector3();

        float X = 0;
        float Y = y*yOffset;


        //coords.z == 111 tells GenerateMap that it's a small node
        if (y % 2 == 0)
        {
            if (x % 2 == 0)
            {
                coords.z = 111;
            }
        }
        else if (x % 2 != 0) coords.z = 111;

        for(int i =0; i < x; i++)
        {
            X += 2;
        }

        coords.x = X;
        coords.y = Y;

        return coords;
    }

    public void SetNodePosition()
    {

    }

    public void LayoutNewBases(MapCreateInfo info)
    {
        List<Node> nodeList = GetEquidistantSpread(info.players.Length);
        StartCoroutine(LayoutCircle(nodeList));
    }

    public IEnumerator LayoutCircle(List<Node> nodeList)
    {
        yield return new WaitForSeconds(0.05f);
        for (int i = 0; i < players.Length; i++)
        {
            nodeList[i].SetOwner(players[i]);
            yield return new WaitForSeconds(playerLayoutSpeed);
        }
        yield return null;
    }

    public Player[] GeneratePlayers(PlayerData[] playerdata)
    {
        List<Player> list = new List<Player>();
        foreach(PlayerData data in playerdata)
        {
            GameObject playerObj = new GameObject("Player: "+data.playerName);
            playerObj.transform.parent = playerParent.transform;
            playerObj.AddComponent(typeof(Player));
            Player newPlayer = playerObj.GetComponent<Player>();
            newPlayer.SetData(data);

            if(saveInfo.currentTurn != null  )
            {
                if(data.playerName == saveInfo.currentTurn.playerName)
                {
                    turnManager.currentPlayer = newPlayer;
                }
            }

            list.Add(newPlayer);
        }

        OnGeneratePlayers?.Invoke();

        return list.ToArray();
    }

    public PlayerData[] GeneratePlayerData(Player[] myPlayers)
    {
        List<PlayerData> list = new List<PlayerData>();
        foreach (Player player in myPlayers)
        {
            PlayerData playerData = new PlayerData(player);
            //playerData.SetData(player);
            list.Add(playerData);
        }

        return list.ToArray();
    }

    public PlayerData[] GeneratePlayerData(Player[] myPlayers, out PlayerData currentTurn)
    {
        List<PlayerData> list = new List<PlayerData>();
        currentTurn = null;
        foreach (Player player in myPlayers)
        {
            PlayerData playerData = new PlayerData(player);
            list.Add(playerData);
            if (player == turnManager.currentPlayer) currentTurn = playerData;
        }
        
        return list.ToArray();
    }

    public void LoadMap(string path)
    {
        savePath = path;
        saveInfo = JsonUtility.FromJson<MapSaveInfo>(System.IO.File.ReadAllText(path));

        sizeX = saveInfo.sizeX;
        sizeY = saveInfo.sizeY;

        nodes = new Node[sizeX, sizeY];
        players = GeneratePlayers(saveInfo.players);
        GenerateNodes(saveInfo);
        LayoutNodeSaveInfo(saveInfo);
    }

    public void SaveMap()
    {
        if (nodes == null) { return; }

        MapSaveInfo saveInfo = new MapSaveInfo();

        List<NodeData> saveNodes = new List<NodeData>();

        for(int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                if(nodes[x,y] != null)
                {
                    NodeData nodeData = new NodeData();
                    nodeData.SetData(nodes[x,y]);

                    saveNodes.Add(nodeData);
                }
                
            }
        }

        saveInfo.sizeX = sizeX;
        saveInfo.sizeY = sizeY;
        saveInfo.nodes = saveNodes.ToArray();

        saveInfo.players = GeneratePlayerData(players, out saveInfo.currentTurn);
        jsonSave = JsonUtility.ToJson(saveInfo);

        File.WriteAllText(savePath, jsonSave);
    }

    public void LayoutNodeSaveInfo(MapSaveInfo saveInfo)
    {
        /*/ WARNING \ if this gets called before GeneratePlayers(), Node.SetPlayersInitial breaks.*/

        for(int x = 0; x < saveInfo.sizeX; x++)
        {
            for (int y = 0; y < saveInfo.sizeY; y++)
            {
                //Because the original layout was sideways for(y){for(x){ Generation() }}
                nodes[x, y].SetData(saveInfo.nodes[x * sizeY + y]);
            }
        }

        StartCoroutine(OnNodesAddedDelay());
    }

    public IEnumerator OnNodesAddedDelay()
    {
        //Because they have an enumerator too
        yield return new WaitForSeconds(nodes[0, 0].ownerSetDelay+ 0.001f);
        OnNodesAddedToOwner?.Invoke();
    }

    public bool CheckCoords(int x, int y)
    {
        /*The X alternates, for example between lines of 3 and 4 nodes.
         LayoutNodes return -1,-1 if the node.x == 4 when it's a line of 3*/
        if (LayoutNodes(x, y).x == -1) return false;
        else return true;
    }

    public Node[] GetAllBigNodes()
    {
        List<Node> nodeList = new List<Node>();

        foreach(Node node in nodes)
        {
            if (node.GetIsSmallNode() == false) { nodeList.Add(node); }
            
        }

        return nodeList.ToArray();
    }

    public Node[] GetAllSideNodes(Node[] nodeArray)
    {
        List<Node> nodeList = new List<Node>();

        foreach(Node node in nodeArray)
        {
            if (node.X == sizeX-1 || node.X == 0 || node.Y == sizeY-1 || node.Y == 0)
            {
                nodeList.Add(node);
            }
        }

        return nodeList.ToArray();
    }

    public List<Node> GetPerimeterNodes(Node[] nodeArray)
    {
        //Node[] nodeArray = GetAllSideNodes(GetAllBigNodes());
        
        List<Node> returnList = new List<Node>();
        
        //starting with the first big node
        int firstX = 1;
        int firstY = 0;

        List<Node> orderedList = new List<Node>();

        orderedList.Add(nodes[firstX, firstY]);
        // go counter clockwise
        int XX = firstX;
        int YY = firstY;

        while(XX < sizeX - 1)
        {
            XX++;
            foreach(Node node in nodeArray)
            {
                if(node.X == XX && node.Y == YY)
                {
                    orderedList.Add(node);
                }
            }
        }
        while(YY < sizeY-1)
        {
            YY++;
            foreach (Node node in nodeArray)
            {
                if (node.X == XX && node.Y == YY)
                {
                    orderedList.Add(node);
                }
            }
        }
        while(XX > 0)
        {
            XX--;
            foreach (Node node in nodeArray)
            {
                if (node.X == XX && node.Y == YY)
                {
                    orderedList.Add(node);
                }
            }
        }
        while (YY > 0)
        {
            YY--;
            foreach (Node node in nodeArray)
            {
                if (node.X == XX && node.Y == YY)
                {
                    orderedList.Add(node);
                }
            }
        }
        while(XX < firstX - 1)
        {
            XX++;
            foreach (Node node in nodeArray)
            {
                if (node.X == XX && node.Y == YY)
                {
                    orderedList.Add(node);
                }
            }
        }
        return orderedList;
    }

    public List<Node> GetEquidistantSpread(int toSpread)
    {
        List<Node> returnList = new List<Node>();

        List<Node> nodeList = GetPerimeterNodes(GetAllSideNodes(GetAllBigNodes()));

        Vector2Int[] coords = new Vector2Int[toSpread];

        float distance = ((nodeList.Count - toSpread) / toSpread)+1;

        float i = 0;
        for(int d = 0; d < toSpread; d ++)
        {
            
            returnList.Add(nodeList[Mathf.RoundToInt(i)]);
            i += distance;
        }

        return returnList;
    }

    
}

[System.Serializable]
public class MapSaveInfo
{
    public PlayerData[] players;
    public PlayerData currentTurn;
    public NodeData[] nodes;

    public int sizeX;
    public int sizeY;

    public Vector2 pos;

    public string savePath;
}

[System.Serializable]
public class MapCreateInfo
{
    public int sizeX;
    public int sizeY;
    public string savePath;
    public string mapName;
    public PlayerData[] players;
    public BrownianMotionData brownianMotionData;
}
