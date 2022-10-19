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
        var hit = collider.gameObject;
        Debug.Log("Le NOOOOOOOOOOM " + hit.name);

        ThirdPersonScript vrPlayer = hit.GetComponent<ThirdPersonScript>();
        if (vrPlayer != null)
        {
            Debug.Log("It is a player !!");
            vrPlayer.HitByBall();
        }
        Destroy(gameObject);
    }
}
