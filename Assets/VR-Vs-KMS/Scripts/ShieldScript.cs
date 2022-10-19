using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldScript : MonoBehaviour
{
    public static int currentHealth = 5;
    public static bool isDestroyed = false;

    public void HitByBall()
    {
        --currentHealth;

        if (currentHealth <= 0)
        {
            Debug.Log(currentHealth + "vie shield");
            isDestroyed = true;
            gameObject.SetActive(false);
        }
    }
}
