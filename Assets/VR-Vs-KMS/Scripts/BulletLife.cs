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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name != "PhotonPlayer(Clone)")
            Destroy(gameObject);

        var hit = collision.gameObject;
        Debug.Log("Snowball hit something:" + hit);

        VRPlayerScript vrPlayer = hit.GetComponent<VRPlayerScript>();
        if (vrPlayer != null)
        {
            Debug.Log("It is a player !!");
            vrPlayer.HitByBall();
        }
        Destroy(gameObject);
    }
}
