using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityStandardAssets.Cameras;
using UnityStandardAssets.Characters.ThirdPerson;

public class ThirdPersonScript : MonoBehaviourPunCallbacks
{
    public static GameObject UserMeInstance;
    public List<GameObject> pills;
    public Transform spawnPoint;
	
    public int maxHealth = 1, currentHealth;
    public TMP_Text healthText, currentHealthText;

    private float speed = 5f;
    private float firingSpeed = .5f;
    private float TimeBetweenBullet = 0f;

    public Material PlayerLocalMat;
    public GameObject GameObjectLocalPlayerColor, deathScreen;

    public AudioClip soundFire, soundHit, soundDead, soundRespawn;

    private RaycastHit hit;

    /// <summary>
    /// The FreeLookCameraRig GameObject to configure for the UserMe
    /// </summary>
    GameObject goFreeLookCameraRig = null;
    GameObject mainCamera;

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

        maxHealth = GameManager.Instance.gameSetting.LifeNumber;
        currentHealth = maxHealth;

        Debug.Log("currentHealt = " + currentHealth);
        Debug.Log("max Health = " + maxHealth);

        updateGoFreeLookCameraRig();
        followLocalPlayer();
        activateLocalPlayer();
    }

    private void Update()
    {
        if (!photonView.IsMine)
            return;

        if (Input.GetMouseButton(0))
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
            mainCamera = goFreeLookCameraRig.transform.Find("Pivot").Find("MainCamera").gameObject;
            Debug.Log("Main camera = " + mainCamera);
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

    public void HitByBall()
    {
        if (!photonView.IsMine) return;
        Debug.Log("Got me and health = " + currentHealth);

        // Manage to leave room as UserMe

        --currentHealth;
        GetComponent<AudioSource>().PlayOneShot(soundHit);

        if (currentHealth <= 0)
        {
            gameObject.transform.position = new Vector3(200, 200, 200);
            deathScreen.SetActive(true);
            GetComponent<AudioSource>().PlayOneShot(soundDead);
            StartCoroutine(Respawn());
        }
    }

    [PunRPC]
    void SpawnBullet(PhotonMessageInfo info)
    {
        TimeBetweenBullet += Time.deltaTime;
        if (TimeBetweenBullet > firingSpeed)
        {
            //Projectile initialisation
            var tempBullet = Instantiate(pills[Random.Range(0, pills.Count)], spawnPoint.position, spawnPoint.transform.rotation);
            GetComponent<AudioSource>().PlayOneShot(soundFire);

            //Shoot from the player to the RaycastHit from the camera
            if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
            {
                Vector3 shootingDirection = hit.point - spawnPoint.position;
                tempBullet.GetComponent<Rigidbody>().velocity = shootingDirection * speed;
            }
            else
            {
                tempBullet.GetComponent<Rigidbody>().velocity = mainCamera.transform.forward * speed;
            }

            //Add some rotation to the projectile
            tempBullet.GetComponent<Rigidbody>().angularVelocity = new Vector3((Random.value - 0.5f) * 10000, (Random.value - 0.5f) * 10000, (Random.value - 0.5f) * 10000);
            TimeBetweenBullet = 0;
        }
    }

    public IEnumerator Respawn()
    {
        yield return new WaitForSeconds(5f);

        transform.position = GameManager.Instance.spawnPoints[Random.Range(0, GameManager.Instance.spawnPoints.Length)].transform.position;
        currentHealth = maxHealth;
        deathScreen.SetActive(false);
        GetComponent<AudioSource>().PlayOneShot(soundRespawn);
    }
}
