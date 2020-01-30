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
    private GameObject revenueContent, expensesContent; // Scrollrect content object
    [SerializeField]
    private UIElement elementTemplate; // Custom UI

    [SerializeField]
    private TextMeshProUGUI nextTurnRevenue, nextTurnExpense;
    
    public void Refresh()
    {
        DisplayBreakdown(TurnManager.Instance.currentPlayer);
    }

    /// <summary>
    /// Shows the breakdown of expenses and revenues on the EconomyMenu
    /// </summary>
    /// <param name="player"></param>
    public void DisplayBreakdown(Player player)
    {
        List<BuyableInfo> revenue, expenses;
        GetRevenueAndExpenses(player, out revenue, out expenses);
        List<BuyableDisplay> rev = GetBuyableDisplay(revenue), spend = GetBuyableDisplay(expenses);
        ClearContents();
        PopulateContents(rev, spend);
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
            display.Add(new BuyableDisplay(info.type, info.revenue - info.maintenance, info.thumbnail));
        }

        return display;
    }

    private void ClearContents()
    {
        Utility.ClearChildren(revenueContent);
        Utility.ClearChildren(expensesContent);
    }

    private void PopulateContents(List<BuyableDisplay> revenue, List<BuyableDisplay> expenses)
    {
        Dictionary<string, int> revAssetOccurence = GetOccurenceFrequency(revenue);
        Dictionary<string, int> expAssetOccurence = GetOccurenceFrequency(expenses);

        float totalRev, totalExp;

        PopulateContent(revenueContent, revAssetOccurence, revenue, out totalRev);
        PopulateContent(expensesContent, expAssetOccurence, expenses, out totalExp);

        SetExpectedTotals(totalRev, totalExp);
    }

    private void PopulateContent(GameObject content, Dictionary<string, int> occurence, List<BuyableDisplay> list, out float total)
    {
        total = 0;
        foreach (KeyValuePair<string, int> entry in occurence)
        {
            BuyableDisplay buyable = list.Find(element => element.name == entry.Key);
            AddChildElement(content, buyable, entry.Value);
            total += buyable.netProfit * entry.Value;
        }
    }

    private void AddChildElement(GameObject content, BuyableDisplay buyable, int frequency)
    {
        UIElement element = Instantiate(elementTemplate);
        element.transform.SetParent(content.transform);
        
        element.SetImage(buyable.thumbnail);
        element.SetTitle(buyable.name + " (" + frequency + ")");
        element.SetParagraph(buyable.netProfit + " ( Total: " + buyable.netProfit * frequency + ")");
    }

    private Dictionary<string, int> GetOccurenceFrequency(List<BuyableDisplay> list)
    {
        Dictionary<string, int> occurence = new Dictionary<string, int>();
        
        foreach (BuyableDisplay buyable in list)
        {
            if (!occurence.ContainsKey(buyable.name))
            {
                occurence.Add(buyable.name, list.FindAll(element => element.name == buyable.name).Count);
            }
        }

        return occurence;
    }
    
    private void SetExpectedTotals(float revenue, float expenses)
    {
        nextTurnRevenue.text = revenue.ToString();
        nextTurnExpense.text = expenses.ToString();
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