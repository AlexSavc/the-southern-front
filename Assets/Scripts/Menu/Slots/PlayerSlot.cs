using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerSlot : MonoBehaviour
{
    public CreationMenu creationMenu;

    public TMP_InputField playerName;
    public Dropdown playerType;
    public Dropdown playerColor;
    public Dropdown playerTeam;

    List<Dropdown.OptionData> colorOptions;

    public Sprite colorSprite;
    
    public void Start()
    {
        if(creationMenu == null)
        {
            creationMenu = FindObjectOfType<CreationMenu>();
        }

        creationMenu.onAddPlayer += OnPlayerAdded;

        
    }

    public void InitializeDropdowns()
    {
        //CALLED FROM CreationMenu.OnPlayerAdded()!!!!
        SetTeamDropDown();
        SetTeamColors();
    }

    public void OnPlayerAdded()
    {
        SetTeamDropDown();
    }

    public void SetTeamDropDown()
    {
        playerTeam.ClearOptions();

        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();

        for(int i = 0; i < PlayerCount(); i++)
        {
            Dropdown.OptionData option = new Dropdown.OptionData(i+"");
            options.Add(option);
        }

        playerTeam.options = options;
    }

    public void SetTeamColors()
    {
        //THIS IS A PROBLEM, MATCHING SPRITE COLORS WITH REAL COLORS
        playerColor.ClearOptions();
        colorOptions = new List<Dropdown.OptionData>();

        Sprite[] sprites = creationMenu.playerColors;
        for (int i =0; i< creationMenu.playerColors.Length; i++)
        {
            Dropdown.OptionData option = new Dropdown.OptionData(sprites[i]);
            colorOptions.Add(option);
        }

        playerColor.options = colorOptions;
    }

    public int PlayerCount()
    {
        //to account for button
        return transform.parent.childCount - 1;
    }

    public void OnDelete()
    {
        creationMenu.onAddPlayer -= OnPlayerAdded;
        //So the menu doesnt count it anymore*/
        transform.SetParent(null);
        creationMenu.RemovePlayerSlot(this);
    }
}
