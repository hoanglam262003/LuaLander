using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static void ResetStaticData()
    {
        levelNumber = 1;
        totalScore = 0;
    }

    public event EventHandler onPause;
    public event EventHandler onResume;

    private static int levelNumber = 1;
    private static int totalScore = 0;
    [SerializeField] private List<GameLevel> gameLevelList;
    [SerializeField] private CinemachineCamera cinemachineCamera;
    private int score;
    private float time;
    private bool isTimerActive;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        Lander.Instance.OnCoinPickUp += Lander_OnCoinPickUp;
        Lander.Instance.OnLanded += Lander_OnLanded;
        Lander.Instance.OnStateChanged += Lander_OnStateChanged;
        GameInput.Instance.OnMenuButtonPressed += GameInput_OnMenuButtonPressed;
        LoadCurrentLevel();
    }

    private void GameInput_OnMenuButtonPressed(object sender, EventArgs e)
    {
        PauseResumeGame();
    }

    private void Update()
    {
        if (isTimerActive)
        {
            time += Time.deltaTime;
        }
    }

    public void AddScore(int scoreAmount)
    {
        score += scoreAmount;
    }

    private void Lander_OnCoinPickUp(object sender, System.EventArgs e)
    {
        AddScore(100);
    }

    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        AddScore(e.score);
    }

    private void Lander_OnStateChanged(object sender, Lander.OnStateChangedEventArgs e)
    {
        isTimerActive = e.state == Lander.State.Normal;

        if (e.state == Lander.State.Normal)
        {
            cinemachineCamera.Target.TrackingTarget = Lander.Instance.transform;
            CinemachineZoomCamera.Instance.ResetToNormalOrthographicSize();
        }
    }

    public int GetScore()
    {
        return score;
    }

    public float GetTime()
    {
        return time;
    }

    private void LoadCurrentLevel()
    {
        GameLevel level = GetGameLevel();
        GameLevel spawnGameLevel = Instantiate(level, Vector3.zero, Quaternion.identity);
        Lander.Instance.transform.position = spawnGameLevel.GetLanderStartPosition();
        cinemachineCamera.Target.TrackingTarget = spawnGameLevel.GetCameraStartTarget();
        CinemachineZoomCamera.Instance.SetTargetOrthographicSize(spawnGameLevel.GetZoomedOutOrthographicSize());
    }

    private GameLevel GetGameLevel()
    {
        foreach (GameLevel level in gameLevelList)
        {
            if (level.GetLevelNumber() == levelNumber)
            {
                return level;
                
            }
        }
        return null;
    }

    public int GetTotalScore()
    {
        return totalScore;
    }

    public void NextLevel()
    {
        levelNumber++;
        totalScore += score;

        if (GetGameLevel() == null)
        {
            SceneLoader.LoadScene(SceneLoader.Scene.GameOverScene);
        } else
        {
            SceneLoader.LoadScene(SceneLoader.Scene.GameScene);
        }
    }

    public void RetryLevel()
    {
        SceneLoader.LoadScene(SceneLoader.Scene.GameScene);
    }

    public int GetLevelNumber()
    {
        return levelNumber;
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        onPause?.Invoke(this, EventArgs.Empty);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        onResume?.Invoke(this, EventArgs.Empty);
    }

    public void PauseResumeGame()
    {
        if (Time.timeScale == 1f)
        {
            PauseGame();
        } else
        {
            ResumeGame();
        }
    }
}
