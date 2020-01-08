using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class CreationMenu : MonoBehaviour
{
    public MainMenu mainMenu;

    public int mapSize = 3;


    [SerializeField]
    private int maxPlayerCount = 2;
    [SerializeField]
    public Sprite[] playerColors;
    //private Color[] playerColors;
    private MapSlot myMapSlot;

    public MapCreationUtility mapUtility;
    
    [Header("Map Name")]
    public TMP_InputField mapNameInput;
    [Space]
    [Header("Player Count")]
    [SerializeField]
    private SmartButtonUI addPlayerButton;
    [SerializeField]
    private GameObject playerSlotPrefab;

    /*Call this EVent befor setting the playerSlot's data, 
     * as it first has to call PlayerSlot to update the available data.*/
    public delegate void AddPlayerEvent();
    public event AddPlayerEvent onAddPlayer;

    public Dropdown mapSizeDropDown;

    public Sprite human;
    public Sprite easy;
    public Sprite medium;
    public Sprite hard;

    //public Sprite[] getColors() { return playerColors; }

    [SerializeField]
    private PlayerSlot[] playerSlots;
    [SerializeField]
    private GameObject playerListHolder;

    public PopupHandler popupHandler;
    public Dictionary<string, object> popups;

    public BrownianMotionData brownianMotionData;

    void Awake()
    {
        popups = new Dictionary<string, object>()
        {
            {
                "PlayerNameEmpty",
                new InfoPopupInfo()
                {
                    infoText = "Player name Cannot be empty",
                    okText = "Ok, boomer"
                }
            },

            {
                "MapNameEmpty",
                new InfoPopupInfo()
                {
                    infoText = "Please enter a map name",
                    okText = "Alright"
                }
            },

            {
                "PlayerNamesIdentical",
                new InfoPopupInfo()
                {
                    infoText = "Two players can't have the same name",
                    okText = "Damn it"
                }
            },

            {
                "MapNamesIdentical",
                new InfoPopupInfo()
                {
                    infoText = "This map name already exists, please choose a new one",
                    okText = "Fine"
                }
            },

            {
                "ZeroPlayers",
                new InfoPopupInfo()
                {
                    infoText = "There must be at least one player. Add more",
                    okText = "Ok"
                }
            }
        };
    }

    void Start()
    {
        if (mapUtility == null) mapUtility = FindObjectOfType<MapCreationUtility>();
        if (popupHandler == null) popupHandler = FindObjectOfType<PopupHandler>();
        mapNameInput.text = "New Map";
    }

    public void StartCreation(MapSlot mapSlot)
    {
        myMapSlot = mapSlot;
    }

    public void OnCancel()
    {
        mainMenu.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnCreate()
    {
        PlayerData[] myPlayers = GetPlayers();

        MapCreateInfo info = new MapCreateInfo
        {
            sizeX = mapSize,
            sizeY = mapSize,
            mapName = mapNameInput.text,
            players = myPlayers,
            brownianMotionData = brownianMotionData
        };
        

        if (CheckMapCreateInfo(info) == false) return; 

        if(GetPlayerCount() < 1) { GetPopup("ZeroPlayers"); return; }

        mapUtility.Create(info);
        
        mainMenu.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void GetPlayerSlots()
    {
        Transform holder = playerListHolder.transform;
        int count = holder.childCount;
        List<PlayerSlot> siblings = new List<PlayerSlot>();
        for (int i = 0; i < count; i++)
        {
            GameObject sibling = holder.GetChild(i).gameObject;
            PlayerSlot slot = sibling.GetComponent<PlayerSlot>();
            if (slot != null) siblings.Add(slot);
        }

        playerSlots = siblings.ToArray();
    }

    public int GetPlayerCount()
    {
        return playerListHolder.transform.childCount - 1;
    }

    public void OnAddPlayer()
    {
        AddPlayerSlot();
        RefreshAddButton();
        onAddPlayer?.Invoke();
        UpdatePlayerList();
    }

    public void AddPlayerSlot()
    {
        GameObject slot = Instantiate(playerSlotPrefab, playerListHolder.transform);
        PlayerSlot playerSlot = slot.GetComponent<PlayerSlot>();

        playerSlot.creationMenu = this;
        playerSlot.InitializeDropdowns();

        int num = GetPlayerCount() - 1;
          
        playerSlot.playerName.text = "Player " + GetPlayerCount();
        playerSlot.playerTeam.value = num;
        playerSlot.playerColor.value = num;
    }

    void UpdatePlayerList()
    {
        addPlayerButton.ResetChildPosition();
        
        /*to refresh the content size fitter*/
        StartCoroutine(updateList());
    }

    public void RefreshAddButton()
    {
        string dropDown = mapSizeDropDown.captionText.text;

        if (int.TryParse(dropDown, out maxPlayerCount))
        {
            //MaxPlayerCount is pasize-1 if mapSize & 2 == 0
            mapSize = maxPlayerCount;
            //Mapsize-1 makes it so that players can be spread evenly with one space in between them
            maxPlayerCount--;
        }
        addPlayerButton.Activate();
        if (GetPlayerCount() >= maxPlayerCount) { addPlayerButton.Deactivate(); return; }
        
    }

    IEnumerator updateList()
    {
        /*PLayerlistHolder size fitter would only update once every two times you added a map. 
         * This refreshes it and solves the problem*/
        playerListHolder.SetActive(false);
        yield return new WaitForSeconds(0.005f);
        playerListHolder.SetActive(true);
        yield return null;
    }

    public Color GetColorFromSprite(Sprite sprite)
    {
        Texture2D texture = sprite.texture;
        Color color = texture.GetPixel(20, 20);
        return color;
    }

    public PlayerData[] GetPlayers()
    {
        List<PlayerData> play = new List<PlayerData>();
        Transform holder = playerListHolder.transform;
        for (int i = 0; i < holder.childCount; i++)
        {
            GameObject obj = holder.GetChild(i).gameObject;
            PlayerSlot slot = obj.GetComponent<PlayerSlot>();
            if (slot != null)
            {
                PlayerData player = new PlayerData();

                player.playerName = slot.playerName.text;
                player.playerColor = GetColorFromSprite(slot.playerColor.captionImage.sprite);
                player.type = slot.playerType.captionText.text;
                player.team = slot.playerTeam.itemText.text;
                
                play.Add(player);
            }
        }
        
        return play.ToArray();
    }
    
    public void RemovePlayerSlot(PlayerSlot toRemove)
    {
        List<PlayerSlot> objs = new List<PlayerSlot>();
        Transform p = playerListHolder.transform;
        for (int i = 0; i < p.childCount; i++)
        {
            if (p.GetChild(i).GetComponent<PlayerSlot>()) objs.Add(p.GetChild(i).GetComponent<PlayerSlot>());
        }

        foreach(PlayerSlot slot in objs)
        {
            if(slot == toRemove)
            {
                Destroy(slot.gameObject);
            }
        }
        RefreshAddButton();
    }

    public bool CheckMapCreateInfo(MapCreateInfo info)
    {
        if (CheckPlayerNames(GetNamesFromInfo(info)) == false) { return false; }
        if (CheckMapNames(GetMapNames(), info.mapName) == false) { return false; }
        else return true;
    }

    public string[] GetNamesFromInfo(MapCreateInfo info)
    {
        string[] names = new string[info.players.Length];

        PlayerData[] playerData = info.players;

        for (int i = 0; i < playerData.Length; i++)
        {
            names[i] = playerData[i].playerName;
        }

        return names;
    }

    public bool CheckPlayerNames(string[] names)
    {
        List<string> strings = new List<string>();

        foreach(string str in names)
        {
            if(str == "") { Debug.Log("PlayerName empty"); GetPopup("PlayerNameEmpty");  return false; }
            if (!strings.Contains(str)) strings.Add(str);
            else { Debug.Log("PlayerName the same"); GetPopup("PlayerNamesIdentical");   return false; }
        }
        
        return true;
    }

    public bool CheckMapNames(string[] names, string mapName)
    {
        if (names.Length == 0) return true;

        if(mapName.Length < 1) { GetPopup("MapNameEmpty"); return false; }

        foreach (string str in names)
        {
            if (str == mapName)  { Debug.Log("MapName the same"); GetPopup("MapNamesIdentical"); return false; }
        }

        return true;
    }
    

    public string[] GetMapNames()
    {
        string[] mapPaths = Directory.GetFiles(mapUtility.GetSinglePlayerPath(), "*.json");

        for(int i = 0; i < mapPaths.Length; i++)
        {
            mapPaths[i] = Path.GetFileNameWithoutExtension(mapPaths[i]);
        }

        return mapPaths;
    }

    public void GetPopup(string key)
    {
        popupHandler.AddToQueue(popups[key]);
    }

    void SetUpPopupDictionnary()
    {

    }
}
