using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableImage : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Transform initialParent;
    public Commander initialCommander;
    public GraphicRaycaster caster;
    [SerializeField]
    private ScrollRect scrollRect;
    [SerializeField]
    private Image background;
    [SerializeField]
    private Color movedColor;
    private Color originalColor;

    public UnitSlot slot;
    [SerializeField]
    private bool allowDrag;
    [SerializeField]
    private bool pointerDown;

    [SerializeField]
    private float minPressTime = 1;
    [SerializeField]
    private float pressTimer = 0;

    void Awake()
    {
        caster = (GraphicRaycaster)Utility.GetFirstComponentInParents(gameObject, typeof(GraphicRaycaster));
        slot = (UnitSlot)Utility.GetFirstComponentInParents(gameObject, typeof(UnitSlot));
        slot.draggableImage = this;
        originalColor = background.color;
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        SetInitialReferences();
        RemoveFromParent();
        RemoveFromCommander();
    }

    public void OnDrag(PointerEventData eventData)
    {
        MoveWithDrag(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        List<RaycastResult> results = new List<RaycastResult>();
        caster.Raycast(eventData, results);

        foreach(RaycastResult result in results)
        {
            GarrisonSlot garSlot = result.gameObject.GetComponent<GarrisonSlot>();
            if (garSlot != null)
            {
                garSlot.OnAddUnitSlot(slot);
                return;
            }
        }

        ReturnToParent();
        ReturnToCommander();
        UnSetMovedColor();
        GarrisonSlot gSlot = (GarrisonSlot)Utility.GetFirstComponentInParents(gameObject, typeof(GarrisonSlot));
        gSlot.SetTroopsList();
    }

    public void OnCancelTransfer()
    {
        ReturnToCommander();
        UnSetMovedColor();
    }

    public void OnConfirmTransfer()
    {
        initialCommander = slot.unit.commander;
    }

    public void RemoveFromParent()
    {
        GameObject o = Utility.GetFirstParentWithComponent(gameObject, typeof(Canvas));
        slot.transform.SetParent(o.transform);
        SetMovedColor();
    }

    public void SetInitialReferences()
    {
        initialParent = slot.transform.parent;
        initialCommander = slot.unit.commander;
    }

    public void RemoveFromCommander()
    {
        slot.unit.commander.RemoveUnit(slot.unit);
    }

    public void ReturnToParent()
    {
        slot.transform.SetParent(initialParent, false);
    }

    public void ReturnToCommander()
    {
        slot.unit.commander.RemoveUnit(slot.unit);
        initialCommander.AddUnit(slot.unit);
        slot.unit.commander = initialCommander;
    }


    public void MoveToNewSlot(GarrisonSlot garSlot)
    {
        garSlot.OnAddUnitSlot(slot);
        SetMovedColor();
    }

    void MoveWithDrag(Vector3 pos)
    {
        slot.transform.position = pos;
        slot.unit.commander = initialCommander;
    }
    
    public void SetMovedColor()
    {
        background.color = movedColor;
    }

    public void UnSetMovedColor()
    {
        if(background != null) background.color = originalColor;
    }
}
