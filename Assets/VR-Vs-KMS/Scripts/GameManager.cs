using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public TextAsset jsonConfigFile;
    public GameState State;
    public JsonStructure gameSetting;
    public GameObject[] spawnPoints;
    public int tpsScore, vrScore;
    public string roomName;

    public static event Action<GameState> OnGameStateChanged;

    private void Awake()
    {
        Instance = this;

        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoints");

        UpdateGameState(GameState.Select);

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        CheckScore();
        Debug.Log(tpsScore + " tps");
        Debug.Log(vrScore + " vr");
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch (State)
        {
            case GameState.Select:
                SetGameSettings();
                break;
            case GameState.Playing:
                break;
            case GameState.Final:
                break;
        }

        OnGameStateChanged?.Invoke(newState);
    }

    public void SetGameSettings()
    {
        gameSetting = JsonUtility.FromJson<JsonStructure>(File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "GameConfig.json")));
        Debug.Log("test");
    }

    public void CheckScore()
    {
        if(vrScore > tpsScore && vrScore == 3)
        {
            UpdateGameState(GameState.Final);

            var listVrPlayer = GameObject.FindGameObjectsWithTag("VRPlayer");
            var listTpsPlayer = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject vrPlayer in listVrPlayer)
            {
                VRPlayerScript player = vrPlayer.GetComponent<VRPlayerScript>();
                player.victoryGo.SetActive(true);
                player.enabled = false;
            }

            foreach (GameObject tpsPlayer in listTpsPlayer)
            {
                ThirdPersonScript player = tpsPlayer.GetComponent<ThirdPersonScript>();
                player.loseGo.SetActive(true);
                player.enabled = false;
            }

            StartCoroutine(CloseRoomNetwork());
        }
        else if(vrScore < tpsScore && tpsScore == 3)
        {
            UpdateGameState(GameState.Final);

            var listVrPlayer = GameObject.FindGameObjectsWithTag("VRPlayer");
            var listTpsPlayer = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject vrPlayer in listVrPlayer)
            {
                VRPlayerScript player = vrPlayer.GetComponent<VRPlayerScript>();
                player.loseGo.SetActive(true);
                player.enabled = false;
            }

            foreach (GameObject tpsPlayer in listTpsPlayer)
            {
                ThirdPersonScript player = tpsPlayer.GetComponent<ThirdPersonScript>();
                player.victoryGo.SetActive(true);
                player.enabled = false;
            }

            StartCoroutine(CloseRoomNetwork());
        }
    }

    IEnumerator CloseRoomNetwork()
    {
        yield return new WaitForSeconds(10f);

        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("InitScene");
    }

    public enum GameState {
    Select,
    Playing,
    Final
}
}