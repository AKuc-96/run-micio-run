using System;
//using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

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
    [Range(0, 1)] public float obstacleSpawnTimeFactor = 0.1f;
    public float obstacleSpeed = 1f; 
    public float bossSpeed = 2f;
    [Range(0, 1)] public float obstacleSpeedFactor = 0.2f; 


    private float _spawnDelayTime;
    private float _obstacleSpawnTime;
    private float _obstacleSpeed;
    private float timeUntilObstacleSpawn;

    private float timeAlive; 

    private int countSpawn = 0; 

    public GameObject spawnedBoss;
    private bool isBossCreated = false; 

    private PhaseConfig _currentPhase;
    
    private UnityEngine.Vector3 stopAt = new(6, -3, 0); 

    private float _currentExtraLifeSpawnChance; 
    private float _phaseTimer;
    
    private void OnEnable()
    {
        LevelManager.OnPhaseChanged += HandlePhaseChanged;
    }

    private void OnDisable()
    {
        LevelManager.OnPhaseChanged -= HandlePhaseChanged;
    }

    private void Start()
    {
        GameManager.onGameOver += ClearObstacles;
        GameManager.onPlay += ResetFactors;
        _phaseTimer = 0f;
    }
    internal void Update()
    {
        if (GameManager.Instance?.isPlaying == true)
        {
            Tick(Time.deltaTime);
        }
    }

    internal void Tick(float deltaTime)
    {
        timeAlive += deltaTime;
        _phaseTimer += deltaTime;

            //CalculateFactors();
            
        SpawnLoop(deltaTime); 

        if (isBossCreated && spawnedBoss == null)
        {
            ResetLoopAfterBoss();
        }

        if (_currentPhase != null && !_currentPhase.IsBossPhase && _phaseTimer >= (_currentPhase.PhaseDuration - 5.0f) && !isExtraLifeSpawned)
        {
            if (extraLifePrefab != null)
            {
                GameObject spawnedExtraLife = Instantiate(extraLifePrefab, transform.position, Quaternion.identity);
                spawnedExtraLife.transform.parent = obstacleParent; 
            }
            
            isExtraLifeSpawned = true;
        }
    }

    private void SpawnLoop(float deltaTime)
    {
        if (_currentPhase == null || _currentPhase.IsBossPhase || _obstacleSpawnTime <= 0f)
            return;
        
        if (_spawnDelayTime > 0f)
        {
            _spawnDelayTime -= deltaTime;
            return;
        }

        timeUntilObstacleSpawn += deltaTime;

        if (timeUntilObstacleSpawn >= _obstacleSpawnTime)
            {
                Spawn(); 
                timeUntilObstacleSpawn = 0f; 
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
        _obstacleSpeed = obstacleSpeed * Mathf.Pow(timeAlive, obstacleSpeedFactor);
    }

    private void ResetFactors()
    {
        timeAlive = 1f; 
        countSpawn = 0; 
        timeUntilObstacleSpawn = 0f; 
        isBossCreated = false; 
        isExtraLifeSpawned = false;
        _obstacleSpeed = obstacleSpeed;

        if (spawnedBoss != null)
        {
            Destroy(spawnedBoss);
        }
    }

    internal void HandlePhaseChanged(PhaseConfig config)
    {
        _phaseTimer = 0f;
        isExtraLifeSpawned = false;
        _currentPhase = config;
        _obstacleSpeed = config.ObstacleSpeed;
        _obstacleSpawnTime = config.SpawnInterval; 
        if (_currentPhase.IsBossPhase)
        {
            BossSpawn();
        }
        else
        {
            _spawnDelayTime = 3f;
        }
        Debug.Log($"[Spawner] Применена фаза! Скорость: {_obstacleSpeed}, Интервал: {_obstacleSpawnTime}");
    }

    private void Spawn()
    {
        if (obstaclePrefabs != null && obstaclePrefabs.Length > 0)
        {
            GameObject obstacleToSpawn = obstaclePrefabs[UnityEngine.Random.Range(0, obstaclePrefabs.Length)];
            GameObject spawnedObstacle = Instantiate(obstacleToSpawn, transform.position, Quaternion.identity);
            spawnedObstacle.transform.parent = obstacleParent; 

            if (spawnedObstacle.TryGetComponent<Rigidbody2D>(out var obstacleRB))
            {
                obstacleRB.linearVelocity = UnityEngine.Vector2.left * _obstacleSpeed; 
            }
        }

        countSpawn += 1;

        if (_currentPhase != null && _currentPhase.CanSpawnExtraLife && !isExtraLifeSpawned)
        {
            _currentExtraLifeSpawnChance = _randomRange(0f, 100f);

            if (_currentExtraLifeSpawnChance <= _currentPhase.ExtraLifeSpawnChance)
            {
                if (extraLifePrefab != null)
                {
                    GameObject spawnedExtraLife = Instantiate(extraLifePrefab, transform.position, Quaternion.identity);
                    spawnedExtraLife.transform.parent = obstacleParent;
                }  
            
                isExtraLifeSpawned = true;
            }
        }
    } 

    public void BossSpawn()
    {

        if (bossPrefab != null && !isBossCreated)
        {
            spawnedBoss = Instantiate(bossPrefab, new UnityEngine.Vector3(12, -3, 0), UnityEngine.Quaternion.identity);
            isBossCreated = true; 
            Debug.Log("[Spawner] Босс успешно заспавнен!");
        }
    }

    public float PhaseTimer => _phaseTimer;
    public bool IsExtraLifeSpawned => isExtraLifeSpawned; 

    private System.Func<float, float, float> _randomRange = Random.Range; 

    internal void SetRandomGeneratorForTest(System.Func<float, float, float> customRandom)
    {
        _randomRange = customRandom;
    }
}
