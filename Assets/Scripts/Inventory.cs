using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private static GameObject _weaponHolder;

    private static List<Material> _materials;

    private static Dictionary<string, int> _items;

    private static Dictionary<string, string> _tools;

    private static Dictionary<string, int> _maxCapacities;
    
    public static void Initialize()
    {   
        _items = new Dictionary<string, int>();
        _tools = new Dictionary<string, string>();
        _materials = new List<Material>();
        _maxCapacities = new Dictionary<string, int>();

        _items.Add("Wood", 0);
        _items.Add("Planks", 0);
        _items.Add("Stone", 0);
        _items.Add("CutStone", 0);
        _items.Add("Copper Ore", 0);
        _items.Add("Copper Bar", 0);
        _items.Add("Iron Ore", 0);
        _items.Add("Iron Bar", 0);

        _maxCapacities.Add("Wood", 50);
        _maxCapacities.Add("Planks", 50);
        _maxCapacities.Add("Stone", 50);
        _maxCapacities.Add("CutStone", 50);
        _maxCapacities.Add("Copper Ore", 50);
        _maxCapacities.Add("Copper Bar", 50);
        _maxCapacities.Add("Iron Ore", 50);
        _maxCapacities.Add("Iron Bar", 50);

        _tools.Add("sword", "Wood");
        _tools.Add("axe", "Wood");
        _tools.Add("pickaxe", "Wood");
        _tools.Add("torch", "Basic");
        
        _materials.Add(Resources.Load<Material>("Tool_Handle"));    // 0
        _materials.Add(Resources.Load<Material>("Stone_Tool"));     // 1
        _materials.Add(Resources.Load<Material>("Copper_Tool"));    // 2
        _materials.Add(Resources.Load<Material>("Iron_Tool"));      // 3
        _materials.Add(Resources.Load<Material>("Cutting_Edge"));   // 4
        _materials.Add(Resources.Load<Material>("Copper_Sword"));   // 5

    }

    public static void AddItem(string n, int qty)
    {        
        _items[n] = Mathf.Min(_maxCapacities[n], _items[n] + qty);
        UIManager.UpdateCount(n);
    }

    public static void AddItems(IReadOnlyCollection<KeyValuePair<string, int>> items)
    {
        foreach (var item in items) { AddItem(item.Key, item.Value); }
    }

    public static void RemoveItem(string n, int qty)
    {
        if (_items[n] - qty < 0) { Debug.Log("Not enough quantity!"); return; }
        _items[n] -= qty;
        UIManager.UpdateCount(n);
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
        ApplyToolMaterial(type, n);
    }

    private static void ApplyToolMaterial(string toolType, string toolVariation)
    {

        _weaponHolder = GameObject.Find("WeaponHolder");
        MeshRenderer swordMesh = _weaponHolder.transform.GetChild(0).GetComponent<MeshRenderer>();
        MeshRenderer hatchetMesh = _weaponHolder.transform.GetChild(1).GetComponent<MeshRenderer>();
        MeshRenderer pickaxeMesh = _weaponHolder.transform.GetChild(2).GetComponent<MeshRenderer>();

        /*
            _materials[0] - Tool Handle Material
            _materials[1] - Stone Material
            _materials[2] - Copper Material
            _materials[3] - Iron Material
            _materials[4] - Hatchet edge Material
            _materials[5] - Copper Sword Material
        */

        if (toolType == "sword")
        {
            Material[] oldMater /*lmfao*/ = swordMesh.materials;

            if (toolVariation == "Stone")
            {
                oldMater[0] = _materials[1];
                swordMesh.materials = oldMater;
            }
            else if (toolVariation == "Copper")
            {
                oldMater[0] = _materials[5];
                swordMesh.materials = oldMater;
            }
            else if (toolVariation == "Iron")
            {
                oldMater[0] = _materials[3];
                swordMesh.materials = oldMater;
            }
            else
            {
                print("Invalid material type.");
            }

        }
        /*
            axe: oldMater[0] - Handle
            axe: oldMater[1] - Head
            axe: oldMater[2] - Cutting Edge
        */

        if (toolType == "axe")
        {
            Material[] oldMater = hatchetMesh.materials;

            if (toolVariation == "Stone")
            {
                oldMater[1] = _materials[1];
                oldMater[2] = _materials[4];
                hatchetMesh.materials = oldMater;
            }
            else if (toolVariation == "Copper")
            {
                oldMater[1] = _materials[2];
                hatchetMesh.materials = oldMater;
            }
            else if (toolVariation == "Iron")
            {
                oldMater[1] = _materials[3];
                hatchetMesh.materials = oldMater;
            }
            else
            {
                print("Invalid material type.");
            }
        }

        /*
            pickaxe: oldMater[0] - Handle
            pickaxe: oldMater[1] - Tips
            pickaxe: oldMater[2] - Head
        */

        else if (toolType == "pickaxe")
        {
            Material[] oldMater = pickaxeMesh.materials;

            if (toolVariation == "Stone")
            {
                oldMater[1] = _materials[1];
                oldMater[2] = _materials[1];
                pickaxeMesh.materials = oldMater;
            }
            else if (toolVariation == "Copper")
            {
                oldMater[1] = _materials[2];
                oldMater[2] = _materials[2];
                pickaxeMesh.materials = oldMater;
            }
            else if (toolVariation == "Iron")
            {
                oldMater[0] = _materials[2];
                oldMater[1] = _materials[4];
                oldMater[2] = _materials[3];
                pickaxeMesh.materials = oldMater;
            }
            else
            {
                print("Invalid material type.");
            }
        }

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
    {

        _tools = new Dictionary<string, string>(newTools);
        foreach(var tool in _tools) { ApplyToolMaterial(tool.Key, tool.Value); }

    }
    
}
