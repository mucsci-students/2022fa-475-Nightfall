using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHandler : MonoBehaviour
{
    public struct enemy
    {
        public GameObject enemyObject;
        public NavMeshAgent agent;
        public int health;
        public int maxHealth;

    }

    public GameObject skeletonEnemy;
    public GameObject player;

    private List<enemy> enemies = new List<enemy>();
    private Transform[] spawnPositions;
    
    private int currentCount = 0;
    private int killedCount = 0;
    private float lastSpawn;

    // Start is called before the first frame update
    void Start()
    {
        lastSpawn = Time.time;
        
        GameObject parent = GameObject.Find("SpawnPositions");
        spawnPositions = parent.GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastSpawn > 20)
        {
            lastSpawn = Time.time;
            SpawnEnemy();
        }

        
        foreach (enemy e in enemies)
        {
            e.agent.destination = player.transform.position;
        }
    }

    private void SpawnEnemy()
    {
        enemy newEnemy = new enemy();
        newEnemy.enemyObject = Instantiate(skeletonEnemy, spawnPositions[0].position, spawnPositions[0].rotation);
        newEnemy.maxHealth = 100;
        newEnemy.health = newEnemy.maxHealth;
        newEnemy.agent = newEnemy.enemyObject.GetComponent<NavMeshAgent>();
        newEnemy.agent.speed = 10;
        
        enemies.Add(newEnemy);
    }
}
