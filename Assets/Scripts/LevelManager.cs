using System;
using System.Net.NetworkInformation;
using System.Runtime.Serialization;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Timer settings")]
    [SerializeField] private float warmupDuration = 5f; 
    [SerializeField] private float preBossDuration = 30f; 
    [SerializeField] private float postBossDuration = 5f; 
    [SerializeField] private float finalRunDuration = 15f;

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
        if (CurrentPhase == GamePhase.BossFight || CurrentPhase == GamePhase.Victory)
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
                ChangePhase(GamePhase.PreBoss);
                break;
            case GamePhase.PreBoss:
                ChangePhase(GamePhase.BossFight);
                break;
            case GamePhase.PostBoss:
                ChangePhase(GamePhase.FinalRun);
                break;
            case GamePhase.FinalRun:
                ChangePhase(GamePhase.Victory);
                break;
        }
    }

    private float GetDurationForPhase(GamePhase phase)
    {
        return phase switch
        {
            GamePhase.Warmup => warmupDuration, 
            GamePhase.PreBoss => preBossDuration, 
            GamePhase.PostBoss => postBossDuration, 
            GamePhase.FinalRun => finalRunDuration, 
            _ => 0f
        };
    }
}