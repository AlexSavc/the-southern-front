using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class SkinSystem : MonoBehaviour
{
    private static SkinSystem _instance;
    public static SkinSystem Instance
    {
        get { return _instance; }
    }

    public List<Sprite> sprites;

    public Dictionary<string, Sprite> spriteDictionary;

    void Awake()
    {
        _instance = this;
    }

    public Sprite GetSprite(string type)
    {
        if (spriteDictionary.ContainsKey(type))
        {
            return spriteDictionary[type];
        }
        else return null;
    }

    public List<string> GetTypes()
    {
        List<string> names = new List<string>();
        foreach(Sprite sprite in sprites)
        {
            //string path = AssetDatabase.GetAssetPath(sprite.GetInstanceID());
            //names.Add(Path.GetFileNameWithoutExtension(path));
        }
        return names;
    }
}
