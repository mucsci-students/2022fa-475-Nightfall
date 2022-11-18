using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class BuildObjectPair
{

    public GameObject ObjectToSpawn;
    public GameObject OutlineObject;

}

[Serializable]
public class BuildableItem
{

    public string Name;
    public string[] RequiredItems;
    public BuildObjectPair Spawnables;
    public Dictionary<string, int> RequiredResources = new Dictionary<string, int>();

}

public class BuildingManager : MonoBehaviour
{

    [SerializeField]
    private BuildableItem[] _buildableItems;

    public IReadOnlyCollection<BuildableItem> BuildableItems => _buildableItems;

    // Start is called before the first frame update
    void Start() => RecordRequiredResources();

    private void RecordRequiredResources()
    {

        // Generate the requirements as a dictionary
        foreach (var item in _buildableItems)
        {
            
            foreach (var requiredItem in item.RequiredItems)
            {
                
                var commaSplitIndex = requiredItem.IndexOf(",");
                var resourceName = requiredItem.Substring(0, commaSplitIndex);
                var requiredQuantity = int.Parse(requiredItem.Substring(commaSplitIndex + 1));
                item.RequiredResources.Add(resourceName, requiredQuantity);

            }

        }

    }

    public bool CanBuild(string itemName, Dictionary<string, int> inventory, out (BuildObjectPair spawnables, Dictionary<string, int> cost) result)
    {

        result = (null, null);

        var matchingBuildable = _buildableItems.Where(item => item.Name == itemName).FirstOrDefault();
        if (matchingBuildable != null)
        {

            // Check that the player has all required resources to build the thing
            foreach (var requiredResource in matchingBuildable.RequiredResources)
            {

                var resourceName = requiredResource.Key;
                var requiredQuantity = requiredResource.Value;

                var hasItem = inventory
                    .Where(pair => pair.Key == resourceName && pair.Value >= requiredQuantity)
                    .Count() > 0;
                if (!hasItem) { return false; }

            }

            result.spawnables = new BuildObjectPair();
            result.spawnables.ObjectToSpawn = matchingBuildable.Spawnables.ObjectToSpawn;
            result.spawnables.OutlineObject = matchingBuildable.Spawnables.OutlineObject;
            result.cost = matchingBuildable.RequiredResources;
            return true;

        }

        return false;

    }

}
