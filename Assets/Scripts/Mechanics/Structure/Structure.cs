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
    public ScriptableObject scriptableObject;

    public void SetOwner(Node node)
    {
        node.SetStructure(this);
        owner = node;
    }

    public void Demolish()
    {
        owner.RemoveStructure();
        Destroy(gameObject);
    }
}

public class StructureObject
{
    public int Xcoord;
    public int Ycoord;
}