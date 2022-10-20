﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Cameras;
using UnityStandardAssets.Characters.ThirdPerson;

public class ThirdPersonScript : MonoBehaviourPunCallbacks
{
    public static GameObject UserMeInstance;
    public List<GameObject> pills;
    public Transform spawnPoint;

    public int maxHealth = 5, currentHealth;
    private float previousHealth;

    private float speed = 5f;
    private float firingSpeed = .5f;
    private float TimeBetweenBullet = 0f;

    public Slider slider, slider2;
    public GameObject deathScreen;

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
        maxHealth = GameManager.Instance.gameSetting.LifeNumber;
        currentHealth = maxHealth;
        slider.maxValue = maxHealth;
        slider.value = currentHealth;
        slider2.maxValue = maxHealth;
        slider2.value = currentHealth;

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
        SetHealth();

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
            if (Physics.Raycast(spawnPoint.position, spawnPoint.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
            {
                Vector3 shootingDirection = hit.point - spawnPoint.position;
                tempBullet.GetComponent<Rigidbody>().velocity = shootingDirection * speed;
            }
            else
            {
                tempBullet.GetComponent<Rigidbody>().velocity = spawnPoint.transform.forward * speed;
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
        SetHealth();
        deathScreen.SetActive(false);
        GetComponent<AudioSource>().PlayOneShot(soundRespawn);
    }

    public void SetHealth()
    {
        slider.value = (float)currentHealth;
        slider2.value = (float)currentHealth;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(currentHealth);
        }
        else
        {
            currentHealth = (int)stream.ReceiveNext();
        }

        if (previousHealth != currentHealth) SetHealth();
        previousHealth = currentHealth;
    }
}
