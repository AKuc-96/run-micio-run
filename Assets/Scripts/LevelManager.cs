using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Phase settings")]
    [SerializeField] private PhaseConfig[] levelPhases;

    private int currentPhaseIndex = 0;

    public GamePhase CurrentPhase { get; private set; }
    public float CurrentPhaseTimeRemaining { get; private set; }

    //public event Action<GamePhase> OnPhaseChanged; 
    public event Action<float> OnTimerUpdated; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartLevel();
    }

    public void StartLevel()
    {
        currentPhaseIndex = 0;

        if (levelPhases != null && levelPhases.Length > 0)
        {
            SetPhase(currentPhaseIndex);
        }
        else
        {
            Debug.LogError("[LevelManager] Массив levelPhases пуст!");
        }
    }

    private void SetPhase(int index)
    {
        currentPhaseIndex = index;
        PhaseConfig currentConfig = levelPhases[currentPhaseIndex];

        CurrentPhaseTimeRemaining = currentConfig.PhaseDuration;

        OnTimerUpdated?.Invoke(CurrentPhaseTimeRemaining);

        Debug.Log($"[LevelManager] Запущена фаза {currentPhaseIndex + 1}/{levelPhases.Length}: {currentConfig.name}");
    }

    public void AdvanceToNextPhase()
    {
        if (currentPhaseIndex + 1 < levelPhases.Length)
        {
            SetPhase(currentPhaseIndex + 1);
        }
        else
        {
            Debug.Log("[LevelManager] Все фазы пройдены! Победа!");
        }
    }
}