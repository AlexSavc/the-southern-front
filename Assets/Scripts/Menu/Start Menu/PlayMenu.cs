using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayMenu : MonoBehaviour
{
    public GameObject mainMenu;

    public CreationMenu creationMenu;
    public MapCreationUtility mapUtility;

    public int maxMaps = 10;

    public GameObject mapSlotPrefab;
    public GameObject mapSlotHolder;
    public GameObject newMapButtonPrefab;

    [SerializeField]
    private GameObject newMapButton;

    private bool mapLimitReached = false;

    void Awake()
    {
        if (mapUtility == null) mapUtility = FindObjectOfType<MapCreationUtility>();
        
        newMapButton.GetComponent<Button>().onClick.AddListener(delegate () { CreatNewMap(); });
    }

    public void OnPressBack()
    {
        mainMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnEnable()
    {
        Refresh();
    }

    void Refresh()
    {
        if (MapLimitIsReached())
        {
            SetMaxMapsReached();
        }
        else if (!MapLimitIsReached())
        {
            SetMaxMapsNotReached();
        }

        mapUtility.Refresh();
    }

    public void SetAddButtonAsLast()
    {
        if (newMapButton != null) newMapButton.transform.SetAsLastSibling();
    }

    void SetMaxMapsReached()
    {
        TextMeshProUGUI text;
        if (mapSlotHolder.transform.childCount > 0) text = newMapButton.GetComponentInChildren<TextMeshProUGUI>();
        else return;

        text.text = "Game Count Limit Reached";
        mapLimitReached = true;
        newMapButton.GetComponent<Button>().interactable = false;
    }

    void SetMaxMapsNotReached()
    {
        TextMeshProUGUI text;
        if (mapSlotHolder.transform.childCount > 0)
        {
            text = newMapButton.GetComponentInChildren<TextMeshProUGUI>();
        }
        else return;

        text.text = "Create New Game";
        text.enabled = true;
        mapLimitReached = false;
        newMapButton.GetComponent<Button>().interactable = true;
    }

    public void CreatNewMap()
    {
        GameObject mapSlotObj = Instantiate(mapSlotPrefab, mapSlotHolder.transform);

        MapSlot mapSlot = mapSlotObj.GetComponent<MapSlot>();

        creationMenu.gameObject.SetActive(true);
        creationMenu.StartCreation(mapSlot);
        gameObject.SetActive(false);
    }

    public void AddMapSlot(string path)
    {
        GameObject mapSlotObj = Instantiate(mapSlotPrefab, mapSlotHolder.transform);

        MapSlot mapSlot = mapSlotObj.GetComponent<MapSlot>();
        mapSlot.SetPath(path, true);
    }

    public void ClearMapsSlots()
    {
        MapSlot[] mapSlots = mapSlotHolder.transform.GetComponentsInChildren<MapSlot>();
        foreach(MapSlot slot in mapSlots)
        {
            Destroy(slot.gameObject);
        }
    }

    public bool MapLimitIsReached()
    {
        if (mapSlotHolder.transform.childCount > maxMaps) return true;
        else return false;
    }
}
