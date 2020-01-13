using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class RoundButton : MonoBehaviour
{
    Button button;
    Image image;
    string text;
    UnityAction<RoundButtonData> buttonAction;
    RoundButtonData buttonData;

    public void SetButton(RoundButtonData data)
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(InvokeButtonAction);

        buttonAction = (RoundButtonData) => { RoundButtonData.buttonDelegate?.Invoke(buttonData.obj); };

        image = button.image;
        image.sprite = data.sprite;
        text = data.text;
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
