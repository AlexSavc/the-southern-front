using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public Map map;
    public GameObject pauseMenuUI;

    public List<GameObject> UiActiveOnPause;

    void Awake()
    {
        map = FindObjectOfType<Map>();
    }

    /*void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }*/

    public void Pause()
    {
        
        Time.timeScale = 0f;
        GameIsPaused = true;
        pauseMenuUI.SetActive(true);
        DisableActiveSiblings();
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        ReEnableActiveSiblings();
    }

    public void LoadSettings()
    {

    }

    public void Save()
    {
        map.SaveMap();
        Resume();
    }

    public void SaveAndQuit()
    {
        map.SaveMap();
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    
    

    public void DisableActiveSiblings()
    {
        UiActiveOnPause = Utility.GetActiveSiblings(pauseMenuUI);
        foreach (GameObject obj in UiActiveOnPause)
        {
            obj.SetActive(false);
        }
    }

    public void ReEnableActiveSiblings()
    {
        foreach (GameObject obj in UiActiveOnPause)
        {
            obj.SetActive(true);
        }
    }
}
