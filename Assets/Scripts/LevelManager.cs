using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Timer settings")]
    [SerializeField] private float warmupDuration = 60f; 
    [SerializeField] private float firstBossDuration = 30f; 
    [SerializeField] private float firstPostBossDuration = 90f;
    [SerializeField] private float secondBossDuration = 30f;
    [SerializeField] private float finalRunDuration = 60f; 
    [SerializeField] private float thirdBossDuration = 30f;

    public GamePhase CurrentPhase { get; private set; }
    public float CurrentPhaseTimeRemaining { get; private set; }

    public event Action<GamePhase> OnPhaseChanged; 
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

    private void Update()
    {
        UpdatePhaseTimer();
    }

    public void StartLevel()
    {
        ChangePhase(GamePhase.Warmup);
    }

    private void UpdatePhaseTimer()
    {
        if (CurrentPhase == GamePhase.Victory)
            return;

        if (CurrentPhaseTimeRemaining > 0)
        {
            CurrentPhaseTimeRemaining -= Time.deltaTime;
            OnTimerUpdated?.Invoke(CurrentPhaseTimeRemaining);

            if (CurrentPhaseTimeRemaining <= 0)
            {
                CurrentPhaseTimeRemaining = 0;
                OnTimerUpdated?.Invoke(CurrentPhaseTimeRemaining);
                AdvanceToNextPhase();
            }
        }
    }

    public void ChangePhase(GamePhase newPhase)
    {
        CurrentPhase = newPhase;
        CurrentPhaseTimeRemaining = GetDurationForPhase(newPhase);

        OnPhaseChanged?.Invoke(CurrentPhase);
        OnTimerUpdated?.Invoke(CurrentPhaseTimeRemaining);

        Debug.Log($"[LevelManager] New phase: {CurrentPhase}");
    }

    private void AdvanceToNextPhase()
    {
        switch (CurrentPhase)
        {
            case GamePhase.Warmup: 
                ChangePhase(GamePhase.FirstBoss);
                break;
            case GamePhase.FirstBoss:
                ChangePhase(GamePhase.FirstPostBoss);
                break;
            case GamePhase.FirstPostBoss:
                ChangePhase(GamePhase.SecondBoss);
                break;
            case GamePhase.SecondBoss:
                ChangePhase(GamePhase.FinalRun);
                break;
            case GamePhase.FinalRun:
                ChangePhase(GamePhase.ThirdBoss);
                break;
            case GamePhase.ThirdBoss:
                ChangePhase(GamePhase.Victory);
                break;
        }
    }

    private float GetDurationForPhase(GamePhase phase)
    {
        return phase switch
        {
            GamePhase.Warmup => warmupDuration, 
            GamePhase.FirstBoss => firstBossDuration, 
            GamePhase.FirstPostBoss => firstPostBossDuration, 
            GamePhase.SecondBoss => secondBossDuration,
            GamePhase.FinalRun => finalRunDuration, 
            GamePhase.ThirdBoss => thirdBossDuration, 
            _ => 0f
        };
    }
}