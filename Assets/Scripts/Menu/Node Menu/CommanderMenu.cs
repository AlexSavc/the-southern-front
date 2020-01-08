using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommanderMenu : MonoBehaviour
{
    public GameObject commanderButtonObj;
    public GameObject garrisonSlot;
    public GameObject addButton;
    public GameObject backButton;
    public GameObject content;

    public TurnManager turnManager;
    public List<Commander> commanders;

    public Player currentPlayer;

    public GameObject GarissonMenu;
    
    void Awake()
    {
        
    }

    public void Start()
    {
        turnManager = TurnManager.Instance;
    }
    
    public void RefreshDisplay()
    {
        ClearCommanderMenu();
        Setup();
        backButton.transform.SetAsFirstSibling();
        addButton.transform.SetAsLastSibling();
    }
    
    public void Setup()
    {
        if (turnManager == null) turnManager = TurnManager.Instance;
        SetCurrentPlayer();
        SetCommanders();
        AddCommanderButtons();
    }

    public void OnCommanderPressed(Commander commander)
    {
        if (GarissonMenu.activeInHierarchy == true)
        {
            CloseGarrisonMenu();
        }
        else OpenGarrisonMenu();
    }

    public void OpenGarrisonMenu()
    {
        GarissonMenu.SetActive(true);
    }
    public void CloseGarrisonMenu()
    {
        GarissonMenu.SetActive(false);
    }

    public void ClearCommanderMenu()
    {
        foreach(GameObject obj in Utility.GetChildren(content.transform))
        {
            if (obj != addButton )
                if (obj != backButton)
                    Destroy(obj);
        }
    }

    public void OnAddCommander()
    {
        
    }


    public void SetCommanders()
    {
        if (currentPlayer == null) return;
        commanders = new List<Commander>(currentPlayer.Commanders);

        if(IsNodeSelected() == true)
        {
            Commander nodeLord = Interaction.Instance.selected.GetComponent<Node>().LordProtector;
            commanders.Insert(0, nodeLord);
        }
    }

    public void SetCurrentPlayer()
    {
        currentPlayer = turnManager.currentPlayer;
    }

    public void Refresh()
    {
        RefreshDisplay();
    }


    public void AddCommanderButtons()
    {
        if (commanders.Count < 1) return;
        foreach (Commander commander in commanders)
        {
            GameObject button = Instantiate(commanderButtonObj, content.transform);
            CommanderButton comButton = button.GetComponent<CommanderButton>();
            comButton.SetCommander(commander);
        }
    }

    private bool IsNodeSelected()
    {
        GameObject obj = Interaction.Instance.selected;
        if (obj != null)
        {
            if(obj.GetComponent<Node>())
            {
                return true;
            }
        }
        return false;
    }

    public void OnDisable()
    {
        ClearCommanderMenu();
        CloseGarrisonMenu();
    }
}
