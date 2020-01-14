using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject defaultMenu;
    [SerializeField]
    private GameObject nodeMainMenu;
    [SerializeField]
    private GameObject nodeInfoMenu;
    [SerializeField]
    private GameObject economyMenu;
    [SerializeField]
    private GameObject upperMenu;
    [SerializeField]
    private GameObject nextTurnMenu;
    [SerializeField]
    private GameObject CommandersMenu;

    [SerializeField]
    private GameObject garrisonMenu;


    List<GameObject> allMenus;

    [SerializeField]
    private Node currentNode;

    [SerializeField]
    private Interaction interaction;

    private NodeDesriptionUtility nodeUtil;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Deselect();
        }
    }

    void Start()
    {

        allMenus = new List<GameObject>() { nodeMainMenu, defaultMenu, nodeInfoMenu, economyMenu, upperMenu, nextTurnMenu, CommandersMenu /*nodeGarrissonMenu, nodeRecruitMenu, nodeUpgradeMenu*/};
        DeactivateAll();
        ActivateDefaultMenus();
        interaction = FindObjectOfType<Interaction>();
        interaction.selectionEvent += OnSelection;
        nodeUtil = gameObject.AddComponent<NodeDesriptionUtility>();

        garrisonMenu.SetActive(true);
        garrisonMenu.GetComponent<GarrisonMenu>().SetSingleton();
        garrisonMenu.SetActive(false);
    }

    public void Deselect()
    {
        nodeMainMenu.GetComponent<OpenNodeMenu>().GoBack();
        DeactivateAll();
        ActivateDefaultMenus();
    }

    public void OnSelection(GameObject selectable)
    {
        if (selectable == null) {Deselect(); return; }
        Node n = selectable.GetComponent<Node>();
        Open(n);
    }

    public void Open(Node node)
    {
        DeactivateAll();
        nodeMainMenu.SetActive(true);
        nodeMainMenu.GetComponent<OpenNodeMenu>().SetNode(node);
        upperMenu.SetActive(true);

        if(node != null)
        {
            nodeInfoMenu.SetActive(true);
            NodeDescription description = nodeUtil.GetDescription(node);
            nodeInfoMenu.GetComponent<InfoMenu>().SetDisplay(description);
        }
        
    }

    public void Close()
    {
        DeactivateAll();
        ActivateDefaultMenus();
    }

    public void OpenEconomyMenu()
    {
        if(economyMenu.activeInHierarchy)
        {
            DeactivateAll();
            ActivateDefaultMenus();
        }
        else
        {
            DeactivateAll();
            ActivateDefaultMenus();
            economyMenu.SetActive(true);
        }
    }

    void ActivateDefaultMenus()
    {
        defaultMenu.SetActive(true);
        upperMenu.SetActive(true);
    }

    public void OpenNextPlayerWindow()
    {
        DeactivateAll();
        nextTurnMenu.SetActive(true);
    }

    public void OpenCommanderMenu()
    {
        DeactivateAll();
        upperMenu.SetActive(true);
        CommandersMenu.SetActive(true);
        CommandersMenu.GetComponent<CommanderMenu>().Refresh();
    }

    public void CloseNextPlayerWindw()
    {
        Deselect();
    }

    public void DeactivateAll()
    {
        Utility.SetActiveAll(allMenus, false);
    }
}
