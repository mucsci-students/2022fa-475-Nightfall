using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildTestScript : MonoBehaviour
{

    private BuildingManager _buildManager;
    private IReadOnlyCollection<BuildableItem> _buildables;
    private int _currentBuildIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        
        _buildManager = GetComponent<BuildingManager>();
        _buildables = _buildManager.BuildableItems;

        _buildManager.OnExitedBuildMode += (_, _) => { print("Player exited build mode"); };
        _buildManager.OnItemPlaced += (_, _) => { print("Player placed item"); };

    }

    // Update is called once per frame
    void Update()
    {

        if (!_buildManager.InBuildMode && Input.GetKeyDown(KeyCode.T)) { LoadCurrentBuildable(); }

        else if (_buildManager.InBuildMode && Input.GetKeyDown(KeyCode.R)) 
        {

            if (++_currentBuildIndex >= _buildables.Count) { _currentBuildIndex = 0; }

            LoadCurrentBuildable();

        }
     
    }

    private void LoadCurrentBuildable() 
    {

        var target = _buildables.ElementAt(_currentBuildIndex);
        _buildManager.EnterBuildMode((target.Spawnables, new Dictionary<string, int>() { }));

    }

}
