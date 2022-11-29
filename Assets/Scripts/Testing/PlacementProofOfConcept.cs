using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlacementProofOfConcept : MonoBehaviour
{

    private Camera _cameraHolderCamera;

    [SerializeField]
    private BuildObjectPair[] _spawnablePrefabs;
    private BuildObjectPair _activeBuildable;
    private GameObject _activeOutline;

    private bool _inSpawnMode;
    private int _currentItemIndex = 0;

    private float distanceLimit = 2;
    private float distanceDeltaSec = 2;
    private float minDistanceLimit = 2;
    private float maxdistanceLimit = 7;

    // Start is called before the first frame update
    void Start()
    {

        var cameras = GetComponentsInChildren<Camera>();
        _cameraHolderCamera = cameras.Where(camera => camera.name == "CameraHolder").FirstOrDefault();
        
    }

    // Update is called once per frame
    void Update()
    {
     
        // Toggle spawn mode
        if (Input.GetKeyDown(KeyCode.T)) 
        {
            
            _inSpawnMode = !_inSpawnMode;
            if (!_inSpawnMode && _activeBuildable != null && _activeOutline != null)
            {

                Destroy(_activeOutline);
                _activeOutline = null;
                _activeBuildable = null;

            }
            
            else if (_inSpawnMode && _activeBuildable == null && _activeOutline == null)
            {

                _activeBuildable = _spawnablePrefabs[_currentItemIndex];
                LoadOutline();

            }

        }

        // Increment the current item index
        if (Input.GetKeyDown(KeyCode.F) && _inSpawnMode) { CycleToNextBuildable(); }

        // Handle placing the item
        if (Input.GetMouseButtonDown(0) && _activeBuildable != null && _activeOutline != null)
        {

            Vector3 spawnLocation = _activeOutline.transform.position;
            Quaternion spawnRotation = _activeOutline.transform.rotation;

            Destroy(_activeOutline);
            Instantiate(_activeBuildable.ObjectToSpawn, spawnLocation, spawnRotation);
            _activeOutline = null;
            _activeBuildable = null;
            _inSpawnMode = false;

        }

        // Handle adjusting the distance of the object with the mouse wheel
        float adjustmentAxisValue = Input.GetAxis("AdjustObjectDistance");
        if (_activeOutline != null && (adjustmentAxisValue != 0))
        {
            distanceLimit += Time.deltaTime * distanceDeltaSec * adjustmentAxisValue;
            distanceLimit = Mathf.Clamp(distanceLimit, minDistanceLimit, maxdistanceLimit);
        }
        
        // Handle making the item follow where the player is looking
        if (_activeBuildable != null && _activeOutline != null && _inSpawnMode)
        {

            Renderer outlineRenderer = _activeOutline.GetComponentInChildren<Renderer>();
            float yOffset = outlineRenderer == null ? 0 : outlineRenderer.bounds.size.y / 2f;

            _activeOutline.transform.position = GetObjectPreviewLocation() + new Vector3(0f, yOffset, 0f);
            _activeOutline.transform.rotation = transform.rotation;

        }

    }

    private void CycleToNextBuildable()
    {

        _currentItemIndex++;
        if (_currentItemIndex >= _spawnablePrefabs.Length) { _currentItemIndex = 0; }

        // Cycle to the next object, spawn the new outline for it
        _activeBuildable = _spawnablePrefabs[_currentItemIndex];
        LoadOutline();

    }

    private void LoadOutline()
    {

        Destroy(_activeOutline);
        _activeOutline = Instantiate(_activeBuildable.OutlineObject, transform.position, transform.rotation);

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

        Vector3 targetLocation = hitLocation.point;

        /* Don't allow the obejct to come closer to the player than the min allowed distance
        float distanceToObject = Vector3.Distance(targetLocation, transform.position);
        if (distanceToObject < minDistanceLimit)
        {

            CharacterController characterController = GetComponentInChildren<CharacterController>();
            float outwardAngle = Mathf.Asin(characterController.bounds.size.y / minDistanceLimit);
            float distanceOffset = Mathf.Cos(outwardAngle) * minDistanceLimit;

            print($"{outwardAngle} {distanceOffset}");

            targetLocation = transform.forward * distanceOffset + transform.position;
            targetLocation.y = hitLocation.point.y;

            // targetLocation += offsetVector;
        }
        */

        return targetLocation;

    }

}
