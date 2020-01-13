using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Node))]
public class DisplayGenerals : MonoBehaviour
{
    [SerializeField]
    private Node targetNode;
    [SerializeField]
    private CommanderDisplay DisplayPrefab;
    [SerializeField]
    private HorizontalObjectLayout DefaultHolder;
    [SerializeField]
    private HorizontalObjectLayout SelectedHolder;
    [SerializeField]
    private Vector2 offset;
    private List<Commander> commanders;

    [SerializeField]
    private bool isVisible;
    [SerializeField]
    private bool isNodeSelected;


    public void Start()
    {
        targetNode = GetComponent<Node>();
        targetNode.onNodeSelection += SetDisplay;
    }

    public void SetUpListeners()
    {
        targetNode.GetOwner().onCommanderAdded += OnCommanderAdded;
    }

    private void SetDisplay(bool nodeIsSelected)
    {
        isNodeSelected = nodeIsSelected;
        if (!isVisible) return;
        GetCommanders();
        ClearDisplays();
        Transform display = GetHolder(nodeIsSelected);

        foreach (Commander commander in commanders)
        {
            GameObject obj = Instantiate(DisplayPrefab.gameObject, display);
            SetCommanderDisplay(commander, obj, nodeIsSelected);
        }

        display.gameObject.GetComponent<HorizontalObjectLayout>().Refresh();
    }

    public void SetVisible(bool visible)
    {
        isVisible = visible;
        if (isVisible) SetDisplay(false);
        else ClearDisplays();
    }

    private void ClearDisplays()
    {
        Utility.ClearChildren(DefaultHolder.gameObject);
        Utility.ClearChildren(SelectedHolder.gameObject);
    }

    private Transform GetHolder(bool isSelected)
    {
        if (isSelected) return SelectedHolder.transform; else return DefaultHolder.transform;
    }

    private void SetCommanderDisplay(Commander commander, GameObject obj, bool isSelected)
    {
        CommanderDisplay disp = obj.GetComponent<CommanderDisplay>();
        disp.ProfileRend.sprite = commander.sprite;
        disp.SetSpriteRendOrder(isSelected);
    }

    private void GetCommanders()
    {
        //The Last commander must be the lord protector;
        commanders = new List<Commander>(targetNode.GarrisonedCommanders);
        commanders.Add(targetNode.LordProtector);
    }

    private void OnCommanderAdded(Commander comm)
    { 
        GetCommanders();
        if (commanders.Contains(comm)) SetDisplay(isNodeSelected);
    }
}
