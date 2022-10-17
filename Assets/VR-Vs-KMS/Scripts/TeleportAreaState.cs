using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportAreaState : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Teleporting"))
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
