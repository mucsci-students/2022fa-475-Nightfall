using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolData
{
    private static Dictionary<string, Dictionary<string, Dictionary<string, int>>> _tools;

    public static void Initialize()
    {

        _tools = new Dictionary<string, Dictionary<string, Dictionary<string, int>>>()
        {
            {"sword", new Dictionary<string, Dictionary<string, int>>()
            {
                {"Wood", new Dictionary<string, int>()
                {
                    {"speed", 5 },
                    {"stamina", 10 }
                } },
                {"Stone", new Dictionary<string, int>()
                {
                    {"speed", 6 },
                    {"stamina", 9 }
                } },
                {"Copper", new Dictionary<string, int>()
                {
                    {"speed", 7 },
                    {"stamina", 8 }
                } },
                {"Iron", new Dictionary<string, int>()
                {
                    {"speed", 8 },
                    {"stamina", 7 }
                } }

            } },
            {"axe", new Dictionary<string, Dictionary<string, int>>()
            {
                {"Wood", new Dictionary<string, int>()
                {
                    {"speed", 5 },
                    {"stamina", 8 }
                } },
                {"Stone", new Dictionary<string, int>()
                {
                    {"speed", 6 },
                    {"stamina", 7 }
                } },
                {"Copper", new Dictionary<string, int>()
                {
                    {"speed", 7 },
                    {"stamina", 6 }
                } },
                {"Iron", new Dictionary<string, int>()
                {
                    {"speed", 8 },
                    {"stamina", 5 }
                } }

            } },
            {"pickaxe", new Dictionary<string, Dictionary<string, int>>()
            {
                {"Wood", new Dictionary<string, int>()
                {
                    {"speed", 5 },
                    {"stamina", 8 }
                } },
                {"Stone", new Dictionary<string, int>()
                {
                    {"speed", 6 },
                    {"stamina", 7 }
                } },
                {"Copper", new Dictionary<string, int>()
                {
                    {"speed", 7 },
                    {"stamina", 6 }
                } },
                {"Iron", new Dictionary<string, int>()
                {
                    {"speed", 8 },
                    {"stamina", 5 }
                } }

            } },
            {"torch", new Dictionary<string, Dictionary<string, int>>()
            {
                {"Basic", new Dictionary<string, int>()
                {
                    {"brightness", 5 }
                } }
            } }
        };        
    }
    
    public static int GetValue(string type, string quality, string field)
    {
        return _tools[type][quality][field];
    }

}
