using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestionPopUp : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI questionText;
    [SerializeField]
    private Text yesButtonText;
    [SerializeField]
    private Text noButtonText;
    [SerializeField]
    private Button yesButton;
    [SerializeField]
    private Button noButton;

    public delegate void PressYesDelegate();
    public PressYesDelegate OnPressYes;

    public delegate void PressNoDelegate();
    public PressNoDelegate OnPressNo;

    public void Start()
    {
        questionText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void OnPopup(QuestionPopupInfo info)
    {
        try
        {
            OnPressYes += info.OnYes.Invoke;
        }
        catch (System.ArgumentException) { Close(); }
        OnPressYes += Close;
        try
        {
            OnPressNo += info.OnNo.Invoke;
        }
        catch(System.ArgumentException) { }
        OnPressNo += Close;
        gameObject.SetActive(true);
        questionText.text = info.questionText;
        yesButtonText.text = info.YesButtonText;
        noButtonText.text = info.NoButtonText;
    }

    public void OnButtonPress()
    {
        ClearDelegates();
        Close();
    }

    public void Close()
    {
        ClearDelegates();
        gameObject.SetActive(false);
    }

    public void PressYes()
    {
        OnPressYes?.Invoke();
        Close();
    }

    public void PressNo()
    {
        OnPressNo?.Invoke();
    }

    public void ClearDelegates()
    {
        OnPressYes = null;
        OnPressNo = null;
    }
}

