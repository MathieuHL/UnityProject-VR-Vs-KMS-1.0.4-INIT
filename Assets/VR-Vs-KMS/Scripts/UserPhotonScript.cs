using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class UserPhotonScript : MonoBehaviourPunCallbacks
{
    public static GameObject UserMeInstance;

    // Start is called before the first frame update
    void Awake()
    {
        if (photonView.IsMine)
        {
            Debug.LogFormat("Avatar UserMe created for userId {0}", photonView.ViewID);
            UserMeInstance = gameObject;

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
