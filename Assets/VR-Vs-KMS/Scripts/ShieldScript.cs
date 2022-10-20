using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldScript : MonoBehaviour
{
    public int currentHealth = 5;

    public void HitByBall()
    {
        --currentHealth;

        if (currentHealth <= 0)
        {
            Debug.Log(currentHealth + "vie shield");
            Destroy(gameObject);
        }
    }
}
