using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class BuildableItem
{

    public string Name;
    public string[] RequiredItems;
    public GameObject PlacableObject;
    public Dictionary<string, int> RequiredResources = new Dictionary<string, int>();

}

public class BuildingManager : MonoBehaviour
{

    [SerializeField]
    private BuildableItem[] BuildableItems;

    // Start is called before the first frame update
    void Start() => RecordRequiredResources();

    private void RecordRequiredResources()
    {

        // Generate the requirements as a dictionary
        foreach (var item in BuildableItems)
        {
            
            foreach (var requiredItem in item.RequiredItems)
            {
                
                var commaSplitIndex = requiredItem.IndexOf(",");
                var resourceName = requiredItem.Substring(0, commaSplitIndex).ToLower();
                var requiredQuantity = int.Parse(requiredItem.Substring(commaSplitIndex + 1));
                item.RequiredResources.Add(resourceName, requiredQuantity);

            }

        }

    }

    public bool CanBuild(string itemName, Dictionary<string, int> inventory, out GameObject buildablePrefab)
    {

        buildablePrefab = null;
        itemName = itemName.ToLower();

        var matchingBuildable = BuildableItems.Where(item => item.Name.ToLower() == itemName).FirstOrDefault();
        if (matchingBuildable != null)
        {

            foreach (var requiredResource in matchingBuildable.RequiredResources)
            {

                var resourceName = requiredResource.Key;
                var requiredQuantity = requiredResource.Value;

                var hasItem = inventory
                    .Where(pair => pair.Key.ToLower() == resourceName && pair.Value >= requiredQuantity)
                    .Count() > 0;
                if (!hasItem) { return false; }

            }

            buildablePrefab = matchingBuildable.PlacableObject;
            return true;

        }

        return false;

    }

}
