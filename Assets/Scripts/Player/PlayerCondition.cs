using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(int damage);
}

public class PlayerCondition : MonoBehaviour , IDamageable
{
    public UICondition condition;

    Condition health { get { return condition.health; } }
    Condition stamina { get { return condition.stamina; } }

    public event Action onTakeDamage;

    // Update is called once per frame
    void Update()
    {
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        if(health.currentValue <= 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public bool UseStamina(float amount)
    {
        if(stamina.currentValue - amount < 0)
        {
            return false;
        }
        stamina.Subtract(amount);
        return true;
    }

    public void TakeDamage(int damage)
    {
        health.Subtract(damage);
        onTakeDamage?.Invoke();
    }

    public void Die()
    {
        Debug.Log("»ç¸Á");
    }
}
