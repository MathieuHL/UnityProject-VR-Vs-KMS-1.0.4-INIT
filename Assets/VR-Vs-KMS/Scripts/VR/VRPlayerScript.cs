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
    public GameObject canvas;
    public AudioClip soundFire, soundHit, soundDead, soundRespawn;

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

        --currentHealth;
        GetComponent<AudioSource>().PlayOneShot(soundHit);

        if (currentHealth <= 0)
        {
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
        if(!ShieldScript.isDestroyed)
            shieldGO.SetActive(!shieldGO.activeSelf);

        Debug.Log(ShieldScript.isDestroyed + "etat shield");
    }

    public IEnumerator Respawn()
    {
        yield return new WaitForSeconds(5f);

        transform.position = GameManager.Instance.spawnPoints[Random.Range(0, GameManager.Instance.spawnPoints.Length)].transform.position;
        currentHealth = maxHealth;
        canvas.SetActive(false);
        ShieldScript.isDestroyed = false;
        GetComponent<AudioSource>().PlayOneShot(soundRespawn);
    }
}
