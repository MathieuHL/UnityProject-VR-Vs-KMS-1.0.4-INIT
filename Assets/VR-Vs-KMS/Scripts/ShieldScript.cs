using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldScript : MonoBehaviourPunCallbacks, IPunObservable
{
    public int currentHealth = 5;
    private int previousHealth;

    public void HitByBall()
    {
        --currentHealth;

        if (currentHealth <= 0)
        {
            Debug.Log(currentHealth + "vie shield");
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "pills")
        {
            Destroy(other);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(currentHealth);
        }
        else
        {
            currentHealth = (int)stream.ReceiveNext();
        }

        if(previousHealth != currentHealth)
            previousHealth = currentHealth;

        Debug.Log("vie shield " + currentHealth);
    }
}
