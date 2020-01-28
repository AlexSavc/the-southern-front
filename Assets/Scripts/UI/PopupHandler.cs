using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PopupHandler : MonoBehaviour
{
    private static PopupHandler _instance;
    public static PopupHandler Instance { get { return _instance; } } 
    public GameObject infoPopup;
    public GameObject questionPopup;

    public List<object> popupQueue;

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        popupQueue = new List<object>();
    }

    public void OnInfoPupup(InfoPopupInfo info)
    {
        infoPopup.SetActive(true);
        infoPopup.GetComponent<InfoPopUp>().OnPopup(info.infoText, info.okText);
    }

    public void OnQuestionPopup(QuestionPopupInfo info)
    {
        questionPopup.SetActive(true);
        questionPopup.GetComponent<QuestionPopUp>().OnPopup(info);
    }

    public void AddToQueue(object obj)
    {
        popupQueue.Add(obj);
        DoAllPopups();
    }

    public void DoAllPopups()
    {
        if (popupQueue.Count == 0) return;
        NextInQueue();
    }

    public void OnDismissPopup()
    {
        popupQueue.RemoveAt(0);
        NextInQueue();
    }

    public void NextInQueue()
    {
        if (popupQueue.Count == 0) { return; }

        object obj = popupQueue[0];
        if (obj.GetType() == typeof(QuestionPopupInfo))
        {
            OnQuestionPopup((QuestionPopupInfo)obj);
            popupQueue.Remove(obj);
        }
        else if (obj.GetType() == typeof(InfoPopupInfo))
        {
            OnInfoPupup((InfoPopupInfo)obj);
            popupQueue.Remove(obj);
        }
        else Debug.LogError("PopUpHandler.AddToQueue(object obj): tried adding an object which is not a referenced popupInfo");

    }
}

public class QuestionPopupInfo
{
    public Action OnYes { get; set; }
    public Action OnNo { get; set; }
    public string questionText;
    public string YesButtonText;
    public string NoButtonText;
}

public class InfoPopupInfo
{
    public string infoText;
    public string okText;
}
