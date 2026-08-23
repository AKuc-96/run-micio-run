using System;
using UnityEngine;
using UnityEngine.Events;

public class HealthSystem
{
    public static HealthSystem Instance;
    public int CurrentLives { get; private set; }
    public int MaxLives { get; private set; }
    public int BonusCoins { get; private set; }

    public UnityEvent<int> onHealthChanged = new (); 
    public UnityEvent<int> onAddBonuses = new ();

    public HealthSystem (int initialLives = 1, int maxLives = 9)
    {
        MaxLives = maxLives; 
        ResetLives(initialLives);   
    }

    public void ResetLives(int count)
    {
        CurrentLives = Mathf.Clamp(count, 0, MaxLives); 
        onHealthChanged?.Invoke(CurrentLives);
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
            onHealthChanged?.Invoke(CurrentLives);
        }

        if (bonusesChanged)
        {
            onAddBonuses?.Invoke(BonusCoins);
        }
    }

    public void TakeDamage(int damage = 1)
    {
        CurrentLives = Mathf.Max(0, CurrentLives - damage);

        onHealthChanged?.Invoke(CurrentLives);
    }
}