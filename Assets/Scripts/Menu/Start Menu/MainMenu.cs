using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject playMenu;

    public void Start()
    {
        playMenu.GetComponent<PlayMenu>().mainMenu = gameObject;
        playMenu.SetActive(true);
        playMenu.SetActive(false);
    }

    public void OnPressPlay()
    {
        playMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnPressQuit()
    {
        Application.Quit();
    }
}