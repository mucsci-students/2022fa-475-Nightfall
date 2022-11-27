using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    public struct enemy
    {
        public GameObject enemyType;
        public int health;
        public int maxHealth;
        public int speed;

    }

    public GameObject basicEnemy;

    private List<enemy> enemies = new List<enemy>();
    
    private int currentCount = 0;
    private int killedCount = 0;
    private float lastSpawn;

    // Start is called before the first frame update
    void Start()
    {
        lastSpawn = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastSpawn > 20)
        {
            lastSpawn = Time.time;
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        enemy newEnemy = new enemy();
        newEnemy.enemyType = Instantiate(basicEnemy);
        newEnemy.maxHealth = 100;
        newEnemy.health = newEnemy.maxHealth;
        newEnemy.speed = 20;
    }
}
