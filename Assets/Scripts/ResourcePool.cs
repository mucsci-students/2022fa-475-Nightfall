using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePool : MonoBehaviour
{
    public static ResourcePool SharedInstance;

    [Header("Trees")]
    public List<GameObject> pooledTrees;
    public GameObject treeToPull;
    public int treeAmountToPool;

    [Header("Stone")]
    public List<GameObject> pooledRocks;
    public GameObject stoneToPool;
    public int rockAmountToPool;


    [Header("Terrain Spawn Coordinates")]
    public int xAxis;
    public int yAxis;
    public int zAxis;

    void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        pooledTrees = new List<GameObject>();
        GameObject temp;

        for(int i = 0; i < treeAmountToPool; i++)
        {
            Vector3 spawnLocation = GenerateSpawnLocation();
            Vector3 pos = spawnLocation;

            // I believe we can use "bounds" or something similar from:
            // https://docs.unity3d.com/ScriptReference/TerrainData.html
            // to obtain world coordinates for terrain.
            pos.y = Terrain.activeTerrain.SampleHeight(spawnLocation);
            spawnLocation = pos;

            temp = Instantiate(treeToPull, spawnLocation, Quaternion.identity);
            temp.SetActive(false);
            pooledTrees.Add(temp);
        }
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

    // public GameObject GetPooledRocks()
    // {
    //     for(int i = 0; i < rockAmountToPool; i++)
    //     {
    //         if(!pooledRocks[i].activeInHierarchy)
    //         {
    //             return pooledRocks[i];
    //         }
    //     }
    //     return null;
    // }

    Vector3 GenerateSpawnLocation()
    {
        int x,y,z;
        x = UnityEngine.Random.Range (-xAxis, xAxis);
        y = UnityEngine.Random.Range (-yAxis, yAxis);
        z = UnityEngine.Random.Range (-zAxis, zAxis);
        return new Vector3(x, y, z);
    }
}
