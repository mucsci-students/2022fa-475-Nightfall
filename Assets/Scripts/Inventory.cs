using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private static Dictionary<string, int> _items = new Dictionary<string, int>();

    private static Dictionary<string, string> _tools = new Dictionary<string, string>();
    
    public static void Initialize()
    {
        _items.Add("Wood", 0);
        _items.Add("Stone", 0);
        _items.Add("Copper Ore", 0);
        _items.Add("Iron Ore", 0);

        _tools.Add("sword", "Wood");
        _tools.Add("axe", "Wood");
        _tools.Add("pickaxe", "Wood");
        _tools.Add("torch", "Basic");
    }

    public static void AddItem(string n, int qty)
    {
        _items[n] += qty;
    }

    public static void RemoveItem(string n, int qty)
    {
        _items[n] -= qty;
    }

    public static int GetCount(string n)
    {
        return _items[n];
    }

    public static void ChangeTool(string type, string n)
    {
        _tools[type] = n;
    }

    public static string GetTool(string type)
    {
        return _tools[type];
    }

    public static Dictionary<string, int> GetAllItems()
    {
        return new Dictionary<string, int>(_items);
    }
}
