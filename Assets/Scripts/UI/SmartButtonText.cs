using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SmartButtonText : MonoBehaviour
{
    public string text;
    private TextMeshProUGUI textMesh;

    public void Start()
    {
        //textMesh = GetComponentInChildren<TextMeshProUGUI>();
        //Refresh();
    }

    public void Refresh()
    {
        text = gameObject.name;
        textMesh.SetText(text);
    }
}
