using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class VRPlayerScript : MonoBehaviourPunCallbacks
{
    public Transform leftHand, rightHand;
    public GameObject ballPrefab, shieldGO;

    // Start is called before the first frame update
    void Start()
    {
        
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

    [PunRPC]
    void ThrowBall(PhotonMessageInfo info)
    {
        float lag = (float)(PhotonNetwork.Time - info.SentServerTime);

        var shot = Instantiate(ballPrefab, rightHand.position, rightHand.rotation);
        shot.transform.localPosition = new Vector3(0, 0, 0);

        var shotRb = shot.GetComponent<Rigidbody>();
        shotRb.velocity = (-rightHand.up + rightHand.forward) * 25f;

        Destroy(shot, 5.0f);
    }

    [PunRPC]
    void ChangeShieldState(PhotonMessageInfo info)
    {
        shieldGO.SetActive(!shieldGO.activeSelf);
    }
}
