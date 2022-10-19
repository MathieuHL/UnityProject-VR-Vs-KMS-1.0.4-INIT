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
        Debug.Log("Le NOOOOOOOOOOM " + hit.name);

        VRPlayerScript vrPlayer = hit.GetComponent<VRPlayerScript>();
        if (vrPlayer != null)
        {
            Debug.Log("It is a player !!");
            vrPlayer.HitByBall();
        }

        ShieldScript shieldScript = hit.GetComponent<ShieldScript>();
        if (shieldScript != null)
        {
            Debug.Log("It is a player !!");
            shieldScript.HitByBall();
        }
        Destroy(gameObject);
    }
}
