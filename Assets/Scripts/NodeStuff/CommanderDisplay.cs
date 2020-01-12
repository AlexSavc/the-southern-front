using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommanderDisplay : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer profileRend;
    public SpriteRenderer ProfileRend { get { return profileRend; } }
    [SerializeField]
    private SpriteRenderer outlineRend;
    public SpriteRenderer OutlineRend { get { return outlineRend; } }

    public void SetTeamColor(Commander commander)
    {
        outlineRend.color = commander.Owner.playerColor;
    }

    public void SetSpriteRendOrder(bool isSelected)
    {
        if (isSelected) profileRend.sortingOrder = 3;
        else profileRend.sortingOrder = 1;
    }
}
