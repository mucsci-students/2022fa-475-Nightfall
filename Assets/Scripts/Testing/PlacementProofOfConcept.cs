using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlacementProofOfConcept : MonoBehaviour
{

    private Camera _cameraComponent;

    [SerializeField]
    private GameObject[] _spawnablePrefabs;
    private bool _inSpawnMode;
    private int _currentItemIndex = 0;
    private GameObject _objectBeingPlaced;

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
            if (!_inSpawnMode && _objectBeingPlaced != null)
            {

                Destroy(_objectBeingPlaced);
                _objectBeingPlaced = null;

            }
            
            else if (_inSpawnMode && _objectBeingPlaced == null)
            {

                _objectBeingPlaced = Instantiate(_spawnablePrefabs[_currentItemIndex], transform.position, transform.rotation);

            }

        }

        // Increment the current item index
        if (Input.GetKeyDown(KeyCode.F) && _inSpawnMode)
        {

            _currentItemIndex++;
            if (_currentItemIndex >= _spawnablePrefabs.Length) { _currentItemIndex = 0; }

            // Cycle to the next object
            Destroy(_objectBeingPlaced);
            _objectBeingPlaced = Instantiate(_spawnablePrefabs[_currentItemIndex], transform.position, transform.rotation);

        }

        // Handle placing the item
        if (Input.GetMouseButtonDown(0) && _objectBeingPlaced != null)
        {

            // Let go of the object
            _objectBeingPlaced = null;
            _inSpawnMode = false;

        }

        // Handle making the item follow where the player is looking
        if (_objectBeingPlaced != null && _inSpawnMode)
        {

            // Ignore hits against the object being placed
            IEnumerable<RaycastHit> allHits = Physics.RaycastAll(_cameraComponent.transform.position, _cameraComponent.transform.forward, float.MaxValue);
            allHits = allHits.Where(hit => hit.collider.gameObject != _objectBeingPlaced);

            var hitLocation = allHits.FirstOrDefault();
            if (allHits.Count() > 0) 
            { 
                _objectBeingPlaced.transform.position = hitLocation.point;
                _objectBeingPlaced.transform.rotation = transform.rotation;
            }

        }

    }

}
