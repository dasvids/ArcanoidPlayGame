using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton

    private static GameManager _instance;

    public static GameManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else _instance = this;
    }
    #endregion
    
    public GameObject gameOverScreen;
    public GameObject victoryScreen;

    public bool IsGameStarted { get; set; }

    public int AvailibleLives = 3;
    public int Lives { get; set; }
    public static event Action<int> OnLiveLost;

    public int Score { get; set; }
    private void Start()
    {
        this.Lives = AvailibleLives;
        Ball.OnBallDeath += OnAllBallDeath;
        Brick.OnBrickDestruction += OnBrickDestruction;
    }

    private void OnBrickDestruction(Brick brick)
    {
        if (BricksManager.Instance.RemainingBricks.ToList().Where(brick => brick.sr.color != BricksManager.Instance.BrickColors[3]).Count() == 0)
        {
            BallsManager.Instance.ResetBalls();
            GameManager.Instance.IsGameStarted = false;
            BricksManager.Instance.LoadNextLevel();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void OnAllBallDeath(Ball ball)
    {
        if (BallsManager.Instance.Balls.Count <= 0)
        {
            this.Lives--;

            if (this.Lives < 1)
            {
                gameOverScreen.SetActive(true);
                //UIManager uiManager = FindObjectOfType<UIManager>();
                //string playerName = PlayerPrefs.GetString("PlayerName", "DefaultPlayer");
                //SaveResult();
                //MainSceneManager.Instance.AddPlayerToTop();
                //SceneManager.LoadScene("Main");
            }
            else
            {
                OnLiveLost?.Invoke(this.Lives);
                BallsManager.Instance.ResetBalls();
                IsGameStarted = false;
                BricksManager.Instance.LoadLevel(BricksManager.Instance.CurrentLevel);
            }
        }
    }

    private void OnDisable()
    {
        Ball.OnBallDeath -= OnAllBallDeath;
    }

    internal void ShowVictoryScreen()
    {
        victoryScreen.SetActive(true);
    }
}
