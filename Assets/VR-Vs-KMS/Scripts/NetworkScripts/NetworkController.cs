using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkController : MonoBehaviourPunCallbacks
{
    public GameObject pcPrefab, vrPrefab;
    public static NetworkController Instance = null;

    // Start is called before the first frame update
    void Start()
    {
        //PhotonNetwork.ConnectUsingSettings(); // Connect to Photon Master
        UserDeviceManager.GetPrefabToSpawnWithDeviceUsed(pcPrefab, vrPrefab);
        
        Instance = this;
        #region TO debug
        Debug.Log("device:" + UserDeviceManager.GetDeviceUsed());
        Debug.Log("prefab:" + UserDeviceManager.GetPrefabToSpawnWithDeviceUsed(pcPrefab, vrPrefab));
        #endregion

        GameObject playerPrefab = UserDeviceManager.GetPrefabToSpawnWithDeviceUsed(pcPrefab, vrPrefab);

        if (playerPrefab == null)
        {
            Debug.LogErrorFormat("<Color=Red><a>Missing</a></Color> playerPrefab Reference for device {0}. Please set it up in GameObject 'NetworkManager'", UserDeviceManager.GetDeviceUsed());
        }
        else
        {
            // TODO: Instantiate the prefab representing my own avatar only if it is UserMe
            if (ThirdPersonScript.UserMeInstance == null)
            {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                Vector3 initialPos = UserDeviceManager.GetDeviceUsed() == UserDeviceType.HTC ? new Vector3(0f, 0f, 0f) : new Vector3(0f, 5f, 0f);
                PhotonNetwork.Instantiate("PhotonPrefabs/" + playerPrefab.name, initialPos, Quaternion.identity, 0);
            }
            else
            {
                Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
            }
        }
        
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
