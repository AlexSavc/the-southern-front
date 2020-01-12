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
    private GameObject DefaultHolder;
    [SerializeField]
    private GameObject SelectedHolder;
    [SerializeField]
    private Vector2 offset;
    private List<Commander> commanders;

    [SerializeField]
    private bool isVisible;
    

    public void Start()
    {
        targetNode = GetComponent<Node>();
        targetNode.onNodeSelection += SetDisplay;
    }

    private void SetDisplay(bool nodeIsSelected)
    {
        if (!isVisible) return;
        GetCommanders();
        ClearDisplays();
        Transform display = GetHolder(nodeIsSelected);

        foreach (Commander commander in commanders)
        {
            GameObject obj = Instantiate(DisplayPrefab.gameObject, display);
            SetCommanderDisplay(commander, obj, nodeIsSelected);
        }
    }

    public void SetVisible(bool visible)
    {
        isVisible = visible;
        if (isVisible) SetDisplay(false);
        else ClearDisplays();
    }

    private void ClearDisplays()
    {
        Utility.ClearChildren(DefaultHolder);
        Utility.ClearChildren(SelectedHolder);
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
        commanders = new List<Commander>();
        commanders.AddRange(targetNode.GarrisonedCommanders);
        commanders.Add(targetNode.LordProtector);
    }
}
