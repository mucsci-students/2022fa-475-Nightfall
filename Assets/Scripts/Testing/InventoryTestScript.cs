using UnityEngine;

public class InventoryTestScript : MonoBehaviour
{

    private Inventory _inventory;
    private BuildingManager _buildingManager;

    // Start is called before the first frame update
    void Start()
    {

        _inventory = GetComponent<Inventory>();
        _buildingManager = GetComponent<BuildingManager>();

        Invoke(nameof(Test), 3);

    }

    void Test()
    {

        _inventory.AddItem("Wood", 5);
        if (_buildingManager.CanBuild("thing", _inventory.GetAllItems(), out var prefab))
        {

            Instantiate(prefab, transform.position, transform.rotation);

        }

    }

}
