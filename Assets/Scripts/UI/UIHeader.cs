using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHeader : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI textMesh;

    public void SetText(string text)
    {
        textMesh.text = text;
    }

    public string GetText()
    {
        return textMesh.text;
    }
}
