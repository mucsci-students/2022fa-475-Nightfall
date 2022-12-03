using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private static Dictionary<string, int> _items;

    private static Dictionary<string, string> _tools;
    
    public static void Initialize()
    {

        _items = new Dictionary<string, int>();
        _tools = new Dictionary<string, string>();

        _items.Add("Wood", 0);
        _items.Add("Planks", 0);
        _items.Add("Stone", 0);
        _items.Add("CutStone", 0);
        _items.Add("CopperOre", 0);
        _items.Add("CopperBar", 0);
        _items.Add("IronOre", 0);
        _items.Add("IronBar", 0);

        _tools.Add("sword", "Wood");
        _tools.Add("axe", "Wood");
        _tools.Add("pickaxe", "Wood");
        _tools.Add("torch", "Basic");
    }

    public static void AddItem(string n, int qty)
    {
        _items[n] += qty;
    }

    public static void AddItems(IReadOnlyCollection<KeyValuePair<string, int>> items)
    {

        foreach (var item in items) { AddItem(item.Key, item.Value); }

    }

    public static void RemoveItem(string n, int qty)
    {
        _items[n] -= qty;
    }

    public static void RemoveItems(IEnumerable<KeyValuePair<string, int>> toRemove)
    {

        foreach (var item in toRemove) { RemoveItem(item.Key, item.Value); }

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

    public static Dictionary<string, string> GetAllTools() => _tools;

    public static void SetTools(Dictionary<string, string> newTools) 
        => _tools = new Dictionary<string, string>(newTools);

}
