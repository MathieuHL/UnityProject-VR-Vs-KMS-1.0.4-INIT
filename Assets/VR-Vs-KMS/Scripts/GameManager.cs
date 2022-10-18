using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public TextAsset jsonConfigFile;
    public GameState State;
    public JsonStructure gameSetting;
    public GameObject[] spawnPoints;

    public static event Action<GameState> OnGameStateChanged;

    private void Awake()
    {
        Instance = this;

        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoints");

        UpdateGameState(GameState.Select);

        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        
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
            case GameState.Lose:
                break;
            case GameState.Victory:
                break;
        }

        OnGameStateChanged?.Invoke(newState);
    }

    public void SetGameSettings()
    {
        gameSetting = JsonUtility.FromJson<JsonStructure>(File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "GameConfig.json")));
        Debug.Log("test");
    }

    public enum GameState {
    Select,
    Playing,
    Lose,
    Victory
}
}