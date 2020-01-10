using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarrisonMenu : MonoBehaviour
{
    private static GarrisonMenu _instance;
    public static GarrisonMenu Instance
    {
        get
        {
            return _instance;
        }
    }


    public List<Commander> selectedCommanders;
    public GameObject garrisonSlotObj;
    public GameObject panel;
    
    public List<Unit> unitTransfer;

    public GameObject slotHolder;
    
    void Start()
    {
        slotHolder = new GameObject();
        slotHolder.SetActive(false);
    }

    public void SetSingleton()
    {
        //THIS IS CALLED IN NODEMENU
        _instance = this;
    }

    public void OnCommanderpressed(Commander commander, CommanderButton button)
    {
        gameObject.SetActive(true);
        if (selectedCommanders.Contains(commander))
        {
            //you cound move the logic here from CommanderButton with an out bool success
        }
        else
        {
            GameObject slotObj = Instantiate(garrisonSlotObj, panel.transform);
            GarrisonSlot garrisonSlot = slotObj.GetComponent<GarrisonSlot>();
            garrisonSlot.commanderButton = button;
            selectedCommanders.Add(commander);
            garrisonSlot.SetData(commander);
        }
    }

    public int GetNumMenusOpen()
    {
        return panel.transform.childCount;
    }

    public void RemoveSlotsForCommander(Commander commander)
    {
        foreach(GameObject slot in Utility.GetChildren(panel.transform))
        {
            if(slot.GetComponent<GarrisonSlot>().commander == commander)
            {
                Destroy(slot);
            }
        }

        selectedCommanders.Remove(commander);
    }

    void AddCommander(Commander commander)
    {

    }

    public void AddUnitSlotToTransfer(UnitSlot slot, out bool alreadyInTransfer)
    {
        if (!unitTransfer.Contains(slot.unit))
        {
            unitTransfer.Add(slot.unit);
            alreadyInTransfer = false;
        }
        else { Debug.Log("Already in transfer"); alreadyInTransfer = true; }
    }

    public void OnPressTransfer()
    {
        ConfirmTransfer();
    }

    public void OnPressCancel()
    {
        CancelTransfer();
    }

    public void OnPressGarrissons()
    {
        HideCanvas(true);
    }

    public void ConfirmTransfer()
    {
        foreach (GarrisonSlot garSlot in GetGarrisonSlots())
        {
            garSlot.OnConfirmTransfer();
        }
        unitTransfer.Clear();
        UpdateAllGarrisonSlots();
    }

    public void CancelTransfer()
    {
        foreach (GarrisonSlot garSlot in GetGarrisonSlots())
        {
            garSlot.OnCancelTransfer();
        }
        unitTransfer.Clear();
        UpdateAllGarrisonSlots();
    }

    public void UpdateAllGarrisonSlots()
    {
        foreach(GarrisonSlot garSlot in GetGarrisonSlots())
        {
            garSlot.SetTroopsList();
        }
    }

    public bool IsInTransfer(Unit unit, Commander currentCommander)
    {
        foreach (Unit u in unitTransfer)
        {
            if (unit == u)
            {
                if (unit.Commander == currentCommander)
                {
                    RemoveFromTransfer(unit);
                    return false;
                }
                else return true;
            }
        }

        return false;
    }

    private void RemoveFromTransfer(Unit unit)
    {
        unitTransfer.Remove(unit);
    }

    private void HideCanvas(bool hide)
    {
        Canvas canvas = GetComponent<Canvas>();
        if (hide) canvas.enabled = false;
        else canvas.enabled = true;
    }

    public void OnDisable()
    {
        CancelTransfer();
        selectedCommanders.Clear();
    }

    public List<UnitSlot> GetUnitSlots(GarrisonSlot garSlot)
    {
        List<UnitSlot> unitSlots = new List<UnitSlot>();
        List<GameObject> objs = Utility.GetChildren(garSlot.content.transform);
        foreach(GameObject obj in objs)
        {
            if(obj.GetComponent<UnitSlot>())
            {
                unitSlots.Add(obj.GetComponent<UnitSlot>());
            }
        }

        return unitSlots;
    }

    private GarrisonSlot[] GetGarrisonSlots()
    {
        GarrisonSlot[] slots = panel.GetComponentsInChildren<GarrisonSlot>();
        return slots;
    }
}
