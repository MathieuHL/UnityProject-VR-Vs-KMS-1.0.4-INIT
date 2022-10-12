using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkController : MonoBehaviourPunCallbacks
{
    public GameObject pcPrefab, vrPrefab;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); // Connect to Photon Master
        UserDeviceManager.GetPrefabToSpawnWithDeviceUsed(pcPrefab, vrPrefab);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("We are now connected to the " + PhotonNetwork.CloudRegion + " server !");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
