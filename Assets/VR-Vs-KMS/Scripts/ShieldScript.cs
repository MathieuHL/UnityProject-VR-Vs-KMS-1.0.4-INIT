using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldScript : MonoBehaviourPunCallbacks, IPunObservable
{
    public int currentHealth = 5;
    private int previousHealth;

    private void Start()
    {
        previousHealth = currentHealth;
    }

    public void HitByBall()
    {
        --currentHealth;

        if (currentHealth <= 0)
        {
            Debug.Log(currentHealth + "vie shield");
            Destroy(gameObject);
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

        previousHealth = currentHealth;
    }
}
