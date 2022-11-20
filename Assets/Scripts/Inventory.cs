using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private Dictionary<string, int> _items = new Dictionary<string, int>();

    private Dictionary<string, string> _tools = new Dictionary<string, string>();
    
    public void Initialize()
    {
        _items.Add("Wood", 0);
        _items.Add("Stone", 0);
        _items.Add("Copper Ore", 0);
        _items.Add("Iron Ore", 0);

        _tools.Add("Sword", "Wood");
        _tools.Add("Axe", "Wood");
        _tools.Add("Pickaxe", "Wood");
        _tools.Add("Torch", "Basic");
    }

    public void AddItem(string n, int qty)
    {
        _items[n] += qty;
    }

    public void RemoveItem(string n, int qty)
    {
        _items[n] -= qty;
    }

    public int GetCount(string n)
    {
        return _items[n];
    }

    public void ChangeTool(string type, string n)
    {
        _tools[type] = n;
    }

    public string GetTool(string type)
    {
        return _tools[type];
    }

    public Dictionary<string, int> GetAllItems()
    {
        return new Dictionary<string, int>(_items);
    }
}
