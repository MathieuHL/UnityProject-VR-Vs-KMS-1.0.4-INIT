using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameSetupController : MonoBehaviourPunCallbacks
{
    public GameObject prefabPlayer;
    public static GameSetupController Instance = null;

    private void Start()
    {
        CreatePlayer();
    }

    private void CreatePlayer()
    {
        Instance = this;

        if (prefabPlayer == null) 
        {             
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefabPC Reference. Please set it up in GameObject 'Game Manager'", this);
        }
        else
        {
            if (UserPhotonScript.UserMeInstance == null)
            {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", prefabPlayer.name), Vector3.zero, Quaternion.identity, 0);
            }
            else
            {
                Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
            }
        }
    }
}
