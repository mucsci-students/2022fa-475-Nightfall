using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private Dictionary<string, int> items = new Dictionary<string, int>();

    private Dictionary<string, string> tools = new Dictionary<string, string>();
    
    // Start is called before the first frame update
    void Start()
    {
        items.Add("Wood", 0);
        items.Add("Stone", 0);
        items.Add("Copper Ore", 0);
        items.Add("Iron Ore", 0);

        tools.Add("Sword", "Wood");
        tools.Add("Axe", "Wood");
        tools.Add("Pickaxe", "Wood");
        tools.Add("Torch", "Basic");
    }

    public void AddItem(string n, int qty)
    {
        items[n] += qty;
    }

    public void RemoveItem(string n, int qty)
    {
        items[n] -= qty;
    }

    public int GetCount(string n)
    {
        return items[n];
    }

    public void ChangeTool(string type, string n)
    {
        tools[type] = n;
    }

    public string GetTool(string type)
    {
        return tools[type];
    }

    public Dictionary<string, int> GetAllItems()
    {
        return new Dictionary<string, int>(items);
    }
}
