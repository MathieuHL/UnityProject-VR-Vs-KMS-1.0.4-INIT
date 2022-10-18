using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusBulletScript : MonoBehaviour
{
    void Start()
    {
        //Delete the bullet after n second(s)
        Destroy(gameObject, 3);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name != "Player")
            Destroy(gameObject);

        var hit = collision.gameObject;

        ThirdPersonScript tpsPlayer = hit.GetComponent<ThirdPersonScript>();
        if (tpsPlayer != null)
        {
            Debug.Log("It is a player !!");
            tpsPlayer.HitByBall();
        }
        Destroy(gameObject);
    }
}
