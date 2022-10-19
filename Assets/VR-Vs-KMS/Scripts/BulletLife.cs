using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLife : MonoBehaviour
{
    //private float gravity = 1f;

    // Start is called before the first frame update
    void Start()
    {
        //Delete the bullet after n second(s)
        Destroy(gameObject, 3);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag != "Player")
            Destroy(gameObject);

        var hit = collider.gameObject;

        VRPlayerScript vrPlayer = hit.GetComponent<VRPlayerScript>();
        if (vrPlayer != null)
        {
            vrPlayer.HitByBall();
            Destroy(gameObject);
        }

        ShieldScript shieldScript = hit.GetComponent<ShieldScript>();
        if (shieldScript != null)
        {
            Debug.Log("It is a ashield !!");
            shieldScript.HitByBall();
            Destroy(gameObject);
        }
        if (hit.name == "Shield")
            Destroy(gameObject);
        Debug.Log("los tag + " + collider.tag);
    }
}
