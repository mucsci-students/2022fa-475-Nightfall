using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePool : MonoBehaviour
{
    public static ResourcePool SharedInstance;

    [Header("Trees")]
    [SerializeField] private GameObject[] treePrefabs;
    [SerializeField] private int treeAmountToPool;
    private List<GameObject> pooledTrees;
    
    [Header("Stone")]
    [SerializeField] private GameObject[] rockPrefabs;
    [SerializeField] private int rockAmountToPool;
    private List<GameObject> pooledRocks;

    [Header("Metals")]
    [SerializeField] private GameObject[] metalsPrefabs;
    [SerializeField] private int metalAmountToPool;
    private List<GameObject> pooledMetals;
    
    private Terrain[] myTerrains;

    void Awake()
    {
        SharedInstance = this;
        pooledTrees = new List<GameObject>();
        pooledRocks = new List<GameObject>();
        pooledMetals = new List<GameObject>();
    }

    void Start()
    {
        myTerrains = Terrain.activeTerrains;
        
        for(int i = 0; i < treeAmountToPool; i++)
        {
            SpawnTrees(myTerrains[1]);
        }

        for(int i = 0; i < rockAmountToPool; i++)
        {
            SpawnRocks(myTerrains[1]);
        }

        for(int i = 0; i < metalAmountToPool; i++)
        {
            SpawnMetals(myTerrains[1]);
        }
    }

    private void SpawnTrees(Terrain t)
    {
        
        int randomPrefab = UnityEngine.Random.Range(0, treePrefabs.Length);
        GameObject temp = Instantiate(treePrefabs[randomPrefab], Vector3.zero, Quaternion.identity);
        Transform spawnPoint = temp.transform;
        GenerateSpawnLocation(t, spawnPoint);
        //var randomRotation = Quaternion.Euler(Random.Range(0, 3), Random.Range(0, 100), Random.Range(0, 3));
        //temp.transform.rotation = randomRotation;
        // AlignTransform(spawnPoint, t);

        temp.SetActive(true);
        pooledTrees.Add(temp);
    }

    public GameObject GetPooledTrees()
    {
        for(int i = 0; i < treeAmountToPool; i++)
        {
            if(!pooledTrees[i].activeInHierarchy)
            {
                return pooledTrees[i];
            }
        }
        return null;
    }


    private void SpawnRocks(Terrain t)
    {
        // Grab random prefab from List of given prefabs.
        int randomPrefab = UnityEngine.Random.Range(0, rockPrefabs.Length);
        GameObject temp = Instantiate(rockPrefabs[randomPrefab], Vector3.zero, Quaternion.identity);
        Transform spawnPoint = temp.transform;
        
        GenerateSpawnLocation(t, spawnPoint);
        temp.SetActive(true);
        pooledRocks.Add(temp);
    }


    public GameObject GetPooledRocks()
    {
        for(int i = 0; i < rockAmountToPool; i++)
        {
            if(!pooledRocks[i].activeInHierarchy)
            {
                return pooledRocks[i];
            }
        }
        return null;
    }


    private void SpawnMetals(Terrain t)
    {
        // Grab random prefab from List of given prefabs.
        int randomPrefab = UnityEngine.Random.Range(0, metalsPrefabs.Length);
        GameObject temp = Instantiate(metalsPrefabs[randomPrefab],  Vector3.zero, Quaternion.identity);
        Transform spawnPoint = temp.transform;
        
        GenerateSpawnLocation(t, spawnPoint);
        temp.SetActive(true);
        pooledMetals.Add(temp);
    }

    public GameObject GetPooledMetals()
    {
        for(int i = 0; i < metalAmountToPool; i++)
        {
            if(!pooledMetals[i].activeInHierarchy)
            {
                return pooledMetals[i];
            }
        }
        return null;
    }

    private static Transform GenerateSpawnLocation(Terrain t, Transform spawnResourceLocation)
    {
        bool validSpawn;

        // Generate random coordinate within the bounds of the given terrain.
        Vector3 spawnLocation = new Vector3 
        (
            UnityEngine.Random.Range(t.transform.position.x, t.transform.position.x + t.terrainData.size.x),
            UnityEngine.Random.Range(t.transform.position.y, t.transform.position.y + t.terrainData.size.y),
            UnityEngine.Random.Range(t.transform.position.z, t.transform.position.z + t.terrainData.size.z)
        );

        // Make sure Y coordinate is ontop of terrain.
        Vector3 pos = spawnLocation;
        pos.y = t.SampleHeight(spawnLocation) + t.transform.position.y;
        spawnResourceLocation.position = pos;

        // Calculates normal of terrain and returns bool.
        validSpawn = AlignTransform(spawnResourceLocation, t);

        if(!validSpawn)
        {
            // If not valid, get new coordinates.
            return GenerateSpawnLocation(t, spawnResourceLocation);
        }

        return spawnResourceLocation;
    }

    /* Align resource with surface of terrain + check slope of terrain for trees */
    private static bool AlignTransform(Transform spawnResourceLocation, Terrain t)
    {
        // Get the normal vector for the coordinates resource is spawning at.
        Vector3 sample = SampleNormal(spawnResourceLocation, t);

        // If resource is a tree, check slope.
        if(spawnResourceLocation.tag == "Wood")
        {
            if(FindSurfaceSlope(sample) > 30f)
            {
                return false;
            }
        }
        
        // Resource positions appropriately regardless of slope.
        Vector3 proj = spawnResourceLocation.forward - (Vector3.Dot(spawnResourceLocation.forward, sample)) * sample;
        spawnResourceLocation.rotation = Quaternion.LookRotation(proj, sample);
        return true;
    }

    // Get the normal vector
    private static Vector3 SampleNormal(Transform spawnResourceLocation, Terrain t)
    {
        var terrainLocalPos = spawnResourceLocation.position - t.transform.position;
        var normalizedPos = new Vector2(
            Mathf.InverseLerp(0f, t.terrainData.size.x, terrainLocalPos.x),
            Mathf.InverseLerp(0f, t.terrainData.size.z, terrainLocalPos.z)
        );

        var terrainNormal = t.terrainData.GetInterpolatedNormal(normalizedPos.x, normalizedPos.y);
        return terrainNormal;
        
    }

    // Check angle of spawn point.
    private static float FindSurfaceSlope (Vector3 surfNormal) 
    {
        return Vector3.Angle (surfNormal, Vector3.up);
    }
}