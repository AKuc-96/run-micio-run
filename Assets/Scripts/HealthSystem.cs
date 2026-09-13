using System;
using UnityEngine;
using UnityEngine.Events;

public class HealthSystem
{
    public static HealthSystem Instance;
    public int CurrentLives { get; private set; }
    public int MaxLives { get; private set; }
    public int BonusCoins { get; private set; }

    public event Action<int> OnHealthChanged; 
    public event Action<int> OnAddBonuses;

    public HealthSystem (int initialLives = 1, int maxLives = 9)
    {
        MaxLives = maxLives; 
        ResetLives(initialLives);   
    }

    public void ResetLives(int count)
    {
        CurrentLives = Mathf.Clamp(count, 0, MaxLives); 
        OnHealthChanged?.Invoke(CurrentLives);
    }

    public void AddLife(int amount = 1)
    {
        bool healthChanged = false; 
        bool bonusesChanged = false; 

        for (int i = 0; i < amount; i++)
        {
            if (CurrentLives < MaxLives)
            {
                CurrentLives++; 
                healthChanged = true;
            }
            else
            {
                BonusCoins++; 
                bonusesChanged = true;
            }
        }

        if (healthChanged)
        {
            OnHealthChanged?.Invoke(CurrentLives);
        }

        if (bonusesChanged)
        {
            OnAddBonuses?.Invoke(BonusCoins);
        }
    }

    public void TakeDamage(int damage = 1)
    {
        CurrentLives = Mathf.Max(0, CurrentLives - damage);

        OnHealthChanged?.Invoke(CurrentLives);
    }
}