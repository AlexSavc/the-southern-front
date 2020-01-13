using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour
{
    [SerializeField]
    private Node owner;
    public Node Owner { get { return owner; } }

    public void SetOwner(Node node)
    {
        node.SetStructure(this);
        owner = node;
    }
}

public class StructureObject
{
    public int Xcoord;
    public int Ycoord;
}
