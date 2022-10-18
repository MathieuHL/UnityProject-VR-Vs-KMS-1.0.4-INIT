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

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name != "Player2")
            Destroy(gameObject);

        var hit = collider.gameObject;

        Debug.Log("La boule a touché");
        ThirdPersonScript tpsPlayer = hit.GetComponent<ThirdPersonScript>();
        if (tpsPlayer != null)
        {
            Debug.Log("It is a player !!");
            tpsPlayer.HitByBall();
        }
        Destroy(gameObject);
    }
}
