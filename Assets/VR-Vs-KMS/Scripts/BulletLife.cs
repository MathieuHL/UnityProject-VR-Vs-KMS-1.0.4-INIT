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

    // Update is called once per frame
    void Update()
    {
        //gameObject.transform.position += new Vector3(0, -(gravity * Time.deltaTime), 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //TODO: Retirer les points de vie du virus
    }
}
