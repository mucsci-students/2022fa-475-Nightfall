using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlacementProofOfConcept : MonoBehaviour
{

    private Camera _cameraComponent;

    [SerializeField]
    private BuildObjectPair[] _spawnablePrefabs;
    private BuildObjectPair _activeBuildable;
    private GameObject _activeOutline;

    private bool _inSpawnMode;
    private int _currentItemIndex = 0;

    // Start is called before the first frame update
    void Start()
        => _cameraComponent = GetComponentInChildren<Camera>();

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

        // Handle making the item follow where the player is looking
        if (_activeBuildable != null && _activeOutline != null && _inSpawnMode)
        {

            // Ignore hits against the object being placed
            IEnumerable<RaycastHit> allHits = Physics.RaycastAll(_cameraComponent.transform.position, _cameraComponent.transform.forward, float.MaxValue);
            allHits = allHits.Where(hit => hit.collider.gameObject != _activeOutline);

            Renderer outlineRenderer = _activeOutline.GetComponentInChildren<Renderer>();
            float yOffset = outlineRenderer == null ? 0 : outlineRenderer.bounds.size.y / 2f;

            var hitLocation = allHits.FirstOrDefault();
            if (allHits.Count() > 0) 
            {
                _activeOutline.transform.position = hitLocation.point + new Vector3(0f, yOffset, 0f);
                _activeOutline.transform.rotation = transform.rotation;
            }

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

}
