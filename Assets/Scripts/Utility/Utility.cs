using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Utility
{
    public static List<GameObject> GetSiblings(GameObject obj)
    {
        List<GameObject> objs = new List<GameObject>();
        for (int i = 0; i < obj.transform.parent.childCount; i++)
        {
            objs.Add(obj.transform.parent.GetChild(i).gameObject);
        }
        return objs;
    }

    public static List<GameObject> GetChildren(Transform form)
    {
        List<GameObject> objs = new List<GameObject>();
        for (int i = 0; i < form.childCount; i++)
        {
            objs.Add(form.GetChild(i).gameObject);
        }
        return objs;
    }

    public static void ClearChildren(GameObject target)
    {
        GameObject[] children = GetChildren(target.transform).ToArray();

        foreach (GameObject child in children)
        {
            Object.Destroy(child);
        }
    }

    public static List<GameObject> GetActiveSiblings(GameObject Obj)
    {
        List<GameObject> temp = new List<GameObject>();
        foreach (GameObject obj in Utility.GetSiblings(Obj))
        {
            if (obj != Obj && obj.activeInHierarchy == true)
            {
                temp.Add(obj);
            }
        }

        return temp;
    }

    public static void SetActiveAll(List<GameObject> toDeactivate, bool active)
    {
        if (toDeactivate == null) return;
        foreach (GameObject obj in toDeactivate)
        {
            obj.SetActive(active);
        }
    }

    public static void SetActiveAll(GameObject[] toDeactivate, bool active)
    {
        foreach (GameObject obj in toDeactivate)
        {
            obj.SetActive(active);
        }
    }

    public static void CheckDuplicates(List<object> toCheck)
    {
        List<object> objectList = new List<object>();

        foreach (object obj in toCheck)
        {
            if (objectList.Contains(obj) == false) objectList.Add(obj);
        }
    }

    public static float BrownianMotion(int x, int y, BrownianMotionData data)
    {

        //for each pixel, get the value total = 0.0f; frequency = 1.0f/(float)hgrid; amplitude = gain;

        float total = 0;

        for (int i = 0; i < data.octaves; ++i)
        {
            total += Mathf.PerlinNoise((float)x * data.frequency, (float)y * data.frequency) * data.amplitude;
            data.frequency *= data.lacunarity;
            data.amplitude *= data.gain;
        }

        Debug.Log(total);
        //now that we have the value, put it in map[x][y]=total;
        
        return total;
    }
    
    public static int CoordsToArrayPos(int x, int y, int sizeY)
    {
        return x * sizeY + y;
    }

    public static bool IsPonterOverUIObject()
    {
        PointerEventData eventDataMousePos = new PointerEventData(EventSystem.current);
        Vector2 mousePos = Input.mousePosition;
        eventDataMousePos.position = new Vector2(mousePos.x, mousePos.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataMousePos, results);

        if (results.Count > 0) return true;
        else return false;
    }

    public static GameObject GetFirstParentWithComponent(GameObject origin, System.Type type)
    {
        int arbitraryCutoff = 1000;
        Transform parent = origin.transform.parent;
        int go = 0;
        while (go < arbitraryCutoff)
        {
            go++;
            if (parent.GetComponent(type))
            {
                return parent.gameObject;
            }
            else if (parent.parent == null) return null;
            else parent = parent.parent;
        }

        return null;
    }

    public static object GetFirstComponentInParents(GameObject origin, System.Type type)
    {
        int arbitraryCutoff = 1000;
        Transform parent = origin.transform.parent;
        int go = 0;
        while (go < arbitraryCutoff)
        {
            go++;
            if (parent.GetComponent(type))
            {
                return parent.GetComponent(type);
            }
            else if (parent.parent == null) return null;
            else parent = parent.parent;
        }

        return null;
    }

    public static void SaveToJson(object toSave, string path)
    {
        string jsonSave = JsonUtility.ToJson(toSave);
        System.IO.File.WriteAllText(path, jsonSave);
    }

    public static object GetFromJson(System.Type type, string path)
    {
        object obj = JsonUtility.FromJson(System.IO.File.ReadAllText(path), type);
        return obj;
    }

    public static string[] GetFiles(string path, string extentionWithJoker)
    {
        System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(path);
        string[] files = System.IO.Directory.GetFiles(path, extentionWithJoker);
        return files;
    }

    public static object[] GetFromJsons(System.Type type, string path)
    {
        string[] files = GetFiles(path, "*.json");
        object[] objs = new object[files.Length];

        for(int i = 0; i < files.Length; i++)
        {
            objs[i] = JsonUtility.FromJson(files[i], type);
        }

        return objs;
    }

    public static int RoundUpInt(float toRound)
    {
        float nearest = Mathf.Round(toRound);
        float f = toRound - nearest;

        int r = Mathf.RoundToInt(toRound - (f));
        return r;
    }

    public static int RoundDownInt(float toRound)
    {
        float remainder = GetDecimalPart(toRound);
        float f = toRound - remainder;
        return (int)f;
    }

    public static float GetDecimalPart(float f)
    {
        return f % 1;
    }
}

[System.Serializable]
public class BrownianMotionData
{
    public BrownianMotionData(float Octaves, float Frequency, float Amplitude, float Gain, float Lacunarity)
    {
        octaves = Octaves;
        frequency = Frequency;
        amplitude = Amplitude;
        gain = Gain;
        lacunarity = Lacunarity;
    }
    public float octaves;
    public float frequency;
    public float amplitude;
    public float gain;
    public float lacunarity;
}

