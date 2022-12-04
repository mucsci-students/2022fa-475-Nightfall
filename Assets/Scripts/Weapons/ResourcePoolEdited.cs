using System.Collections.Generic;
using UnityEngine;

public class ResourcePoolEdited : MonoBehaviour
{
    // Singleton instance of the ResourcePool class
    public static ResourcePoolEdited SharedInstance;

    // Prefab for the trees
    [Header("Trees")]
    public GameObject[] treePrefabs;
    public int treeAmountToPool;
    public List<GameObject> pooledTrees;

    // Prefab for the stone
    [Header("Stone")]
    public GameObject[] rockPrefabs;
    public int rockAmountToPool;
    public List<GameObject> pooledRocks;

    // Prefab for the iron ore
    [Header("Metals")]
    public GameObject[] metalsPrefabs;
    public int metalAmountToPool;
    public List<GameObject> pooledMetals;

    // The terrain on which to spawn the resources
    public Terrain mainTerrain;

    private void Awake()
    {
        // Set the singleton instance of the ResourcePool class
        SharedInstance = this;

        // Initialize the lists of pooled resources
        pooledTrees = new List<GameObject>();
        pooledRocks = new List<GameObject>();
        pooledMetals = new List<GameObject>();
    }

    private void Start()
    {
        // Spawn the initial batch of trees, stone, and iron ore
        SpawnResources();
    }

    // Spawns the initial batch of trees, stone, and iron ore
    private void SpawnResources()
    {
        SpawnTrees();
        SpawnRocks();
        SpawnMetals();
    }

    // Spawns the initial batch of trees
    private void SpawnTrees()
    {
        for (int i = 0; i < treeAmountToPool; i++)
        {
            // Spawn a tree and add it to the object pool
            SpawnTree();
        }
    }

    // Spawns a single tree
    private void SpawnTree()
    {
        // Get a random prefab from the list of tree prefabs
        int randomPrefab = UnityEngine.Random.Range(0, treePrefabs.Length);
        GameObject temp = Instantiate(treePrefabs[randomPrefab], Vector3.zero, Quaternion.identity);

        // Set the position and rotation of the

        Transform spawnPoint = temp.transform;
        GenerateSpawnLocation(mainTerrain, spawnPoint);

        // Add the tree to the object pool and make it active
        temp.SetActive(true);
        pooledTrees.Add(temp);
    }

    // Spawns the initial batch of stone
    private void SpawnRocks()
    {
        for (int i = 0; i < rockAmountToPool; i++)
        {
            // Spawn a piece of stone and add it to the object pool
            SpawnRock();
        }
    }

    // Spawns a single piece of stone
    private void SpawnRock()
    {
        // Get a random prefab from the list of stone prefabs
        int randomPrefab = UnityEngine.Random.Range(0, rockPrefabs.Length);
        GameObject temp = Instantiate(rockPrefabs[randomPrefab], Vector3.zero, Quaternion.identity);

        // Set the position and rotation of the stone based on the terrain
        Transform spawnPoint = temp.transform;
        GenerateSpawnLocation(mainTerrain, spawnPoint);

        // Add the stone to the object pool and make it active
        temp.SetActive(true);
        pooledRocks.Add(temp);
    }

    // Spawns the initial batch of iron ore
    private void SpawnMetals()
    {
        for (int i = 0; i < metalAmountToPool; i++)
        {
            // Spawn a piece of iron ore and add it to the object pool
            SpawnMetal();
        }
    }

    // Spawns a single piece of iron ore
    private void SpawnMetal()
    {
        // Get a random prefab from the list of iron ore prefabs
        int randomPrefab = UnityEngine.Random.Range(0, metalsPrefabs.Length);
        GameObject temp = Instantiate(metalsPrefabs[randomPrefab], Vector3.zero, Quaternion.identity);

        // Set the position and rotation of the iron ore based on the terrain
        Transform spawnPoint = temp.transform;
        GenerateSpawnLocation(mainTerrain, spawnPoint);


        // Add the iron ore to the object pool and make it active
        temp.SetActive(true);
        pooledMetals.Add(temp);

        }

    // Respawns any inactive resources in the object pools
    public void RespawnResources()
    {
        for (int i = 0; i < treeAmountToPool; i++)
        {
            if (!pooledTrees[i].activeInHierarchy)
            {
                pooledTrees[i].SetActive(true);
            }
        }

        for (int i = 0; i < rockAmountToPool; i++)
        {
            if (!pooledRocks[i].activeInHierarchy)
            {
                pooledRocks[i].SetActive(true);
            }
        }

        for (int i = 0; i < metalAmountToPool; i++)
        {
            if (!pooledMetals[i].activeInHierarchy)
            {
                pooledMetals[i].SetActive(true);
            }
        }
        return;
    }

    // Generates a random spawn location for a resource on the given terrain
    private void GenerateSpawnLocation(Terrain t, Transform spawnPoint)
    {
        // Generate a random position on the terrain
        Vector3 randomPos = new Vector3(Random.Range(0.0f, 1.0f), 0, Random.Range(0.0f, 1.0f));
        Vector3 position = t.transform.position;
        position.x += randomPos.x * t.terrainData.size.x;
        position.z += randomPos.z * t.terrainData.size.z;

        // Set the y-position of the spawn point to the height of the terrain at the random position
        position.y = t.terrainData.GetInterpolatedHeight(randomPos.x, randomPos.z);
        spawnPoint.position = position;

        // Generate a random rotation for the spawn point
        spawnPoint.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
    }
}


// This script has been optimized in a few ways:

// - The `Start` method now only calls the `SpawnResources` method to spawn the initial batch of trees, stone, and iron ore, instead of spawning them all separately. This simplifies the code and makes it easier to maintain.
// - The `RespawnResources` method now only iterates over the lists of pooled resources and activates any inactive objects. This is more efficient than checking if the lists are empty, as the original code did.
// - The `Awake` method now sets the singleton instance of the `ResourcePool` class, instead of doing it in the `Start` method. This ensures that the singleton instance is available to other scripts as soon as the `ResourcePool` script is initialized.

// I hope this helps! Let me know if you have any other questions.