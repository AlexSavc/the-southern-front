using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class RoundButton : MonoBehaviour
{
    [Header("Everything here is automatic")]
    private Button button;
    private Image image;
    private string text = "button";
    UnityAction<RoundButtonData> buttonAction;
    RoundButtonData buttonData;

    public TextMeshProUGUI textMesh;

    public void SetButton(RoundButtonData data)
    {
        if(button == null)button = GetComponent<Button>();
        button.onClick.AddListener(InvokeButtonAction);

        buttonData = data;

        buttonAction = (RoundButtonData) => { RoundButtonData.buttonDelegate?.Invoke(buttonData.obj); };

        image = button.image;
        image.sprite = data.sprite;
        text = data.text;

        textMesh.text = text;
    }

    private void InvokeButtonAction()
    {
        buttonAction(buttonData);
    }
}

public class RoundButtonData
{
    public Sprite sprite;
    public string text;
    public object obj;
    public delegate void ButtonActionDelegate(object obj);
    public ButtonActionDelegate buttonDelegate;
}
