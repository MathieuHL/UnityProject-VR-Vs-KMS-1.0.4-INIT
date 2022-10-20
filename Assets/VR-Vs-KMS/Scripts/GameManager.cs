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
    }

    public IEnumerator CloseRoomNetwork()
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