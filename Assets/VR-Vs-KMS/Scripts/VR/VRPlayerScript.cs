using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Valve.VR;

public class VRPlayerScript : MonoBehaviourPunCallbacks
{
    public Transform leftHand, rightHand, spawnPoint;
    public GameObject ballPrefab, shieldGO;
    public int maxHealth = 1, currentHealth;
    public TMP_Text healthText, currentHealthText;

    private void Start()
    {
        maxHealth = GameManager.Instance.gameSetting.LifeNumber;
        currentHealth = maxHealth;
        Debug.Log("currentHealt = " + currentHealth);
        Debug.Log("max Health = " + maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;

        if (SteamVR_Actions.default_GrabPinch.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            photonView.RPC("ThrowBall", RpcTarget.AllViaServer);
        }

        if (SteamVR_Actions.default_GrabPinch.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            photonView.RPC("ChangeShieldState", RpcTarget.AllViaServer);
        }

        if (SteamVR_Actions.default_GrabPinch.GetStateUp(SteamVR_Input_Sources.LeftHand))
        {
            photonView.RPC("ChangeShieldState", RpcTarget.AllViaServer);
        }
    }

    public void HitByBall()
    {
        if (!photonView.IsMine) return;
        Debug.Log("Got me and health = " + currentHealth);

        // Manage to leave room as UserMe
        if (--currentHealth <= 0)
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    [PunRPC]
    void ThrowBall(PhotonMessageInfo info)
    {
        float lag = (float)(PhotonNetwork.Time - info.SentServerTime);

        var shot = Instantiate(ballPrefab, spawnPoint.position, spawnPoint.rotation);
        shot.transform.localPosition = new Vector3(0, 0, 0);

        var shotRb = shot.GetComponent<Rigidbody>();
        shotRb.velocity = (-spawnPoint.up + spawnPoint.forward) * 25f;

        Destroy(shot, 5.0f);
    }

    [PunRPC]
    void ChangeShieldState(PhotonMessageInfo info)
    {
        shieldGO.SetActive(!shieldGO.activeSelf);
    }
}
