using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MapCreationUtility : MonoBehaviour
{
    public string savePath;
    public string[] files;

    public GameObject mapSlotHolder;
    public PlayMenu playMenu;

    public MapSlot[] mapSlots;

    void Start()
    {
        if (playMenu == null) playMenu = FindObjectOfType<PlayMenu>();
    }

    public void OnEnable()
    {
        SetSinglePlayerPath();
    }

    public void Create(MapCreateInfo info)
    {
        info.savePath = savePath + "/" + info.mapName + ".json";

        string save = JsonUtility.ToJson(info);
        File.WriteAllText(info.savePath, save);

        SceneChanger changer = FindObjectOfType<SceneChanger>();
        changer.CreateMap(info.savePath);
    }

    public void SetSinglePlayerPath()
    {
        if (Directory.Exists(Application.persistentDataPath + "/Saves/Singleplayer"))
        {
            savePath = Application.persistentDataPath + "/Saves/Singleplayer";
        }
        else
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Saves/Singleplayer");
            savePath = Application.persistentDataPath + "/Saves/Singleplayer";
        }
    }

    public void Refresh()
    {
        if (savePath == null) SetSinglePlayerPath();
        DirectoryInfo directory = new DirectoryInfo(savePath);
        files = Directory.GetFiles(savePath, "*.json");
        FileInfo[] fileInfo = directory.GetFiles("*.json");
        mapSlots = mapSlotHolder.transform.GetComponentsInChildren<MapSlot>();
        

        foreach (MapSlot slot in mapSlots)
        {
            Destroy(slot.gameObject);
        }
        
        for (int i = 0; i < files.Length; i++)
        { 
            playMenu.AddMapSlot(files[i]);
        }

        playMenu.SetAddButtonAsLast();
    }

    public string GetSinglePlayerPath()
    {
        if (savePath == null) SetSinglePlayerPath();
        return savePath;
    }
}
