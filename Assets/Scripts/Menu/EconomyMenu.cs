using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EconomyMenu : MonoBehaviour
{
    private static EconomyMenu _instance;
    public static EconomyMenu Instance { get { return _instance; } }
    void Awake() { _instance = this; }

    [SerializeField]
    private GameObject revenueContent, expensesContent;
    [SerializeField]
    private UIElement elementTemplate;
    [SerializeField]
    private UIHeader headerTemplate;

    [SerializeField]
    private TextMeshProUGUI nextTurnRevenue, nextTurnExpense;

    public void DisplayBreakdown(Player player)
    {
        List<BuyableInfo> revenue, expenses;
        GetRevenueAndExpenses(player, out revenue, out expenses);
        List<BuyableDisplay> rev = GetBuyableDisplay(revenue), spend = GetBuyableDisplay(expenses);
    }
    
    private void ClearContents()
    {
        Utility.ClearChildren(revenueContent);
        Utility.ClearChildren(expensesContent);
    }

    /// <summary>
    /// Sorts player.assets into net profit and net loss, and returns them as two lists
    /// </summary>
    /// <param name="player"></param>
    /// <param name="Revenue"></param>
    /// <param name="Expenses"></param>
    private void GetRevenueAndExpenses(Player player, out List<BuyableInfo> Revenue, out List<BuyableInfo> Expenses)
    {
        List<BuyableInfo> revenue = new List<BuyableInfo>(), expenses = new List<BuyableInfo>();

        foreach(BuyableInfo info in player.Assets)
        {
            if (info.revenue - info.maintenance > 0) revenue.Add(info);
            else expenses.Add(info);
        }

        Revenue = revenue;
        Expenses = expenses;
    }

    private List<BuyableDisplay> GetBuyableDisplay(List<BuyableInfo> list)
    {
        List<BuyableDisplay> display = new List<BuyableDisplay>();

        foreach(BuyableInfo info in list)
        {
            display.Add(new BuyableDisplay("buyable", info.revenue - info.maintenance, null));
        }

        return display;
    }

    private struct BuyableDisplay
    {
        public string name;
        public float netProfit;
        public Sprite thumbnail;

        public BuyableDisplay(string Name, float NetProfit, Sprite Thumbnail)
        {
            name = Name;
            netProfit = NetProfit;
            thumbnail = Thumbnail;
        }
    }
}