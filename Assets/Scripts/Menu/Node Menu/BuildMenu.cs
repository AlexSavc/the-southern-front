using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMenu : MonoBehaviour
{
    private static BuildMenu _instance;
    public static BuildMenu Instance  { get { return _instance; } }
    [SerializeField]
    private GameObject roundButtonPrefab;
    [SerializeField]
    private GameObject content;
    [SerializeField]
    private Node targetNode;

    List<GameObject> buttonObjects;

    void Awake()
    {
        buttonObjects = new List<GameObject>();
        _instance = this;
        if (roundButtonPrefab.GetComponentInChildren<RoundButton>() == false) throw new System.Exception("Round Button prefab has no RoundButton Component");
    }

    public void OpenBuildMenu(Node node)
    {
        targetNode = node;
        RefreshButtons();
    }

    public void RefreshButtons()
    {
        ClearButtons();
        SetButtons();
    }

    public void SetButtons()
    {
        if (targetNode == null) { throw new System.Exception("Node null"); }
        Builder builder = targetNode.gameObject.GetComponent<Builder>();
        if (builder == null) { throw new System.Exception("Node has no Builder"); }

        List<RoundButtonData> buttons = builder.GetBuildableData();

        foreach (RoundButtonData data in buttons)
        {
            GameObject button = Instantiate(roundButtonPrefab, content.transform);
            button.GetComponentInChildren<RoundButton>().SetButton(data);
            buttonObjects.Add(button);
        }
    }

    private void ClearButtons()
    {
        foreach(GameObject obj in buttonObjects)
        {
            Destroy(obj);
        }

        buttonObjects.Clear();
    }

    void OnDisable()
    {
        ClearButtons();
    }
}