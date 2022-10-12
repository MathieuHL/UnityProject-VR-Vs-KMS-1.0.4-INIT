using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class PhotonTransformChilView : MonoBehaviourPunCallbacks, IPunObservable
{
    public bool synchronizePosition, synchronizeRotation, synchronizeScale;

    public List<Transform> SynchronizedChildTransform;
    private List<Vector3> localPositionList;
    private List<Quaternion> localRotationList;
    private List<Vector3> localScaleList;

    void Awake()
    {
        localPositionList = new List<Vector3>(SynchronizedChildTransform.Count);
        localRotationList = new List<Quaternion>(SynchronizedChildTransform.Count);
        localScaleList = new List<Vector3>(SynchronizedChildTransform.Count);
        foreach (Transform element in SynchronizedChildTransform)
        {
            localPositionList.Add(Vector3.zero);
            localRotationList.Add(Quaternion.identity);
            localScaleList.Add(Vector3.one);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < SynchronizedChildTransform.Count; i++)
        {
            if (synchronizePosition) SynchronizedChildTransform[i].localPosition = localPositionList[i];
            if (synchronizeRotation) SynchronizedChildTransform[i].localRotation = localRotationList[i];
            if (synchronizeScale) SynchronizedChildTransform[i].localScale = localScaleList[i];
        }
    }

    #region IPUnObservable
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            Debug.Log("Writing " + synchronizePosition + " " + synchronizeRotation);
            if (this.synchronizePosition)
            {
                for (int i = 0; i < SynchronizedChildTransform.Count; i++)
                {
                    stream.SendNext(SynchronizedChildTransform[i].localPosition);
                }
            }
            if (this.synchronizeRotation)
            {
                for (int i = 0; i < SynchronizedChildTransform.Count; i++)
                {
                    stream.SendNext(SynchronizedChildTransform[i].localRotation);
                }
            }

            if (this.synchronizeScale)
            {
                for (int i = 0; i < SynchronizedChildTransform.Count; i++)
                {
                    stream.SendNext(SynchronizedChildTransform[i].localScale);
                }
            }
        }
        else
        {
            if (this.synchronizePosition)
            {
                for (int i = 0; i < SynchronizedChildTransform.Count; i++)
                {
                    localPositionList[i] = (Vector3)stream.ReceiveNext();
                }
            }
            if (this.synchronizeRotation)
            {
                for (int i = 0; i < SynchronizedChildTransform.Count; i++)
                {
                    localRotationList[i] = (Quaternion)stream.ReceiveNext();
                }
            }

            if (this.synchronizeScale)
            {
                for (int i = 0; i < SynchronizedChildTransform.Count; i++)
                {
                    localScaleList[i] = (Vector3)stream.ReceiveNext();
                }
            }
        }
    }
    #endregion

}
