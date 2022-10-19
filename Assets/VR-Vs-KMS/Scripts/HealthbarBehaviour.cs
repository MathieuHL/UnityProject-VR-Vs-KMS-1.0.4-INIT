using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarBehaviour : MonoBehaviour
{
    public Slider slider;

    /// <summary>
    /// Set max health and reset current health to the max value
    /// </summary>
    /// <param name="health"></param>
    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    /// <summary>
    /// Set current health
    /// </summary>
    /// <param name="health"></param>
    public void SetHealth(float health)
    {
        slider.value = health;
    }

    /// <summary>
    /// Return current health
    /// </summary>
    /// <returns></returns>
    public float GetHealth()
    {
        return slider.value;
    }
}
