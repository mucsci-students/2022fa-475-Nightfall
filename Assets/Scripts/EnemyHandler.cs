using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHandler : MonoBehaviour
{
    private static string[] policies = { "passive", "patrol", "cautious", "aggressive" };
    private static float[] ghoulProbabilities = { .25f, .25f, .25f, .25f };
    private static float[] skeletonProbabilities = { .25f, .25f, .25f, .25f };
    private static float epsilon = .05f;

    private float spawnDelay = 60;

    public static GameManager gameManager;
    public GameObject skeletonEnemy;
    public GameObject ghoulEnemy;
    public static GameObject player;

    private static GameObject[] spawns;
    private static Transform[] distantPositions;

    public struct enemy
    {
        public string name;
        public string type;
        public GameObject enemyObject;
        public Transform spawnPoint;
        public Transform target;
        public NavMeshAgent agent;
        public string policy;
        public int health;
        public int maxHealth;
        public float timeIdling;
        public int index;
    }

    private static List<enemy> enemies = new List<enemy>();

    private static int totalCount = 0;
    private static int currentCount = 0;
    private static int killedCount = 0;
    private static int maxEnemies = 20;
    private float timeElapsed = 0;
    private float lastSpawn;
    private List<bool> canAttack;

    // Start is called before the first frame update
    void Start()
    {
        lastSpawn = Time.time;
        canAttack = new List<bool>();

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.Find("Player");
        GameObject parent = GameObject.Find("SpawnPositions");
        //spawnPositions = parent.GetComponentsInChildren<Transform>();
        spawns = GameObject.FindGameObjectsWithTag("Stone");
        distantPositions = GameObject.Find("Player/DistantPositions").GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        if (Time.time - lastSpawn > spawnDelay && currentCount < maxEnemies)
        {
            lastSpawn = Time.time;
            SpawnEnemy();
        }
        if (timeElapsed >= .2f)
        {
            timeElapsed -= .2f;
            for (int i = 0; i < enemies.Count; ++i)
            {
                if (!enemies[i].agent.isOnNavMesh)
                {
                    enemies[i].enemyObject.transform.position = spawns[Random.Range(0, spawns.Length)].transform.position;
                }
                enemies[i].agent.destination = enemies[i].target.position;
                if (!enemies[i].agent.pathPending)
                {
                    if (enemies[i].agent.remainingDistance <= enemies[i].agent.stoppingDistance)
                    {
                        if (!enemies[i].agent.hasPath || enemies[i].agent.velocity.sqrMagnitude == 0f)
                        {
                            enemy e = enemies[i];
                            if (e.type == "Ghoul")
                            {
                                if (e.policy == "aggressive")
                                {
                                    e.enemyObject.transform.LookAt(player.transform);
                                    e.enemyObject.GetComponent<GhoulAnim>().Attack();
                                    if ((e.enemyObject.transform.position - player.transform.position).magnitude < 5 && canAttack[e.index])
                                    {
                                        canAttack[e.index] = false;
                                        PlayerHandler.AddValue("health", -10);
                                        StartCoroutine(AttackCooldown(e.index));
                                    }
                                }
                                else
                                {
                                    e.enemyObject.GetComponent<GhoulAnim>().Idle();
                                }
                            }
                            else
                            {
                                if (e.policy == "aggressive")
                                {
                                    e.enemyObject.transform.LookAt(player.transform);
                                    e.enemyObject.GetComponent<SkeletonAnim>().Attack();
                                    if ((e.enemyObject.transform.position - player.transform.position).magnitude < 5 && canAttack[e.index])
                                    {
                                        canAttack[e.index] = false;
                                        PlayerHandler.AddValue("health", -5);
                                        StartCoroutine(AttackCooldown(e.index));
                                    }
                                }
                                else
                                {
                                    e.enemyObject.GetComponent<SkeletonAnim>().Idle();
                                }
                            }
                            e.timeIdling += Time.deltaTime;
                            if (e.policy != "aggressive" && e.timeIdling >= 10)
                            {
                                e.timeIdling = 0;
                                UpdateProbabilities(e);
                                Debug.Log("Probabilities: Ghoul:" + ghoulProbabilities[0] + " " + ghoulProbabilities[1] + " " + ghoulProbabilities[2] + " "
                                    + ghoulProbabilities[3] + " Skel:" + skeletonProbabilities[0] + " " + +skeletonProbabilities[1] + " " + +skeletonProbabilities[2]
                                        + " " + +skeletonProbabilities[3]);
                                e.policy = GetPolicy(e.type);
                                GetDestination(ref e);
                                Debug.Log(e.name + " changed policy to " + e.policy);                            
                            }
                            enemies[i] = e;
                        }
                    }
                    else
                    {
                        if (enemies[i].type == "Ghoul")
                        {
                            enemies[i].enemyObject.GetComponent<GhoulAnim>().Walk();
                        }
                        else
                        {
                            enemies[i].enemyObject.GetComponent<SkeletonAnim>().Walk();
                        }
                    }
                }
            }
        }
    }

    private static void GetDestination(ref enemy e)
    {        
        if (e.policy == "passive")
        {
            Transform temp = e.spawnPoint;
            temp.Translate(Random.Range(0, 4), Random.Range(0, 4), 0);
            e.target = temp;
        }
        else if (e.policy == "patrol")
        {
            int num = Random.Range(0, spawns.Length - 1) + 1;
            e.target = spawns[num].transform;
        }
        else if (e.policy == "cautious")
        {
            int num = Random.Range(0, distantPositions.Length - 1) + 1;
            e.target = distantPositions[num];
        }
        else if (e.policy == "aggressive")
        {
            e.target = player.transform;
        }
    }

    private static void UpdateProbabilities(enemy e)
    {
        float reward = 0;

        if (gameManager.GetTimeOfDay() > 8 && gameManager.GetTimeOfDay() < 20)
        {
            if (e.policy == "aggressive")
            {
                reward -= 10;
            }
            else if (e.policy == "cautious")
            {
                reward -= 4;
            }
            if (e.policy == "patrol")
            {
                reward += 4;
            }
            if (e.policy == "passive")
            {
                reward += 2;
            }
            if (e.type == "Ghoul" && e.policy == "passive")
            {
                reward += 10;
            }
        }
        else
        {
            if (e.policy == "aggressive")
            {
                reward += 5;
                if (e.type == "Ghoul")
                {
                    reward += 10;
                }
            }
            else if (e.policy != "cautious")
            {
                reward -= 2;
                if (e.type == "Ghoul")
                {
                    reward += 5;
                }
            }
        }

        reward += e.policy == "aggressive" || e.policy == "cautious" ? gameManager.GetDaysSurvived() : -gameManager.GetDaysSurvived();
        reward += e.policy == "aggressive" || e.policy == "cautious" ? currentCount : -currentCount;
        reward += e.policy == "aggressive" || e.policy == "cautious" ? -killedCount : killedCount;

        int index = 0;
        while (policies[index] != e.policy)
        {
            ++index;
        }
        if (e.type == "Ghoul")
        {
            ghoulProbabilities[index] = Mathf.Max(epsilon, ghoulProbabilities[index] + (ghoulProbabilities[index] * reward / 100));
        }
        else
        {
            skeletonProbabilities[index] = Mathf.Max(epsilon, skeletonProbabilities[index] + (skeletonProbabilities[index] * reward / 100));
        }
    }

    private string GetPolicy(string type)
    {
        float[] probabilities;
        if (type == "Ghoul")
        {
            probabilities = ghoulProbabilities;
        }
        else
        {
            probabilities = skeletonProbabilities;
        }
            int index = -1;
        while (index == -1)
        {
            int i = Random.Range(0, policies.Length);
            if (Random.Range(0, 1) < probabilities[i])
            {
                index = i;
            }
        }

        return policies[index];
    }

    private void SpawnEnemy()
    {
        totalCount += 1;
        currentCount += 1;
        enemy newEnemy = new enemy();
        newEnemy.spawnPoint = spawns[Random.Range(0, spawns.Length - 1) + 1].transform;

        GameObject enemyObj;
        if (gameManager.GetTimeOfDay() > 6 && gameManager.GetTimeOfDay() < 20)
        {
            enemyObj = Random.Range(0f, 1f) < .9f ? skeletonEnemy : ghoulEnemy;
        }
        else
        {
            enemyObj = Random.Range(0f, 1f) < .5f ? skeletonEnemy : ghoulEnemy;
        }

        newEnemy.enemyObject = Instantiate(enemyObj, newEnemy.spawnPoint.position, newEnemy.spawnPoint.rotation);
        newEnemy.name = enemyObj.name + totalCount;
        newEnemy.type = enemyObj.name;
        newEnemy.enemyObject.name = newEnemy.name;
        newEnemy.maxHealth = newEnemy.type == "Ghoul" ? 50 : 30;
        newEnemy.health = newEnemy.maxHealth;
        newEnemy.agent = newEnemy.enemyObject.GetComponent<NavMeshAgent>();
        newEnemy.timeIdling = 0;
        newEnemy.index = currentCount;
        canAttack.Add(true);
        newEnemy.policy = GetPolicy(newEnemy.type);

        GetDestination(ref newEnemy);

        Debug.Log(newEnemy.name + " spawned at " + newEnemy.spawnPoint.position + " with policy " + newEnemy.policy + " and target " + newEnemy.target);
        
        enemies.Add(newEnemy);
    }

    public static void HitEnemy(string name, int damage)
    {
        Debug.Log("Hit " + name + " for " + damage);
        for (int i = 0; i < enemies.Count; ++i)
        {
            if (enemies[i].name == name)
            {
                enemy e = enemies[i];
                e.health -= damage;

                if (e.health <= 0)
                {
                    if (e.type == "Ghoul")
                    {
                        e.enemyObject.GetComponent<GhoulAnim>().Kill();
                    }
                    else
                    {
                        e.enemyObject.GetComponent<SkeletonAnim>().Kill();
                    }
                    e.agent.enabled = false;
                    UpdateProbabilities(e);
                    Debug.Log("Probabilities: Ghoul:" + ghoulProbabilities[0] + " " + ghoulProbabilities[1] + " " + ghoulProbabilities[2] + " "
                                        + ghoulProbabilities[3] + " Skel:" + skeletonProbabilities[0] + " " + +skeletonProbabilities[1] + " " + +skeletonProbabilities[2]
                                         + " " + +skeletonProbabilities[3]);
                    currentCount--;
                    killedCount++;
                    Destroy(e.enemyObject, 2);
                    enemies.RemoveAt(i);
                }
                else
                {
                    if (e.type == "Ghoul")
                    {
                        e.enemyObject.GetComponent<GhoulAnim>().Hit();
                    }
                    else
                    {
                        e.enemyObject.GetComponent<SkeletonAnim>().Hit();
                    }
                    Debug.Log(e.name + " changed policy to aggressive");
                    e.policy = "aggressive";
                    GetDestination(ref e);
                    enemies[i] = e;
                }
            }
        }
    }

    public void UpdateMaxEnemies(int days)
    {
        maxEnemies = 20 + (int)(days * 1.5);
    }

    IEnumerator AttackCooldown(int i)
    {
        yield return new WaitForSeconds(1f);
        canAttack[i] = true;            
    }
}
