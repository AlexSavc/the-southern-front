using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Economy : MonoBehaviour
{
    [SerializeField]
    private Player[] players;

    [SerializeField]
    private Player currentPlayer;

    [SerializeField]
    private TextMeshProUGUI nextTurnGold;
    [SerializeField]
    private TextMeshProUGUI currentGoldDisplay;

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
        SetCurrentGold(player.CurrentGold()); // Add Animation
        SetNextTurnGold(GetProjectedBudget(player));
    }


    public void CollectTaxes(Player player)
    {
        foreach(Node node in player.GetNodesOwned())
        {
            node.PayTaxes();
        }
    }

    public void SetNextTurnGold(int amount)
    {
        nextTurnGold.text = amount.ToString();
    }

    public void SetCurrentGold(int amount)
    {
        currentGoldDisplay.text = amount.ToString();
    }

    public int GetProjectedBudget(Player player)
    {
        return GetProjectedTaxes(player) - GetProjectedExpenses(player);
    }

    public int GetProjectedTaxes(Player player)
    {
        int tax = 0;
        foreach(Node node in player.GetNodesOwned())
        {
            tax += node.GoldPerTurn();
        }
        return tax;
    }

    public int GetProjectedExpenses(Player player)
    {
        return 0;
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

    }

    public void RemoveGold(int amount)
    {
        currentPlayer.RemoveGold(amount);
        SetCurrentGold(currentPlayer.CurrentGold());
    }
}

[System.Serializable]
public class BuyableInfo
{
    public int price;
    public int maintenance;
    public int maintenanceFrequency;
    public int recupOnDisband;

    public BuyableInfo(int Price, int Maintenance, int MaintenanceFrequency, int RecupOnDisband)
    {
        price = Price;
        maintenance = Maintenance;
        maintenanceFrequency = MaintenanceFrequency;
        recupOnDisband = RecupOnDisband;
    }

    public BuyableInfo(BuyableInfo template)
    {
        price = template.price;
        maintenance = template.maintenance;
        maintenanceFrequency = template.maintenanceFrequency;
        recupOnDisband = template.recupOnDisband;
    }
}
