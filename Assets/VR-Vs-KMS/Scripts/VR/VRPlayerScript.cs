using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class VRPlayerScript : MonoBehaviourPunCallbacks, IPunObservable
{
    public Transform leftHand, rightHand, spawnPoint, shieldPosition;
    public GameObject ballPrefab, shieldGO, shieldPrefab, victoryGo, loseGo;
    public int maxHealth = 5, currentHealth;
    public GameObject canvas;
    public AudioClip soundFire, soundHit, soundDead, soundRespawn;
    public Slider slider, slider2;

    private int previousHealth;

    private void Start()
    {
        maxHealth = GameManager.Instance.gameSetting.LifeNumber;
        currentHealth = maxHealth;
        slider.maxValue = maxHealth;
        slider.value = currentHealth;
        slider2.maxValue = maxHealth;
        slider2.value = currentHealth;

        if (!photonView.IsMine) slider2.gameObject.SetActive(false);

        shieldGO = Instantiate(shieldPrefab, shieldPosition);
        shieldGO.SetActive(false);
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

        if (GameManager.Instance.vrScore > GameManager.Instance.tpsScore && GameManager.Instance.vrScore == 3)
        {
            victoryGo.SetActive(true);
            StartCoroutine(GameManager.Instance.CloseRoomNetwork());
        }
        else if (GameManager.Instance.vrScore < GameManager.Instance.tpsScore && GameManager.Instance.tpsScore == 3)
        {
            loseGo.SetActive(true);
            StartCoroutine(GameManager.Instance.CloseRoomNetwork());
        }
    }

    public void HitByBall()
    {
        if (!photonView.IsMine) return;
        Debug.Log("Got me and health = " + currentHealth);

        --currentHealth;
        SetHealth();
        GetComponent<AudioSource>().PlayOneShot(soundHit);

        if (currentHealth <= 0)
        {
            photonView.RPC("UpdateScore", RpcTarget.AllViaServer);
            gameObject.transform.position = new Vector3(200, 200, 200);
            canvas.SetActive(true);
            GetComponent<AudioSource>().PlayOneShot(soundRespawn);
            StartCoroutine(Respawn());
        }
    }

    [PunRPC]
    void ThrowBall(PhotonMessageInfo info)
    {
        float lag = (float)(PhotonNetwork.Time - info.SentServerTime);

        var shot = Instantiate(ballPrefab, spawnPoint.position, spawnPoint.rotation);
        GetComponent<AudioSource>().PlayOneShot(soundFire);

        shot.GetComponent<Rigidbody>().velocity = (-spawnPoint.up + spawnPoint.forward) * 25f;

        shot.GetComponent<Rigidbody>().angularVelocity = new Vector3((Random.value - 0.5f) * 10000, (Random.value - 0.5f) * 10000, (Random.value - 0.5f) * 10000);

        Destroy(shot, 5.0f);
    }

    [PunRPC]
    void ChangeShieldState(PhotonMessageInfo info)
    {
        if(shieldGO!=null)
            shieldGO.SetActive(!shieldGO.activeSelf);
    }

    public IEnumerator Respawn()
    {
        yield return new WaitForSeconds(5f);

        transform.position = GameManager.Instance.spawnPoints[Random.Range(0, GameManager.Instance.spawnPoints.Length)].transform.position;
        currentHealth = maxHealth;
        SetHealth();
        canvas.SetActive(false);
        GetComponent<AudioSource>().PlayOneShot(soundRespawn);
        shieldGO = Instantiate(shieldPrefab, shieldPosition);
        shieldGO.SetActive(false);
    }

    [PunRPC]
    void UpdateScore(PhotonMessageInfo infp)
    {
        GameManager.Instance.tpsScore++;

        
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
