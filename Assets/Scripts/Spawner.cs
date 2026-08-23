using System;
//using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody2D))]
public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] obstaclePrefabs;
    [SerializeField] private Transform obstacleParent;
    [SerializeField] private GameObject bossPrefab; 
    //префаб дополнительной жизни
    [Header("Extra Life Settings")]
    [SerializeField] private GameObject extraLifePrefab;
    private bool isExtraLifeSpawned = false;
    public float obstacleSpawnTime = 2f;
    [Range(0, 1)] public float obstacleSpawnTimeFactor = 0.1f;
    public float obstacleSpeed = 1f; 
    public float bossSpeed = 2f;
    [Range(0, 1)] public float obstacleSpeedFactor = 0.2f; 


    private float _obstacleSpawnTime;
    private float _obstacleSpeed;
    private float timeUntilObstacleSpawn;

    private float timeAlive; 

    private int countSpawn = 0; 

    public GameObject spawnedBoss;
    private bool isBossCreated = false; 
    
    private UnityEngine.Vector3 stopAt = new(6, -3, 0);
    
    private void Start()
    {
        GameManager.Instance.onGameOver.AddListener(ClearObstacles);
        GameManager.Instance.onPlay.AddListener(ResetFactors);
    }
    private void Update()
    {
        if (GameManager.Instance.isPlaying)
        {
            timeAlive += Time.deltaTime;

            CalculateFactors();
            
            SpawnLoop(); 

            BossSpawn(); 

            ExtraLifeSpawn();

            if (isBossCreated && spawnedBoss == null)
            {
                ResetLoopAfterBoss();
            }
        }
    }

    private void SpawnLoop()
    {
     timeUntilObstacleSpawn += Time.deltaTime;

     if (timeUntilObstacleSpawn >= _obstacleSpawnTime && countSpawn < 10)
        {
            Spawn(); 
            timeUntilObstacleSpawn = 0f; 
        } 
    } 

    private void ExtraLifeSpawn()
    {
        if (timeAlive >= 15 && timeAlive <= 20 && !isExtraLifeSpawned)
        {
            GameObject spawnedExtraLife = Instantiate(extraLifePrefab, transform.position, Quaternion.identity);
            spawnedExtraLife.transform.parent = obstacleParent;
            
            Rigidbody2D extraLifeRB = spawnedExtraLife.GetComponent<Rigidbody2D>();

            if (extraLifeRB != null)
            {
                extraLifeRB.linearVelocity = Vector2.left * _obstacleSpeed;
            }

            isExtraLifeSpawned = true;
            Debug.Log("Дополнительная жизнь появилась!");
        }

    }

    private void ResetLoopAfterBoss()
    {
        countSpawn = 0;
        isBossCreated = false; 
        timeUntilObstacleSpawn = 0f;
    }

    private void ClearObstacles()
    {
        foreach (Transform child in obstacleParent)
        {
            Destroy(child.gameObject);
        }
    }

    private void CalculateFactors()
    {
        _obstacleSpawnTime = obstacleSpawnTime / Mathf.Pow(timeAlive, obstacleSpawnTimeFactor);
        _obstacleSpeed = obstacleSpeed * Mathf.Pow(timeAlive, obstacleSpeedFactor);
    }

    private void ResetFactors()
    {
        timeAlive = 1f; 
        countSpawn = 0; 
        timeUntilObstacleSpawn = 0f; 
        isBossCreated = false; 
        isExtraLifeSpawned = false;
        _obstacleSpawnTime = obstacleSpawnTime;
        _obstacleSpeed = obstacleSpeed;

        if (spawnedBoss != null)
        {
            Destroy(spawnedBoss);
        }
    }

    // private void Destroy(System.Func<GameObject> gameObject)
    // {
    //     throw new System.NotImplementedException();
    // }

    private void Spawn()
    {
        GameObject obstacleToSpawn = obstaclePrefabs[UnityEngine.Random.Range(0, obstaclePrefabs.Length)];

        GameObject spawnedObstacle = Instantiate(obstacleToSpawn, transform.position, UnityEngine.Quaternion.identity);
        spawnedObstacle.transform.parent = obstacleParent; 

        Rigidbody2D obstacleRB = spawnedObstacle.GetComponent<Rigidbody2D>();
        obstacleRB.linearVelocity = UnityEngine.Vector2.left * _obstacleSpeed; 
        
        countSpawn += 1;
    } 

    public void BossSpawn()
    {

        if (countSpawn >= 10 && !isBossCreated)
        {
            spawnedBoss = Instantiate(bossPrefab, new UnityEngine.Vector3(12, -3, 0), UnityEngine.Quaternion.identity);
            isBossCreated = true;
        }
    }
}
