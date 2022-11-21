using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolData
{
    private Dictionary<string, Dictionary<string, Dictionary<string, int>>> _tools = new Dictionary<string, Dictionary<string, Dictionary<string, int>>>();

    public void Initialize()
    {
        Dictionary<string, Dictionary<string, int>> swords = new Dictionary<string, Dictionary<string, int>>();
        Dictionary<string, Dictionary<string, int>> axes = new Dictionary<string, Dictionary<string, int>>();
        Dictionary<string, Dictionary<string, int>> pickaxes = new Dictionary<string, Dictionary<string, int>>();
        Dictionary<string, Dictionary<string, int>> torches = new Dictionary<string, Dictionary<string, int>>();

        Dictionary<string, int> temp = new Dictionary<string, int>();
        temp.Add("speed", 5);
        temp.Add("stamina", 10);
        swords.Add("Wood", temp);

        temp = new Dictionary<string, int>();
        temp.Add("speed", 5);
        temp.Add("stamina", 8);
        axes.Add("Wood", temp);

        temp = new Dictionary<string, int>();
        temp.Add("speed", 5);
        temp.Add("stamina", 8);
        pickaxes.Add("Wood", temp);

        temp = new Dictionary<string, int>();
        temp.Add("brightness", 5);
        torches.Add("Basic", temp);

        _tools.Add("swords", swords);
        _tools.Add("axes", axes);
        _tools.Add("pickaxes", pickaxes);
        _tools.Add("torches", torches);
    }
    
    public int GetValue(string type, string quality, string field)
    {
        return _tools[type][quality][field];
    }
}
