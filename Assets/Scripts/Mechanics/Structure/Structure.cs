using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour
{
    [SerializeField]
    private Node owner;
    public Node Owner { get { return owner; } }

    [SerializeField]
    private SpriteRenderer rend;
    public Sprite sprite { get { return rend.sprite; } }

    [SerializeField]
    private string structureName;
    public string StructureName { get { return structureName; } }

    //FIX SCRIPTABLEOBJECTS
    public BuyableInfoSO buyableInfo;

    public Structure(string name)
    {
        structureName = name;
    }

    public void SetOwner(Node node)
    {
        node.SetStructure(this);
        owner = node;
        owner.GetOwner().AddBuyable(buyableInfo.buyInfo);
    }

    public void SetOwnerInitial(Node node)
    {
        node.SetStructure(this);
        owner = node;
        owner.GetOwner().AddBuyableNoRefresh(buyableInfo.buyInfo);
    }

    public void Demolish()
    {
        Economy.Instance.AddGold(buyableInfo.buyInfo.recupOnDisband);
        owner.GetOwner().RemoveBuyable(buyableInfo.buyInfo);
        owner.RemoveStructure();
        Destroy(gameObject);
    }
}

public class StructureObject
{
    public int Xcoord;
    public int Ycoord;
}