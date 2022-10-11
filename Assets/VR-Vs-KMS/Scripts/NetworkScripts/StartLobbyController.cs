using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
    using Photon.Realtime;
using TMPro;

public class StartLobbyController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject _quickStartBtn, _quickCancelBtn;
    [SerializeField]
    private int _roomSize;
    [SerializeField]
    TMP_InputField usernameIF;

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        _quickStartBtn.SetActive(true);
    }
    
    public void QuickStart()
    {
        _quickStartBtn.SetActive(false);
        _quickCancelBtn.SetActive(true);
        PhotonNetwork.NickName = usernameIF.text;
        PhotonNetwork.JoinRandomRoom();
        Debug.Log("QuickStart");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join a room. Create one...");
        CreateRoom();
    }

    public void CreateRoom()
    {
        Debug.Log("Creating room now");
        int randomRoomNumber = Random.Range(0, 10);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)_roomSize };
        PhotonNetwork.CreateRoom("Room " + randomRoomNumber, roomOps);
        Debug.Log("The room number is " + randomRoomNumber);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to create a room... trying again");
        CreateRoom();
    }

    public void QuickCancel()
    {
        _quickCancelBtn.SetActive(false);
        _quickStartBtn.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }
}
