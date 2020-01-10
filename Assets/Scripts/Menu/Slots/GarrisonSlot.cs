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
    private List<UnitSlot> unitSlots;

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
        unitSlots = new List<UnitSlot>();

        foreach (Unit unit in units)
        {
            GameObject slot = Instantiate(unitSlot, content.transform);
            UnitSlot uSlot = slot.GetComponent<UnitSlot>();
            uSlot.SetData(unit);
            uSlot.MoveToCommander(commander);
            if(GarrisonMenu.Instance.IsInTransfer(unit, commander))
            {
                uSlot.draggableImage.SetMovedColor();
            }
            unitSlots.Add(uSlot);
        }
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
        Unit newUnit = slot.unit;

        if(slot.CurrentCommander != commander)
        {
            menu.AddUnitSlotToTransfer(slot, out alreadyInTransfer);
        }

        slot.MoveToCommander(commander);
        commander.AddUnit(newUnit);

        SetTroopsList();
    }

    private void AddToCommander(UnitSlot slot)
    {
        commander.AddUnit(slot.unit);
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

    public void OnCancelTransfer()
    {
        foreach(UnitSlot uSlot in unitSlots)
        {
            uSlot.MoveToCommander(uSlot.unit.Commander);
        }
    }

    public void OnConfirmTransfer()
    {
        foreach(UnitSlot uSlot in unitSlots)
        {
            uSlot.unit.SetCommander(commander);
        }
    }

    public void OnPressRemove()
    {
        //doesnt seem necessary
        /*
        OnCancelTransfer();
        commanderButton.Deselect();
        commanderButton.RemoveFromSelectedCommanders();
        GarrisonMenu.Instance.UpdateAllGarrisonSlots();
        Remove();*/
    }

    void Remove()
    {
        Destroy(gameObject);
    }
}
