using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainSceneManager : MonoBehaviour
{
    #region Singleton
    private static MainSceneManager _instance;

    public static MainSceneManager Instance => _instance;

    public void Awake()
    {
        if (_instance != null)
            Destroy(gameObject);
        else _instance = this;
    }
    #endregion

    public InputField playerNameInput;
    private TextAsset TopField;

    public Text TopText;
    private List<Player> topPlayers;

    public void OnPlayButtonClicked()
    {
        SceneManager.LoadScene("Game");
    }

    private void Start()
    {
        TopField = Resources.Load("top") as TextAsset;
        LoadTopFromFile();
        UpdateTopPlayersUI();
    }

    private void LoadTopFromFile()
    {
        string[] lines = TopField.text.Split(new[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

        topPlayers = new List<Player>();

        foreach (string line in lines)
        {
            string[] parts = line.Split(' ');

            if (parts.Length == 2)
            {
                Player player = new Player(parts[0], int.Parse(parts[1]));
                topPlayers.Add(player);
            }
            else
            {
                Debug.LogWarning("Некорректный формат строки игрока: " + line);
            }
        }

    }

    public List<Player> LoadPlayers()
    {
        string[] lines = TopField.text.Split(new[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

        topPlayers = new List<Player>();

        foreach (string line in lines)
        {
            string[] parts = line.Split(' ');

            if (parts.Length == 2)
            {
                Player player = new Player(parts[0], int.Parse(parts[1]));
                topPlayers.Add(player);
            }
            else
            {
                Debug.LogWarning("Некорректный формат строки игрока: " + line);
            }
        }
        return topPlayers;
    }

    private void SaveTopToFile()
    {
        string filePath = "Assets/Resources/top.txt";
        File.WriteAllLines(filePath, topPlayers.OrderByDescending(p => p.Score).Select(p => p.ToString()));
    }
    public void UpdateTopPlayersUI()
    {
        TopText.text = string.Join('\n', topPlayers.Select(p => p.toUi()).Take(5));
    }

    public void AddPlayerToTop()
    {
        string playerName = playerNameInput.text;

        playerName = playerName == "" || playerName == string.Empty || playerName == null ? "Anonym" : playerName;
        int playerScore = GameManager.Instance.Score;
        Player newPlayer = new Player(playerName, playerScore);
        topPlayers.Add(newPlayer);

        // Обновление UI
        //UpdateTopPlayersUI();

        // Сохранение топа в файл
        SaveTopToFile();
    }
}
