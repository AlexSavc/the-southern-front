using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Interaction1 : MonoBehaviour
{
    public delegate void SelectionDelegate(GameObject selected);
    public event SelectionDelegate selectionEvent;

    public delegate void DragDelegate(GameObject selected);
    public event DragDelegate dragEvent;

    public List<GameObject> overlap;
    public GameObject selected;

    void Start()
    {
        //FindObjectOfType<TurnManager>().turnChangeEvent += ClearSelection;
        overlap = new List<GameObject>();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            GameObject[] hits = AllScreenMouseRay();

            GameObject toSelect = null;

            //if you press the same tile, with the same occupied
            if (IsTheSame(hits, overlap.ToArray()))
            {
                if (selected != null)
                {
                    // rotate the selections
                    if(overlap.Contains(selected))
                    {
                        int index = overlap.IndexOf(selected);
                        if (index < overlap.Count - 1)
                        {
                            toSelect = overlap[index + 1];
                        }
                        else
                        {
                            toSelect = overlap[0];
                        }
                    }
                }
            }
            //if you press on another tile
            else
            {
                // set new overlap list, select first
                List<GameObject> H = new List<GameObject>();

                if(hits != null)
                {
                    foreach (GameObject obj in hits)
                    {
                        H.Add(obj);
                    }

                    toSelect = H[0];
                }
                
                overlap = H;

                
            }

            
            selected = toSelect;
            selectionEvent?.Invoke(selected);
        }

        if (Input.GetMouseButton(0))
        {
            GameObject hit = ScreenMouseRay();
            if (selected != hit)
            {
                dragEvent?.Invoke(hit);
                selected = hit;
                /*try
                {
                    IInteractable[] interactable = hit.GetComponents<IInteractable>();
                    if (interactable.Length > 0)
                    {
                        foreach (IInteractable interact in interactable)
                            interact.OnInteraction(selected);
                    }
                }
                catch (NullReferenceException) { }*/
            }
        }
    }

    /*public void ClearSelection(Party party)
    {
        selected = null;
    }*/

    public GameObject ScreenMouseRay()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 5f;

        Vector2 v = Camera.main.ScreenToWorldPoint(mousePos);

        Collider2D collider = Physics2D.OverlapPoint(v);
        if (collider == null) return null;
        return collider.gameObject;
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

    void OnDestroy()
    {
        //FindObjectOfType<TurnManager>().turnChangeEvent -= ClearSelection;
    }

    public bool IsTheSame(GameObject[] one, GameObject[]two)
    {

        if (one == null  || two == null || one.Length != two.Length) return false;
        else
        {
            for(int i = 0; i < one.Length; i++)
            {
                if (one[i] != two[i]) return false;
            }
        }

        return true;
    }
}

/*
 old Interact
 if(Input.GetMouseButtonDown(0))
        {
            GameObject[] hits = AllScreenMouseRay();
            GameObject hit = ScreenMouseRay();

            //if you press the same tile, with the same occupied
            if (Equals(hits, overlap))
            {

            }

            if(selected != null)
            {
                try
                {
                    IInteractable[] interactable = hit.GetComponents<IInteractable>();
                    if (interactable.Length > 0)
                    {
                        foreach(IInteractable interact in interactable)
                        interact.OnInteraction(selected);
                    }
                }
                catch (NullReferenceException) { }
            }
            
            selected = hit;
            selectionEvent?.Invoke(selected);
        }
     
     */
