using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    static SceneChanger Instance;
    public Map map;
    public MapCreateInfo myInfo;

    void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
    }

    public void CreateMap(string path)
    {
        SceneManager.LoadScene("Game");

        StartCoroutine(Create(path));
    }

    public void LoadMap(string path)
    {
        SceneManager.LoadScene("Game");

        StartCoroutine(LoadEnumerator(path));
    }

    IEnumerator Create(string path)
    {
        yield return new WaitForSeconds(0.1f);
        map = FindObjectOfType<Map>();
        if (map != null)
            map.GenerateMap(path);
        else Debug.Log("Map null");

    }

    IEnumerator LoadEnumerator(string path)
    {
        yield return new WaitForSeconds(0.1f);
        map = FindObjectOfType<Map>();
        if (map != null)
        {
            map.LoadMap(path);
        }
        else Debug.Log("Map null");

    }
}
