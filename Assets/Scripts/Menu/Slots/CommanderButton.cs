using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CommanderButton : MonoBehaviour
{
    public Image selected;
    public Image rankSprite;
    public Image picture;
    //for legendary status
    public Image background;
    public Image foreground;
    public Button button;
    public CommanderMenu commanderMenu;
    public GarrisonMenu garrisonMenu;
    public Commander commander;
    public TextMeshProUGUI nametext;

    void Start()
    {
        commanderMenu = FindObjectOfType<CommanderMenu>();
        garrisonMenu = GarrisonMenu.Instance;
    }
    public void SetCommander(Commander command)
    {
        commander = command;
        SetDisplay();
    }

    public void RefreshDisplay()
    {
        SetDisplay();
    }

    public void OnPress()
    {
        if (commander == null) { Remove(); return; }
        if(garrisonMenu.selectedCommanders.Contains(commander))
        {
            int index = garrisonMenu.selectedCommanders.IndexOf(commander);
            garrisonMenu.RemoveSlotsForCommander(commander);
            Deselect();
            return;
        }

        Select();
        GameObject garMenuObj = GarrisonMenu.Instance.gameObject;
        garMenuObj.SetActive(true);
        garrisonMenu = garMenuObj.GetComponent<GarrisonMenu>();
        garrisonMenu.OnCommanderpressed(commander, this);
    }


    public void SetDisplay()
    {
        Deselect();
        rankSprite.sprite = commander.GetRankSprite();
        picture.sprite = commander.sprite;
        nametext.text = commander.commaderName;
    }

    public void Select()
    {
        selected.enabled = true;
    }

    public void Deselect()
    {
        selected.enabled = false;

        
    }

    public void RemoveFromSelectedCommanders()
    {
        List<Commander> selectCom = garrisonMenu.selectedCommanders;
        if (selectCom.Contains(commander)) { selectCom.Remove(commander); }
    }
    void Remove()
    {
        Destroy(gameObject);
    }
}
