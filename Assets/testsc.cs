using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class testsc : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); // Connect to Photon Master
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
