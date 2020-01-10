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

    //public List<DraggableImage> transfer;
    public List<Unit> unitTransfer;

    public GameObject slotHolder;
    
    void Start()
    {
        //transfer = new List<DraggableImage>();
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


    public void ClearAllSlots()
    {

    }

    public void AddUnitSlotToTransfer(UnitSlot slot, out bool alreadyInTransfer)
    {
        if (!unitTransfer.Contains(slot.unit))
        {
            unitTransfer.Add(slot.unit);
            Debug.Log("Added to transfer");
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
        foreach(Unit unit in unitTransfer)
        {

        }
        unitTransfer.Clear();
        UpdateAllGarrisonSlots();
    }

    public void CancelTransfer()
    {
        foreach (Unit unit in unitTransfer)
        {

        }
        unitTransfer.Clear();
        UpdateAllGarrisonSlots();
    }

    void UpdateAllGarrisonSlots()
    {
        GarrisonSlot[] slots = panel.GetComponentsInChildren<GarrisonSlot>();

        foreach(GarrisonSlot garSlot in slots)
        {
            garSlot.SetTroopsList();
        }
    }

    public bool IsInTransfer(Unit unit)
    {
        foreach (Unit u in unitTransfer)
        {
            if (unit == u) return true;
        }

        return false;
    }

    public void RefreshTransfers()
    {
        GarrisonSlot[] garSlots = panel.GetComponentsInChildren<GarrisonSlot>();
        foreach(GarrisonSlot garSlot in garSlots)
        {
            List<UnitSlot> unitSlots = GetUnitSlots(garSlot);
            foreach (UnitSlot unitSlot in unitSlots)
            {
                if(unitSlot.unit.Commander == unitSlot.currentCommander)
                {
                    unitTransfer.Remove(unitSlot.unit);
                }
                /*if (unitSlot.unit.commander == garSlot.commander)
                {
                    if(IsInTransfer(unitSlot.unit))
                    {
                        unitTransfer.Remove(unitSlot.unit);
                    }
                }*/
            }
        }
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
}
