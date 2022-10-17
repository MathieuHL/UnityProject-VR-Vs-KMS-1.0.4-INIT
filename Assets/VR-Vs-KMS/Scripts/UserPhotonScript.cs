using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Cameras;
using UnityStandardAssets.Characters.ThirdPerson;

public class UserPhotonScript : MonoBehaviourPunCallbacks
{
    public static GameObject UserMeInstance;
    public List<GameObject> pills;
    public Transform spawnPoint;
    private float speed = 25f;
    private float firingSpeed = 0.2f;
    private float TimeBetweenBullet = 0f;

    public Material PlayerLocalMat;
    public GameObject GameObjectLocalPlayerColor;

    /// <summary>
    /// The FreeLookCameraRig GameObject to configure for the UserMe
    /// </summary>
    GameObject goFreeLookCameraRig = null;

    void Awake()
    {
        if (photonView.IsMine)
        {
            Debug.LogFormat("Avatar UserMe created for userId {0}", photonView.ViewID);
            UserMeInstance = gameObject;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("isLocalPlayer:" + photonView.IsMine);
        updateGoFreeLookCameraRig();
        followLocalPlayer();
        activateLocalPlayer();
    }

    private void Update()
    {
        if (!photonView.IsMine)
            return;

        TimeBetweenBullet += Time.deltaTime;

        if (Input.GetMouseButton(0) && TimeBetweenBullet > firingSpeed)
        {
            photonView.RPC("SpawnBullet", RpcTarget.AllViaServer);
        }
    }
    /// <summary>
    /// Get the GameObject of the CameraRig
    /// </summary>
    protected void updateGoFreeLookCameraRig()
    {
        if (!photonView.IsMine) return;

        try
        {
            goFreeLookCameraRig = GameObject.FindGameObjectWithTag("freeLookCam");
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning("Warning, no goFreeLookCameraRig found\n" + ex);
        }
    }

    /// <summary>
    /// Make the CameraRig following the LocalPlayer only.
    /// </summary>
    protected void followLocalPlayer()
    {
        if (photonView.IsMine)
        {
            if (goFreeLookCameraRig != null)
            {
                // find Avatar EthanHips
                Transform transformFollow = GameObject.FindGameObjectWithTag("ethanHips").transform;
                goFreeLookCameraRig.GetComponent<FreeLookCam>().SetTarget(transformFollow);
                Debug.Log("ThirdPersonControllerMultiuser follow:" + transformFollow);
            }
        }
    }

    protected void activateLocalPlayer()
    {
        // enable the ThirdPersonUserControl if it is a Loacl player = UserMe
        // disable the ThirdPersonUserControl if it is not a Loacl player = UserOther

        GetComponent<ThirdPersonUserControl>().enabled = photonView.IsMine;
        GetComponent<Rigidbody>().isKinematic = !photonView.IsMine;

        if (photonView.IsMine)
        {
            try
            {
                GetComponent<ThirdPersonUserControl>();
            }
            catch (System.Exception)
            {
                GameObjectLocalPlayerColor.GetComponent<Renderer>().material = PlayerLocalMat;
            }
        }
    }

    [PunRPC]
    void SpawnBullet(PhotonMessageInfo info)
    {
        var tempBullet = Instantiate(pills[Random.Range(0, pills.Count)], spawnPoint.position, goFreeLookCameraRig.transform.rotation);
        tempBullet.GetComponent<Rigidbody>().velocity = -goFreeLookCameraRig.transform.forward * speed;
        tempBullet.GetComponent<Rigidbody>().angularVelocity = new Vector3((Random.value - 0.5f) * 10000, (Random.value - 0.5f) * 10000, (Random.value - 0.5f) * 10000);
        TimeBetweenBullet = 0;
    }
}
