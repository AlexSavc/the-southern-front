﻿using System.Collections;
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

    [SerializeField]
    private TextMeshProUGUI nextTurnGold;
    [SerializeField]
    private TextMeshProUGUI currentGoldDisplay;
    [SerializeField]
    private UnityEngine.UI.Slider percentSlider;

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
        SetCurrentGold(player.CurrentGold()); // Add Animation
        SetNextTurnGold(GetProjectedBudget(player));
        SetPercentSlider(currentPlayer.PercentOfPoint);
    }
    
    public void CollectTaxes(Player player)
    {
        player.CollectTaxes();
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
        percentSlider.value = percent;
    }

    public int GetProjectedBudget(Player player)
    {
        return GetProjectedTaxes(player) - GetProjectedExpenses(player);
    }

    public int GetProjectedTaxes(Player player)
    {
        return player.GetProjectedTaxes();
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

    public void AddGold(int amount)
    {
        currentPlayer.AddGold(amount);
        SetCurrentGold(currentPlayer.CurrentGold());
    }
}

[System.Serializable]
public class BuyableInfo
{
    public int price;
    public float maintenance;
    public int recupOnDisband;
    public float revenue;

    public BuyableInfo(int Price, float Maintenance, int RecupOnDisband, float Revenue)
    {
        price = Price;
        maintenance = Maintenance;
        recupOnDisband = RecupOnDisband;
        revenue = Revenue;
    }

    public BuyableInfo(BuyableInfo template)
    {
        price = template.price;
        maintenance = template.maintenance;
        recupOnDisband = template.recupOnDisband;
        revenue = template.revenue;
    }
}
