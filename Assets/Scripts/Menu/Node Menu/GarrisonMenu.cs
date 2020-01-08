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

    public List<DraggableImage> transfer;
    public List<Unit> unitTransfer;

    public GameObject slotHolder;
    
    void Start()
    {
        transfer = new List<DraggableImage>();
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

    public void AddUnitSlotToTransfer(UnitSlot slot)
    {
        if (!transfer.Contains(slot.draggableImage))
        {
            transfer.Add(slot.draggableImage);
            slot.gameObject.transform.SetParent(slotHolder.transform);

            Debug.Log("Added to treansfer");
        }
    }

    public void RemoveUnitSlotFromTransfer(UnitSlot slot)
    {
        if (transfer.Contains(slot.draggableImage))
        {
            transfer.Remove(slot.draggableImage);
            if(Utility.GetChildren(slotHolder.transform).Contains(slot.gameObject))
            {
                slot.DestroySelf();
            }
        }
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
        foreach(DraggableImage image in transfer)
        {
            image.OnConfirmTransfer();
        }
        transfer.Clear();
        UpdateAllGarrisonSlots();
    }

    public void CancelTransfer()
    {
        foreach (DraggableImage image in transfer)
        {
            image.OnCancelTransfer();
        }
        transfer.Clear();
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
        //RefreshTransfers();
        foreach (DraggableImage image in transfer)
        {
            if (unit == image.slot.unit) return true;
        }

        return false;
    }

    private void RefreshTransfers()
    {
        List<DraggableImage> toRemove = new List<DraggableImage>();
        foreach (DraggableImage image in transfer)
        {
            if (image.slot.unit.commander == image.initialCommander)
            {
                toRemove.Add(image);
            }
        }

        foreach (DraggableImage image in toRemove)
        {
            transfer.Remove(image);
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
}
