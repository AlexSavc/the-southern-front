using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class Interaction : MonoBehaviour
{
    //TODO

    /*Iselectable ecevnt is called on every selectable the raycast hits. they nust be queued and handled one by one*/

    private static Interaction _instance;
    public static Interaction Instance
    {
        get { return _instance; }
    }

    public GameObject selected;
    public List<GameObject> overlap;

    public delegate void SelectionDelegate(GameObject selected);
    public event SelectionDelegate selectionEvent;

    public delegate void ISelectableDelegate(ISelectable selected);
    public event ISelectableDelegate ISelectableEvent;

    public bool artificialSelection;

    void Start()
    {
        overlap = new List<GameObject>();
        _instance = this;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Utility.IsPonterOverUIObject()) {  return; }
            Select(ScreenMouseRay());
        }
    }

    public void Select(GameObject hit)
    {
        if (hit == null) { DeSelecteSelected(); selectionEvent?.Invoke(selected); return; }

        GameObject[] hits = AllScreenMouseRay();

        try
        {
            //See if it's Selectable
            ISelectable[] selectable = hit.GetComponents<ISelectable>();
            //trigger all selectable components
            if (selectable.Length > 0)
            {
                selectable[0].OnSelection();
                ISelectableEvent?.Invoke(selectable[0]);
                //Let's not do all
            }
        }
        catch (NullReferenceException) { return; /*No selection if no ISelectable */}

        //if you press the same tile, with the same occupied
        if (Equals(hits, overlap))
        {

        }

        if (selected != null)
        {
            try
            {
                IInteractable[] interactable = hit.GetComponents<IInteractable>();
                if (interactable.Length > 0)
                {
                    foreach (IInteractable interact in interactable)
                        interact.OnInteraction(selected);
                }
            }
            catch (NullReferenceException) { }
        }

        DeSelecteSelected();
        selected = hit;
        selectionEvent?.Invoke(selected);
    }

    public void DeSelecteSelected()
    {
        try
        {
            if (selected != null)
                selected.GetComponent<ISelectable>().OnDeSelection();
        }
        catch (NullReferenceException) { };
        selected = null;
    }

    public GameObject[] AllScreenMouseRay()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 5f;

        Vector2 v = Camera.main.ScreenToWorldPoint(mousePos);

        Collider2D[] colliders = Physics2D.OverlapPointAll(v);


        if (colliders.Length > 0)
        {
            GameObject[] toReturn = new GameObject[colliders.Length];
            for (int i = 0; i < colliders.Length; i++)
            {
                toReturn[i] = colliders[i].gameObject;
            }

            return toReturn;
        }
        else return null;
    }

    public GameObject ScreenMouseRay()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 5f;

        Vector2 v = Camera.main.ScreenToWorldPoint(mousePos);

        Collider2D collider = Physics2D.OverlapPoint(v);
        if (collider == null) return null;
        return collider.gameObject;
    }

    private bool IsPonterOverUIObject()
    {
        PointerEventData eventDataMousePos = new PointerEventData(EventSystem.current);
        Vector2 mousePos = Input.mousePosition;
        eventDataMousePos.position = new Vector2(mousePos.x, mousePos.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataMousePos, results);

        if (results.Count > 0) return true;
        else return false;
    }
}
