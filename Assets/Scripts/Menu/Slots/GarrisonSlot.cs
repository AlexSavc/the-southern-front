using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GarrisonSlot : MonoBehaviour
{
    public UnityEngine.UI.Image commanderSprite;
    public TextMeshProUGUI commanderName;
    public GameObject unitSlot;
    public GameObject content;
    public GameObject recruitButton;
    public CommanderButton commanderButton;
    public Commander commander;
    public List<Unit> units;

    public List<GameObject> stickToTop;
    public List<GameObject> stickToBottom;

    public void SetData(Commander commander)
    {
        this.commander = commander;
        commanderSprite.sprite = commander.sprite;
        commanderName.text = commander.commaderName;
        SetTroopsList();
    }

    public void SetTroopsList()
    {
        ClearTroopsList();
        if (commander.Units == null) return;
        units = commander.Units;

        
        foreach (Unit unit in units)
        {
            GameObject slot = Instantiate(unitSlot, content.transform);
            UnitSlot uSlot = slot.GetComponent<UnitSlot>();
            uSlot.SetData(unit);
            if(GarrisonMenu.Instance.IsInTransfer(unit)) { uSlot.draggableImage.SetMovedColor(); }
        }
        GarrisonMenu.Instance.RefreshTransfers();
        recruitButton.transform.SetAsLastSibling();
    }

    public void ClearTroopsList()
    {
        foreach(GameObject obj in Utility.GetChildren(content.transform))
        {
            if(stickToBottom.Contains(obj) == false )
            {
                if(stickToTop.Contains(obj) == false)
                {
                    Destroy(obj);
                }
            }
        }
    }

    public void OnAddUnitSlot(UnitSlot slot)
    {
        bool alreadyInTransfer;
        GarrisonMenu menu = (GarrisonMenu)Utility.GetFirstComponentInParents(gameObject, typeof(GarrisonMenu));

        //If you move from original commander to original commander, do nothing
        if(slot.unit.Commander != commander)
        {
            //If you transfer it to a different one than the ORIGINAL commander, check if it was already transferred
            menu.AddUnitSlotToTransfer(slot, out alreadyInTransfer);
            //If so, there's no need to add it again
            if(!alreadyInTransfer) AddToCommander(slot);
            else { slot.unit.Commander.RemoveUnit(slot.unit); }
        }

        SetTroopsList();
    }

    private void AddToCommander(UnitSlot slot)
    {
        commander.AddUnit(slot.unit);
        //slot.unit.commander = commander;
    }

    public void OnRecruit()
    {
        Unit added = Instantiate(WarEconomy.Instance.militia);
        added.SetCommander(commander);
        commander.AddUnit(added);
        SetTroopsList();
    }

    public void OnDisable()
    {
        Remove();
    }

    public void OnPressRemove()
    {
        commanderButton.Deselect();
        commanderButton.RemoveFromSelectedCommanders();
        Remove();
    }

    void Remove()
    {
        Destroy(gameObject);
    }
}
