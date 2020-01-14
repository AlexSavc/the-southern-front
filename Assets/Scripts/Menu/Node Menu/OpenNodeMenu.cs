using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenNodeMenu : MonoBehaviour
{
    private static OpenNodeMenu _instance;
    public static OpenNodeMenu Instance { get { return _instance; } }
    [SerializeField]
    private GameObject defaultMenu;

    [SerializeField]
    private GameObject nodeGarrissonMenu;
    [SerializeField]
    private GameObject nodeRecruitMenu;
    [SerializeField]
    private GameObject BuildMenu;
    [SerializeField]
    private Node selected;

    List<GameObject> allMenus;

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        allMenus = new List<GameObject>() { defaultMenu, nodeGarrissonMenu, nodeRecruitMenu, BuildMenu };
        DeactivateAll();
    }

    public void GoBack()
    {
        defaultMenu.SetActive(true);
        DeactivateAll();
        gameObject.SetActive(false);
    }

    public void OpenGarrisson()
    {
        DeactivateAll();
        nodeGarrissonMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OpenRecruitMenu()
    {
        DeactivateAll();
        nodeRecruitMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OpenBuildMenu()
    {
        DeactivateAll();
        BuildMenu.SetActive(true);
        BuildMenu.GetComponent<BuildMenu>().OpenBuildMenu(selected);
        gameObject.SetActive(false);
    }

    public void SetNode(Node node)
    {
        selected = node;
    }

    public void DeactivateAll()
    {
        Utility.SetActiveAll(allMenus, false);
    }
}
