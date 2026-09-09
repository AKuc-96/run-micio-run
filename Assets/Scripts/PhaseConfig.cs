using UnityEngine; 

[CreateAssetMenu(fileName = "NewPhaseConfig", menuName = "Config/PhaseConfig" )]
public class PhaseConfig : ScriptableObject
{
    [Header("Obstacle spawn")]
    [SerializeField] private float spawnInterval;
    [SerializeField] private GameObject[] obstaclePrefabs;

    [Header("Boss settings")]
    [SerializeField] private GameObject bossPrefab;

    [Header("Duration")]
    [SerializeField] private float phaseDuration;

    public float SpawnInterval => spawnInterval;
    public GameObject[] ObstaclePrefabs => obstaclePrefabs;
    public GameObject BossPrefab => bossPrefab;
    public float PhaseDuration => phaseDuration;
}