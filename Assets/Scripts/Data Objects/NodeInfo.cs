using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeInfo
{
    public NodeInfo(Node node)
    {
        sprite = node.rend.sprite;
        title = node.nodeName;
        description = node.nodeDescription;
        color = node.rend.color;
    }
    public Color color;
    public Sprite sprite;
    public string title;
    public string description;
}
