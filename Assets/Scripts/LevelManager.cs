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

    public static event Action<PhaseConfig> OnPhaseChanged; 
    public static event Action<float> OnTimerUpdated; 

    private float _timePassed = 0f;
    private bool _isPhaseActive = false; 

    private void OnEnable()
    {
        GameManager.onPlay += StartLevel;
        GameManager.onGameOver += StopLevel;
    } 

    private void OnDisable()
    {
        GameManager.onPlay -= StartLevel;
        GameManager.onGameOver -= StopLevel;
    }

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

    private void Update()
    {
        if (!_isPhaseActive)
            return;
        
        _timePassed += Time.deltaTime;
        if (_timePassed >= levelPhases[currentPhaseIndex].PhaseDuration)
        {
            AdvanceToNextPhase();
        }
    }

    public void StartLevel()
    {
        _isPhaseActive = true;
        _timePassed = 0f;
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

    public void StopLevel()
    {
        _isPhaseActive = false;
    }

    private void SetPhase(int index)
    {
        _timePassed = 0f;
        currentPhaseIndex = index;
        PhaseConfig currentConfig = levelPhases[currentPhaseIndex];

        OnPhaseChanged?.Invoke(currentConfig);

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
            StopLevel();
            Debug.Log("[LevelManager] Все фазы пройдены! Победа!");
        }
    }
}