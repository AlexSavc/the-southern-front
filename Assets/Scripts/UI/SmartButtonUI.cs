using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SmartButtonUI : MonoBehaviour
{
    public Button button;
    public TextMeshProUGUI text;

    public string enabledText = "This button is active";
    public string disabledText = "This button is inactive";

    public bool isListButton = true;
    public bool stickToTop = false;

    void Start()
    {
        if (text == null) text = GetComponentInChildren<TextMeshProUGUI>();
        if (button == null) button = GetComponent<Button>();
    }

    public void Deactivate()
    {
        text.SetText(disabledText);
        button.interactable = false;
    }

    public void Activate()
    {
        text.SetText(enabledText);
        button.interactable = true;
    }

    public void ResetChildPosition()
    {
        if(stickToTop)
        {
            transform.SetAsFirstSibling();
        }
        else
        {
            transform.SetAsLastSibling();
        }
    }
}
