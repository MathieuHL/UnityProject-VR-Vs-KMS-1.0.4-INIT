using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class StartRoomController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private int multiplayerSceneIndex;

    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined a Room");
        StartGame();
    }

    private void StartGame()
    {
        Debug.Log("fonction start");
        Debug.Log(PhotonNetwork.MasterClient);
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Starting Game");
            PhotonNetwork.LoadLevel(multiplayerSceneIndex);
        }
    }
}
