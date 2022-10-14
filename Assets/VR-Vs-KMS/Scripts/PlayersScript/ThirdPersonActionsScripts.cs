using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonActionsScripts : MonoBehaviourPunCallbacks
{
    public int tpLifePoint;
    public string tpColorShoot;

    private void Start()
    {
        tpLifePoint = GameManager.Instance.gameSetting.LifeNumber;
        tpColorShoot = GameManager.Instance.gameSetting.ColorShotKMS;
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
