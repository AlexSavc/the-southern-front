using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Economy : MonoBehaviour
{
    private static Economy _instance;
    public static Economy Instance { get { return _instance; } }

    [SerializeField]
    private Player[] players;

    [SerializeField]
    private Player currentPlayer;
    public Player CurrentPlayer { get { return currentPlayer; } }
    
    [SerializeField]
    private int starterGold = 10;
    public int StarterGold { get { return starterGold; } }

    [SerializeField]
    private TextMeshProUGUI nextTurnGold;
    [SerializeField]
    private TextMeshProUGUI currentGoldDisplay;
    [SerializeField]
    private UnityEngine.UI.Slider posSlider;
    [SerializeField]
    private UnityEngine.UI.Slider negSlider;

    void Awake()
    {
        _instance = this;
    }

    public void SetPlayers(Player[] set)
    {
        players = set;
    }

    public void OnTurnEnd(Player player)
    {
        CollectTaxes(player);
    }

    public void OnTurnStart(Player player)
    {
        currentPlayer = player;
        Refresh();
    }

    public void Refresh()
    {
        SetCurrentGold(currentPlayer.CurrentGold()); // Add Animation
        float budget = GetProjectedBudget(currentPlayer);
        float dec = Utility.GetDecimalPart(budget);

        SetNextTurnGold(Utility.RoundDownInt(budget));
        SetPercentSlider(Utility.GetDecimalPart(budget));
    }
    
    public void CollectTaxes(Player player)
    {
        float budget = GetProjectedBudget(currentPlayer);
        float dec = Utility.GetDecimalPart(budget);

        
        budget = (Utility.RoundDownInt(budget));

        player.AddGold((int)budget);
    }

    public void SetNextTurnGold(int amount)
    {
        nextTurnGold.text = amount.ToString();
    }

    public void SetCurrentGold(int amount)
    {
        currentGoldDisplay.text = amount.ToString();
    }

    public void SetPercentSlider(float percent)
    {
        if(percent < 0)
        {
            posSlider.gameObject.SetActive(false);
            negSlider.gameObject.SetActive(true);
            negSlider.value = (percent*-1);
        }
        else
        {
            negSlider.gameObject.SetActive(false);
            posSlider.gameObject.SetActive(true);
            posSlider.value = percent;
        }
    }

    public float GetProjectedBudget(Player player)
    {
        return player.GetProjectedTaxes() - player.GetProjectedExpenses();
    }
    
    void CollectAllTaxes()
    {
        //THERE IS NO REASON TO DO THIS, until i do the RTS
        foreach(Player player in players)
        {
            CollectTaxes(player);
        }
    }

    public void OnBuy(BuyableInfo info, out bool success)
    {
        PopupHandler popupHandler = FindObjectOfType<PopupHandler>();
        if(currentPlayer.CurrentGold() < info.price)
        {
            popupHandler.OnInfoPupup(new InfoPopupInfo() { infoText = "You don't have enough money", okText = "Ok" });
            success = false;
            return;
        }
        success = true;
        RemoveGold(info.price);
        StartCoroutine(RefreshDelay());
    }

    public void RemoveGold(int amount)
    {
        currentPlayer.RemoveGold(amount);
        SetCurrentGold(currentPlayer.CurrentGold());
    }

    public void AddGold(int amount)
    {
        currentPlayer.AddGold(amount);
        SetCurrentGold(currentPlayer.CurrentGold());
    }

    public IEnumerator RefreshDelay()
    {
        yield return new WaitForSeconds(0.1f);
        Refresh();
        yield return null;
    }
}

[System.Serializable]
public class BuyableInfo
{
    public int price;
    public float maintenance;
    public int recupOnDisband;
    public float revenue;
    public string type;

    public BuyableInfo(int Price, float Maintenance, int RecupOnDisband, float Revenue, string Type)
    {
        price = Price;
        maintenance = Maintenance;
        recupOnDisband = RecupOnDisband;
        revenue = Revenue;
        type = Type;
    }

    public BuyableInfo(BuyableInfo template)
    {
        price = template.price;
        maintenance = template.maintenance;
        recupOnDisband = template.recupOnDisband;
        revenue = template.revenue;
        type = template.type;
    }
}