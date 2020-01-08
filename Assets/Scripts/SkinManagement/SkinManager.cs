using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SkinManager : MonoBehaviour
{
    /// When adding new UI Elements, you must UPDATE:
    /// - SetTexturesFromEdit()
    /// - SkinData's Texutre fields
    /// - SetSprites()
    public Sprite placeHolder;

    [HideInInspector]
    public SkinData currentSkin;

    [HideInInspector]
    public SkinData editing;
    [HideInInspector]
    public string skinName;
    [HideInInspector]
    public Sprite panelSprite;
    [HideInInspector]
    public Sprite buttonSprite;
    [HideInInspector]
    public Sprite technicalButtonSprite;
    [HideInInspector]
    public Sprite elementSprite;
    [HideInInspector]
    public Sprite headerSprite;

    [HideInInspector]
    public int skinIndex;
    [HideInInspector]
    public List<SkinData> skins;
    [HideInInspector]
    public int textIndex;
    [HideInInspector]
    public string onTextIDNameEdit;
    [HideInInspector]
    public List<string> textTypeIDs;
    public Dictionary<string, Color> IDColorPairs;

    [HideInInspector]
    public string savePath;

    void Start()
    {
        SetSavePath();
        GetSaves();
    }

    public void SetCurrentSkin(SkinData skin)
    {
        currentSkin = skin;
    }

    public void SetEditSkin(SkinData skin)
    {
        editing = skin;
        SetSprites();
    }

    public void AddNewSkin()
    {
        if (skins == null) skins = new List<SkinData>();
        string tempName = "New Skin " + (skins.Count+1);
        skins.Add(new SkinData() { skinName = tempName, colors = new List<Color>(textTypeIDs.Count) });
    }

    public void AddSkin(SkinData skin)
    {
        skins.Add(skin);
    }

    public void AddNewTextID()
    {
        string tempName = "New Text Type" + textTypeIDs.Count;
        textTypeIDs.Add(tempName);
    }

    public string[] SkinsToArray()
    {
        List<string> names = new List<string>();
        foreach(SkinData data in skins)
        {
            names.Add(data.skinName);
        }
        return names.ToArray();
    }

    public void ConfirmTextIDNameEdit()
    {
        textTypeIDs[textIndex] = onTextIDNameEdit;
    }

    public void SetSkinDataColorList(SkinData skinData)
    {
        if (skinData.colors == null) skinData.colors = new List<Color>(textTypeIDs.Count);
        if(skinData.colors.Count > textTypeIDs.Count)
        {
            Debug.Log("Bigger");
            skinData.colors.RemoveRange(textTypeIDs.Count, (skinData.colors.Count - textTypeIDs.Count));
        }
        else if(skinData.colors.Count < textTypeIDs.Count)
        {
            while (skinData.colors.Count < textTypeIDs.Count) skinData.colors.Add(new Color());
        }
    }

    public void SetSavePath()
    {
        string path = Application.persistentDataPath + "/Saves/SkinData";
        if (Directory.Exists(path))
        {
            savePath = path;
        }
        else
        {
            Directory.CreateDirectory(path);
            savePath = path;
        }
    }

    public void SaveSkinEdits()
    {
        SetSavePath();
        string newPath = savePath + "/" + editing.skinName + ".json";

        SetTexturesFromEdit(editing);

        string jsonSave = JsonUtility.ToJson(editing);
        File.WriteAllText(newPath, jsonSave);
    }

    public void GetSaves()
    {
        if (savePath == "") SetSavePath();
        SkinData[] skinDatas = GetFromJsons(savePath);

        foreach(SkinData data in skinDatas)
        {
            AddSkin(data);
        }
    }

    public SkinData[] GetFromJsons(string path)
    {
        string[] files = GetFiles(path, "*.json");
        SkinData[] objs = new SkinData[files.Length];

        for (int i = 0; i < files.Length; i++)
        {
            objs[i] = JsonUtility.FromJson<SkinData>(files[i]);
        }

        return objs;
    }

    public string[] GetFiles(string path, string extentionWithJoker)
    {
        DirectoryInfo directory = new DirectoryInfo(path);
        string[] files = Directory.GetFiles(path, extentionWithJoker);
        return files;
    }
    
    public void SetTexturesFromEdit(SkinData skinData)
    {
        skinData.panel = Serialize(panelSprite.texture);
        skinData.button = Serialize(buttonSprite.texture);
        skinData.technicalButton = Serialize(technicalButtonSprite.texture);
        skinData.header = Serialize(headerSprite.texture);
        skinData.element = Serialize(elementSprite.texture);
    }

    public Texture2D Deserialize(SerializeTexture texture)
    {
        Texture2D newTexture = new Texture2D(texture.x, texture.y);
        ImageConversion.LoadImage(newTexture, texture.bytes);
        return newTexture;
    }

    public SerializeTexture Serialize(Texture2D texture)
    {
        SerializeTexture serial = new SerializeTexture();
        serial.x = texture.width;
        serial.y = texture.height;
        serial.bytes = ImageConversion.EncodeToPNG(texture);

        return serial;
    }

    public void SetSprites()
    {
        List<Sprite> sprites = GetSprites();
        List<Texture2D> editTextures = GetTexture2Ds(editing);

        for(int i = 0; i < sprites.Count; i++)
        {
            sprites[i] = Sprite.Create(editTextures[i], new Rect(), new Vector2(0.5f, 0.5f));
        }
    }

    public List<Sprite> GetSprites()
    {
        return new List<Sprite>() { panelSprite, buttonSprite, technicalButtonSprite, elementSprite, headerSprite};
    }

    public List<SerializeTexture> GetSerialTextures(SkinData data)
    {
        return new List<SerializeTexture>() { data.panel, data.button, data.technicalButton, data.element, data.header };
    }

    public List<Texture2D> GetTexture2Ds(SkinData data)
    {
        List<Texture2D> textures = new List<Texture2D>();

        foreach(SerializeTexture tex in GetSerialTextures(data))
        {
            textures.Add(Deserialize(tex));
        }

        return textures;
    }
}

[System.Serializable]
public class SkinManagerData
{
    public SkinData currentSkin;
}

[System.Serializable]
public class SerializeTexture
{
    public int x;
    public int y;
    public byte[] bytes;
}