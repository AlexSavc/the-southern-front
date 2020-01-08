using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenNodeMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject defaultMenu;

    [SerializeField]
    private GameObject nodeGarrissonMenu;
    [SerializeField]
    private GameObject nodeRecruitMenu;
    [SerializeField]
    private GameObject nodeUpgradeMenu;

    List<GameObject> allMenus;

    void Start()
    {
        allMenus = new List<GameObject>() { defaultMenu, nodeGarrissonMenu, nodeRecruitMenu, nodeUpgradeMenu };
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

    public void OpenUpgradeMenu()
    {
        DeactivateAll();
        nodeUpgradeMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void DeactivateAll()
    {
        Utility.SetActiveAll(allMenus, false);
    }
}
