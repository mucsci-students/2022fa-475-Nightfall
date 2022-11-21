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

        Inventory.AddItem("Wood", 5);
        if (_buildingManager.CanBuild("Thing", Inventory.GetAllItems(), out var result))
        {

            Instantiate(result.spawnables.ObjectToSpawn, transform.position, transform.rotation);

        }

        else { print("Can't do that!"); }

    }

}
