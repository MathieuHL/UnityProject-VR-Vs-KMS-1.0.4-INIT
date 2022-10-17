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
            photonView.RPC("ThrowBall", RpcTarget.AllViaServer, rightHand.position, (-rightHand.up + rightHand.forward) * 25f);
        }

        if (SteamVR_Actions.default_GrabPinch.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            shieldGO.SetActive(true);
        }

        if (SteamVR_Actions.default_GrabPinch.GetStateUp(SteamVR_Input_Sources.LeftHand))
        {
            shieldGO.SetActive(false);
        }
    }

    [PunRPC]
    void ThrowBall(Vector3 position, Vector3 directionAndSpeed, PhotonMessageInfo info)
    {
        float lag = (float)(PhotonNetwork.Time - info.SentServerTime);

        var shot = Instantiate(ballPrefab, position * Mathf.Clamp(lag, 0, 1.0f), rightHand.rotation);
        shot.transform.localPosition = new Vector3(0, 0, 0);

        var shotRb = shot.GetComponent<Rigidbody>();
        shotRb.velocity = directionAndSpeed;

        Destroy(shot, 5.0f);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            Vector3 pos = transform.localPosition;
            stream.Serialize(ref pos);
        }
        else
        {
            Vector3 pos = Vector3.zero;
            stream.Serialize(ref pos);  // pos gets filled-in. must be used somewhere
        }
    }
}
