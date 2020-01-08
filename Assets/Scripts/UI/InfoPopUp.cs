using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InfoPopUp : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI warning;
    [SerializeField]
    private Text buttonText;
    [SerializeField]
    private Button button;

    public void Start()
    {
        warning = GetComponentInChildren<TextMeshProUGUI>();
        button = GetComponentInChildren<Button>();
        buttonText = button.gameObject.GetComponentInChildren<Text>();
    }

    public void OnPopup(string warn, string btn)
    {
        gameObject.SetActive(true);
        warning.text = warn;
        buttonText.text = btn;
    }

    public void OnButtonPress()
    {
        Close();
    }

    void Close()
    {
        gameObject.SetActive(false);
    }
}
