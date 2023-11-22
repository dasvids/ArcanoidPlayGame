using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public GameObject pauseScreen;

    public bool IsGameStarted { get; set; }
    private bool isPaused { get; set; }

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

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            Time.timeScale = 0f;
            pauseScreen.SetActive(true);
        } 
        else 
        {
            Time.timeScale = 1f;
            pauseScreen.SetActive(false);
        }
    }

    public void CloseApp()
    {
        Application.Quit();
    }

    public void OnBackButtonClicked()
    {
        SceneManager.LoadScene("Main");
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
                List<Player> players = MainSceneManager.Instance.LoadPlayers();
                List<int> top5 = players.Select(p => p.Score).Take(5).ToList();
                if (top5.Min() < this.Score || top5.Count() < 5)
                {
                    Text titleText = gameOverScreen.GetComponentInChildren<Text>();
                    titleText.text += titleText.text + "\nВы попали в топ !!!";
                    MainSceneManager.Instance.AddPlayerToTop();
                }
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

    public void UpdateLivesText()
    {
        UIManager uiManager = FindObjectOfType<UIManager>();

        if (uiManager != null)
        {
            uiManager.UpdateLivesText(Lives);
        }
    }
}
