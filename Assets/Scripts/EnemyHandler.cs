using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    public struct enemy
    {
        GameObject enemyType;
        int health;
        int maxHealth;
        int speed;

    }

    public GameObject basicEnemy;

    private List<enemy> enemies = new List<enemy>();
    
    private int currentCount = 0;
    private int killedCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnEnemy()
    {
        enemy newEnemy = new enemy();
        
    }
}
