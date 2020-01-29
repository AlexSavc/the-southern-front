using System.Collections;
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

    void Awake()
    {
        node = GetComponent<Node>();
        manager = StructureManager.Instance;
    }

    void Start()
    {
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
        BuildMenu.Instance.RefreshButtons();
    }

    private void TryDemolish(object obj)
    {
        //This is for the RoundButton Delegate
        QuestionPopupInfo popup = new QuestionPopupInfo
        {
            questionText = "Would you like to demolish " + node.Structure.StructureName + 
                           " and get "+node.Structure.buyableInfo.buyInfo.recupOnDisband +" Geld back",
            NoButtonText = "No",
            YesButtonText = "Yes",
            OnYes = Demolish
        };
        PopupHandler.Instance.OnQuestionPopup(popup);
    }

    private void TryBuild(Structure toBuild)
    {
        tryBuild = toBuild;
        
        QuestionPopupInfo popup = new QuestionPopupInfo
        {
            questionText = "Would you like to buy This for " + tryBuild.buyableInfo.buyInfo.price + " Geld",
            NoButtonText = "No",
            YesButtonText = "Yes",
            OnYes = OnBuyBuild
        };
        PopupHandler.Instance.OnQuestionPopup(popup);
    }

    public void OnBuyBuild()
    {
        bool bought;
        Economy.Instance.OnBuy(new BuyableInfo(tryBuild.buyableInfo.buyInfo), out bought);
        if (bought)
        {
            Build(tryBuild);
        }
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
            buttonDelegate = TryDemolish,
            sprite = demolishSprite,
            text = "Demolish "+node.Structure.StructureName,
            obj = null
        };

        return data;
    }

    public void Build(Structure structure)
    {
        manager.BuildStructure(structure, node);
    }

    public void BuildByName(string structureName)
    {
        foreach(Structure structure in buildable)
        {
            if(structureName == structure.StructureName)
            {
                //In the beginning, the players are not set, so to avoid nullreferenceExceptions,
                // we don't make the Economy Refresh by avoiding Player.AddBuyable
                manager.BuildInitial(structure, node);
                return;
            }
        }
    }
}