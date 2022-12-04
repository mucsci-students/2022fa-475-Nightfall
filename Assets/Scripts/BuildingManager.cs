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

    // Placement variables
    public bool InBuildMode { get; private set; }
    private (BuildObjectPair spawnables, Dictionary<string, int> cost) _currentBuildConfig;
    private GameObject _activeOutline;
    private Camera _cameraHolderCamera;

    // Placement distance variables
    private float distanceLimit = 2;
    private float distanceDeltaSec = 2;
    private float minDistanceLimit = 2;
    private float maxdistanceLimit = 7;

    public IReadOnlyCollection<BuildableItem> BuildableItems => _buildableItems;

    public event EventHandler OnExitedBuildMode;
    public event EventHandler OnItemPlaced;

    // Start is called before the first frame update
    void Start()
    {
        
        RecordRequiredResources();
        var cameras = GetComponentsInChildren<Camera>();
        _cameraHolderCamera = cameras.Where(camera => camera.name == "CameraHolder").FirstOrDefault();

    }

    void Update()
    {

        // Handle placing the outline
        if (InBuildMode && _activeOutline != null) { PlaceOutline(); }

        // Handle adjusting the distance limit
        float adjustmentAxisValue = Input.GetAxis("AdjustObjectDistance");
        if (InBuildMode && (adjustmentAxisValue != 0))
        {
            AdjustDistanceLimit(Time.deltaTime * distanceDeltaSec * adjustmentAxisValue);
        }

        // Handle confirming the build
        if (InBuildMode && Input.GetKeyDown(KeyCode.F)) { ConfirmPurchase(); }

        // Handle cancelling
        if (InBuildMode && Input.GetKeyDown(KeyCode.Y)) { ExitBuildMode(); }

    }

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

    public void EnterBuildMode((BuildObjectPair spawnables, Dictionary<string, int> cost) buildConfig)
    {

        // Erase the old outline
        if (InBuildMode) { DestroyOutline(); }

        _currentBuildConfig = buildConfig;
        _activeOutline = Instantiate(_currentBuildConfig.spawnables.OutlineObject, transform.position, transform.rotation);

        InBuildMode = true;

    }

    private void ExitBuildMode()
    {

        InBuildMode = false;
        _currentBuildConfig = (null, null);
        DestroyOutline();
        OnExitedBuildMode?.Invoke(this, EventArgs.Empty);

    }

    private void DestroyOutline()
    {
        
        Destroy(_activeOutline);
        _activeOutline = null;

    }

    private void AdjustDistanceLimit(float delta) 
        => distanceLimit = Mathf.Clamp(distanceLimit + delta, minDistanceLimit, maxdistanceLimit);

    private void PlaceOutline()
    {

        Renderer outlineRenderer = _activeOutline.GetComponentInChildren<Renderer>();
        print(outlineRenderer == null);
        float yOffset = outlineRenderer == null ? 0 : outlineRenderer.bounds.size.y / 2f;

        _activeOutline.transform.position = GetObjectPreviewLocation() + new Vector3(0f, yOffset, 0f);
        _activeOutline.transform.rotation = transform.rotation;

    }

    private Vector3 GetObjectPreviewLocation()
    {

        // Ignore hits against the object being placed and the player
        IEnumerable<RaycastHit> allHits = Physics.RaycastAll(_cameraHolderCamera.transform.position, _cameraHolderCamera.transform.forward, distanceLimit);
        allHits = allHits.Where(hit => hit.collider.gameObject != _activeOutline && hit.collider.gameObject != gameObject);

        // In the case that there was no raycast hits, position at the point at the end of the limit
        if (allHits.Count() == 0)
        {
            Vector3 cameraForwardVector = _cameraHolderCamera.transform.forward;
            return cameraForwardVector * distanceLimit + _cameraHolderCamera.transform.position;
        }

        // Take the closest hit to the player
        RaycastHit hitLocation = allHits.FirstOrDefault();
        float closestSoFar = Vector3.Distance(hitLocation.point, transform.position);
        foreach (var hit in allHits)
        {
            float distance = Vector3.Distance(hit.point, transform.position);
            if (distance < closestSoFar)
            {
                closestSoFar = distance;
                hitLocation = hit;
            }
        }

        return hitLocation.point;

    }

    private void ConfirmPurchase()
    {

        Instantiate(_currentBuildConfig.spawnables.ObjectToSpawn, _activeOutline.transform.position, _activeOutline.transform.rotation);

        Inventory.RemoveItems(_currentBuildConfig.cost);
        OnItemPlaced?.Invoke(this, EventArgs.Empty);
        ExitBuildMode();

    }

}
