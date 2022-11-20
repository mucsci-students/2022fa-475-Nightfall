using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    private int _health;
    private int _maxHealth;

    private int _stamina;
    private int _maxStamina;

    public Inventory plrInv;
    
    public void Initialize()
    {
        _maxHealth = 100;
        _health = _maxHealth;

        _maxStamina = 100;
        _stamina = _maxStamina;
        
        plrInv = this.AddComponent<Inventory>();
        plrInv.Initialize();

        Debug.Log(plrInv.GetTool("Sword"));
    }

    public void AddValue(UIManager ui, string field, int value)
    {
        if (String.Equals(field, "health"))
        {
            _health = Math.Clamp(_health + value, 0, _maxHealth);
            ui.UpdateValue(field, _health, _maxHealth);
        }
        else if (String.Equals(field, "stamina"))
        {
            _stamina = Math.Clamp(_stamina + value, 0, _maxStamina);
            ui.UpdateValue(field, _stamina, _maxStamina);
        }
    }

    public int GetValue(string field)
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
}
