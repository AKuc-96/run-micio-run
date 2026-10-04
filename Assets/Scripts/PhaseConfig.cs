using UnityEngine; 

[CreateAssetMenu(fileName = "NewPhaseConfig", menuName = "Config/PhaseConfig" )]
public class PhaseConfig : ScriptableObject
{
    [Header("Obstacle spawn")]
    [SerializeField] private float spawnInterval;
    [SerializeField] private GameObject[] obstaclePrefabs;

    [Header("Boss settings")]
    [SerializeField] private GameObject bossPrefab; 
    [SerializeField] private bool isBossPhase;

    [Header("Duration")]
    [SerializeField] private float phaseDuration; 

    [SerializeField] private float obstacleSpeed = 5f; 
    [Header("Extra life spawning")]
    [SerializeField] private bool canSpawnExtraLife;
    [SerializeField] private float extraLifeSpawnChance = 20f;

    public float SpawnInterval => spawnInterval;
    public GameObject[] ObstaclePrefabs => obstaclePrefabs;
    public GameObject BossPrefab => bossPrefab;
    public float PhaseDuration => phaseDuration; 
    public float ObstacleSpeed => obstacleSpeed; 
    public bool IsBossPhase => isBossPhase; 
    public bool CanSpawnExtraLife => canSpawnExtraLife;
    public float ExtraLifeSpawnChance => extraLifeSpawnChance;

    internal void InitForTest(float interval, bool isBoss, float phaseIsLong = 10f)
    {
        spawnInterval = interval;
        isBossPhase = isBoss;
        phaseDuration = phaseIsLong;
    }
}