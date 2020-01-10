using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableImage : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Transform initialParent;
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
                slot.DestroySelf();
                return;
            }
        }
    
        ReturnToParent();
        UnSetMovedColor();
        GarrisonSlot gSlot = (GarrisonSlot)Utility.GetFirstComponentInParents(gameObject, typeof(GarrisonSlot));
        gSlot.SetTroopsList();
        Debug.Log("SetTroopsList");
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
    }

    public void ReturnToParent()
    {
        slot.transform.SetParent(initialParent, false);
    }
    
    public void AddToNewSlot(GarrisonSlot garSlot)
    {
        garSlot.OnAddUnitSlot(slot);
        SetMovedColor();
    }

    void MoveWithDrag(Vector3 pos)
    {
        slot.transform.position = pos;
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
