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

        foreach(Unit unit in units)
        {
            GameObject slot = Instantiate(unitSlot, content.transform);
            UnitSlot uSlot = slot.GetComponent<UnitSlot>();
            uSlot.SetData(unit);
            if(GarrisonMenu.Instance.IsInTransfer(unit)) { uSlot.draggableImage.SetMovedColor(); }
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
        AddToCommander(slot);

        GarrisonMenu menu = (GarrisonMenu)Utility.GetFirstComponentInParents(gameObject, typeof(GarrisonMenu));
        menu.AddUnitSlotToTransfer(slot);

        SetTroopsList();
    }

    private void AddToCommander(UnitSlot slot)
    {
        commander.AddUnit(slot.unit);
        slot.unit.commander = commander;
    }

    public void OnRecruit()
    {
        Unit added = Instantiate(WarEconomy.Instance.militia);
        added.commander = commander;
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
