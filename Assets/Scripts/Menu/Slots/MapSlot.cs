using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;

public class MapSlot : MonoBehaviour
{
    public TextMeshProUGUI mapName;
    public Button mapButton;
    public Button deleteMap;

    private string savePath;

    public delegate void DeleteEvent();
    public event DeleteEvent onDeleteMap;

    public void Start()
    {

    }

    public void SetMapName(string newName)
    {
        mapName.text = newName;
    }

    public void SetPath(string path, bool resetName)
    {
        savePath = path;

        if(resetName)
        {
            SetMapName(Path.GetFileNameWithoutExtension(path));
        }
    }

    public void PlayMap()
    {
        SceneChanger changer = FindObjectOfType<SceneChanger>();
        changer.LoadMap(savePath);
    }

    public void OnDeleteMap()
    {
        GetWarningPopup();
    }

    void DeleteMap()
    {
        if (File.Exists(savePath + ".meta"))
        {
            File.Delete(savePath + ".meta");
        }

        if (File.Exists(savePath))
        {
            File.Delete(savePath);
        }
        Destroy(gameObject);
    }

    public void GetWarningPopup()
    {
        QuestionPopupInfo question = new QuestionPopupInfo();
        question.OnYes = DeleteMap;
        question.OnNo = null;
        question.questionText = "Do you really wanna delete this Map?";
        question.YesButtonText = "Yes, Burn it";
        question.NoButtonText = "Spare it";
        FindObjectOfType<PopupHandler>().OnQuestionPopup(question);
    }

    void OnDestroy()
    {
        onDeleteMap?.Invoke();
    }
}
