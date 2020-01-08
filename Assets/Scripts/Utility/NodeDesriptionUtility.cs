using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeDesriptionUtility : MonoBehaviour
{
    Interaction interaction;
    TurnManager turnManager;

    public string yourNodeDescription = "You can Build units here. Brings 1 Gold per turn";

    void Start()
    {
        if(interaction == null) interaction = FindObjectOfType<Interaction>();
        if(turnManager == null) turnManager = FindObjectOfType<TurnManager>();
    }

    public NodeDescription GetDescription(Node node)
    {
        NodeDescription description = new NodeDescription
        {
            nodeSprite = node.rend.sprite,
            nodeName = node.nodeName,
            nodeDescription = node.nodeDescription
        };

        return SetNodeDescription(node, description);
    }

    NodeDescription SetNodeDescription(Node node, NodeDescription description)
    {
        if(node.GetOwner() != null)
        {
            if (turnManager.currentPlayer != node.GetOwner())
            {
                description.nodeName = node.GetOwner().playerName + " 's Node";
                description.nodeDescription = "Enemy node";
            }
            else
            {
                description.nodeName = "Your Node";
                description.nodeDescription = yourNodeDescription;
            }
        }
        description.color = node.rend.color;
        
        return description;
    }
}
