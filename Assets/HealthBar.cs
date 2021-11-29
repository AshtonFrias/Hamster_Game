using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public int currentHealth = 10;
    public int maxHealth = 10;
    
    public void SetMaxHealth()
    {
        slider.maxValue = maxHealth;
        slider.value = currentHealth;
    }

    public void SetHealth()
    {
        slider.value = currentHealth;
    }

    public void reset()
    {
        currentHealth = maxHealth;
        slider.value = currentHealth;
    }
}
