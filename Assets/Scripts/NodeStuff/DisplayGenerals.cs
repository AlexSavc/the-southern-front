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

    public void Start()
    {
        targetNode = GetComponent<Node>();
    }

    private void SetDefaultDisplay()
    {
        Utility.ClearChildren(DefaultHolder);
        foreach(Commander commander in commanders)
        {
            GameObject obj = Instantiate(DisplayPrefab.gameObject, DefaultHolder.transform);
            obj.GetComponent<CommanderDisplay>().Rend.sprite = commander.sprite;
        }
    }

    private void SetSelectedDisplay()
    {
        
    }

    private void GetCommanders()
    {
        //The Last commander must be the lord protector;
        commanders = targetNode.GarrisonedCommanders;
        commanders.Add(targetNode.LordProtector);
    }
}
