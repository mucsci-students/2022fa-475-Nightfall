using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHandler : MonoBehaviour
{
    private string[] policies = { "passive", "patrol", "cautious", "aggressive" };
    private float spawnDelay = 5f;

    public GameObject skeletonEnemy;
    public GameObject player;

    private Transform[] spawnPositions;
    private Transform[] distantPositions;

    public struct enemy
    {
        public string name;
        public GameObject enemyObject;
        public Transform spawnPoint;
        public Transform target;
        public NavMeshAgent agent;
        public string policy;
        public int health;
        public int maxHealth;
    }

    private List<enemy> enemies = new List<enemy>();
    
    private int currentCount = 0;
    private int killedCount = 0;
    private float lastSpawn;

    // Start is called before the first frame update
    void Start()
    {
        lastSpawn = Time.time;
        
        GameObject parent = GameObject.Find("SpawnPositions");
        spawnPositions = parent.GetComponentsInChildren<Transform>();
        distantPositions = GameObject.Find("Player/DistantPositions").GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastSpawn > spawnDelay)
        {
            lastSpawn = Time.time;
            SpawnEnemy();
        }

        
        for (int i = 0; i < enemies.Count; ++i)
        {
            enemies[i].agent.destination = enemies[i].target.position;

            if (!enemies[i].agent.pathPending)
            {
                if (enemies[i].agent.remainingDistance <= enemies[i].agent.stoppingDistance)
                {
                    if (!enemies[i].agent.hasPath || enemies[i].agent.velocity.sqrMagnitude == 0f)
                    {
                        enemy e = enemies[i];
                        e.target = GetDestination(enemies[i]);
                        Debug.Log("New target: " + e.target);
                        enemies[i] = e;
                    }
                }
            }
        }
    }

    private Transform GetDestination(enemy e)
    {
        Transform temp = e.;
        temp.Translate(e.target.position);
        Debug.Log("Transform - Target " + e.target.position);
        if (e.policy == "passive")
        {
            temp.transform.Translate(e.spawnPoint.position + new Vector3(Random.Range(0, 3), Random.Range(0, 3), 0));
        }
        else if (e.policy == "patrol")
        {
            temp.transform.Translate(spawnPositions[(int)Random.Range(0, spawnPositions.Length - 1)].position + new Vector3(Random.Range(0, 3), Random.Range(0, 3), 0));
            //temp = spawnPositions[(int)Random.Range(0, spawnPositions.Length - 1)];
        }
        else if (e.policy == "cautious")
        {
            temp.transform.Translate(distantPositions[(int)Random.Range(0, spawnPositions.Length - 1)].position + new Vector3(Random.Range(0, 3), Random.Range(0, 3), 0));
            //temp = distantPositions[(int)Random.Range(0, spawnPositions.Length - 1)];
        }
        else if (e.policy == "aggressive")
        {
            temp.transform.Translate(player.transform.position + new Vector3(Random.Range(0, 3), Random.Range(0, 3), 0));
        }
        return temp;
    }

    private void SpawnEnemy()
    {
        enemy newEnemy = new enemy();
        Transform spawnPos = spawnPositions[(int)Random.Range(0,spawnPositions.Length - 1)];
        //newEnemy.spawnPoint.position = spawnPos.position;
        //newEnemy.spawnPoint.rotation = spawnPos.rotation;
        newEnemy.enemyObject = Instantiate(skeletonEnemy, spawnPos.position, spawnPos.rotation);
        newEnemy.maxHealth = 100;
        newEnemy.health = newEnemy.maxHealth;
        newEnemy.agent = newEnemy.enemyObject.GetComponent<NavMeshAgent>();
        newEnemy.agent.speed = 10;
        newEnemy.policy = policies[(int)Random.Range(0, 4f)];

        newEnemy.target = GetDestination(newEnemy);

        //Debug.Log(newEnemy.name + " spawned at " + spawnPos.position + " with policy " + newEnemy.policy + " and target " + newEnemy.target.position);
        
        enemies.Add(newEnemy);
    }
}
