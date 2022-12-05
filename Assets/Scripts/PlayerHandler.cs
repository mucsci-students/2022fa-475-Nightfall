using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerHandler
{
    public static int _health { get; private set; }
    private static int _maxHealth;

    public static int _stamina { get; private set; }
    private static int _maxStamina;
    
    public static event EventHandler OnPlayerKilled;
    public static bool PlayerIsDead { get; private set; }

    public static void Initialize()
    {
        _maxHealth = 100;
        _health = _maxHealth;

        _maxStamina = 100;
        _stamina = _maxStamina;

        PlayerIsDead = false;
    }

    public static void AddValue(string field, int value)
    {
        if (String.Equals(field, "health"))
        {
            _health = Math.Clamp(_health + value, 0, _maxHealth);
            UIManager.UpdateValue(field, _health, _maxHealth);
            if (_health <= 0) 
            {
                PlayerIsDead = true;
                OnPlayerKilled?.Invoke(null, EventArgs.Empty);
            }
        }
        else if (String.Equals(field, "stamina"))
        {
            _stamina = Math.Clamp(_stamina + value, 0, _maxStamina);
            UIManager.UpdateValue(field, _stamina, _maxStamina);
        }
    }

    public static int GetValue(string field)
    {
        if (String.Equals(field, "health"))
        {
            return _health;
        }
        else if (String.Equals(field, "stamina"))
        {
            return _stamina;
        }

        return -1;
    }

    public static void SetVitality (int newHealth, int newStamina)
    {

        _health = newHealth;
        _stamina = newStamina;

    }
}
