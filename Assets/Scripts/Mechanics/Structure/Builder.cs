﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Node))]
public class Builder : MonoBehaviour
{
    [SerializeField]
    private List<Structure> buildable;
    public List<Structure> Buildable { get { return buildable; } }

    private StructureManager manager;
    private Node node;

    [SerializeField]
    private Sprite demolishSprite;

    Structure tryBuild;

    void Start()
    {
        manager = StructureManager.Instance;
        node = GetComponent<Node>();
    }

    public void OnTryBuild(object obj)
    {
        if (obj.GetType() == typeof(Structure))
        {
            if(Buildable.Contains((Structure)obj))
            {
                TryBuild((Structure)obj);
            }
        }
        else Debug.LogError("Builder.OnTryBuild is not a structure");
    }

    private void Demolish()
    {
        node.Structure.Demolish();
    }
    private void Demolish(object obj)
    {
        //This is for the RoundButton Delegate
        Demolish();
        BuildMenu.Instance.RefreshButtons();
    }

    private void TryBuild(Structure toBuild)
    {
        tryBuild = toBuild;
        
        QuestionPopupInfo popup = new QuestionPopupInfo
        {
            questionText = "Would you like to buy This for (temporary) " + 1,// SET THIS TO SCRIPTABLEOBJECT
            NoButtonText = "Cancel",
            YesButtonText = "Buy it",
            OnYes = OnBuyBuild
        };
        PopupHandler.Instance.OnQuestionPopup(popup);
    }

    public void OnBuyBuild()
    {
        //USE SCRIPTABLEOBJECT HERE
        /*bool bought;
        Economy.instance.OnBuy(new BuyableInfo(roadSO.buyInfo), out bought);
        if (bought)
        {*/
            manager.BuildStructure(tryBuild, node);
        /*}*/


    }

    public List<RoundButtonData> GetBuildableData()
    {
        List<RoundButtonData> buttonData = new List<RoundButtonData>();
        if(node.Structure != null)
        {
            buttonData.Add(GetDemolishButton());
            return buttonData;
        }
        foreach(Structure build in buildable)
        {
            RoundButtonData data = new RoundButtonData
            {
                buttonDelegate = OnTryBuild,
                sprite = build.sprite,
                text = build.StructureName,
                obj = build
            };
            buttonData.Add(data);
        }

        return buttonData;
    }

    private RoundButtonData GetDemolishButton()
    {
        RoundButtonData data = new RoundButtonData
        {
            buttonDelegate = Demolish,
            sprite = demolishSprite,
            text = "Demolish "+node.Structure.StructureName,
            obj = null
        };

        return data;
    }
}