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
        GameObject temp;
        Vector3 spawnLocation = GenerateSpawnLocation(t);
        int randomPrefab = UnityEngine.Random.Range(0, treePrefabs.Length);

        temp = Instantiate(treePrefabs[randomPrefab], spawnLocation, Quaternion.identity);
        
        var randomRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        temp.transform.rotation = randomRotation;
        AlignTransform(temp.transform, t);

        
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
        GameObject temp;
        Vector3 spawnLocation = GenerateSpawnLocation(t);
        
        // Grab random prefab from List of given prefabs.
        int randomPrefab = UnityEngine.Random.Range(0, rockPrefabs.Length);

        temp = Instantiate(rockPrefabs[randomPrefab], spawnLocation, Quaternion.identity);
        AlignTransform(temp.transform, t);
        var randomRotation = Quaternion.Euler(0, Random.Range(0, 360) , 0);
        temp.transform.rotation = randomRotation;
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
        GameObject temp;
        Vector3 spawnLocation = GenerateSpawnLocation(t);
        
        // Grab random prefab from List of given prefabs.
        int randomPrefab = UnityEngine.Random.Range(0, metalsPrefabs.Length);

        temp = Instantiate(metalsPrefabs[randomPrefab], spawnLocation, Quaternion.identity);
        AlignTransform(temp.transform, t);
        var randomRotation = Quaternion.Euler(0, Random.Range(0, 360)  , 0);
        temp.transform.rotation = randomRotation;
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

    private Vector3 GenerateSpawnLocation(Terrain t)
    {
        // Generate random coordinate within the bounds of the given terrain.
        Vector3 spawnLocation = new Vector3 (
            UnityEngine.Random.Range(t.transform.position.x, t.transform.position.x + t.terrainData.size.x),
            UnityEngine.Random.Range(t.transform.position.y, t.transform.position.y + t.terrainData.size.y),
            UnityEngine.Random.Range(t.transform.position.z, t.transform.position.z + t.terrainData.size.z)
        );

        Vector3 pos = spawnLocation;

        // Make sure Y coordinate is ontop of terrain.
        pos.y = t.SampleHeight(spawnLocation) + t.transform.position.y;
        spawnLocation = pos;

        return spawnLocation;
    }

    private static void AlignTransform(Transform transform, Terrain t)
    {
        Vector3 sample = SampleNormal(transform.position, t);
        
        Vector3 proj = transform.forward - (Vector3.Dot(transform.forward, sample)) * sample;
        transform.rotation = Quaternion.LookRotation(proj, sample);
    }

    private static Vector3 SampleNormal(Vector3 position, Terrain t)
    {
        var terrainLocalPos = position - t.transform.position;
        var normalizedPos = new Vector2(
            Mathf.InverseLerp(0f, t.terrainData.size.x, terrainLocalPos.x),
            Mathf.InverseLerp(0f, t.terrainData.size.z, terrainLocalPos.z)
        );
        var terrainNormal = t.terrainData.GetInterpolatedNormal(normalizedPos.x, normalizedPos.y);
        return terrainNormal;
    }

}