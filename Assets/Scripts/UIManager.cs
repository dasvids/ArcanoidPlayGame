using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text Target;
    public Text ScoreText;
    public Text LivesText;

    public void Awake()
    {
        Brick.OnBrickDestruction += OnBrickDestruction;
        BricksManager.OnLevelLoaded += OnLevelLoaded;
        GameManager.OnLiveLost += OnLiveLost;
    }
    private void Start()
    {
        OnLiveLost(GameManager.Instance.AvailibleLives);
    }
    private void OnLiveLost(int remainingLives)
    {
        LivesText.text = $"Lives: {remainingLives}";
    }

    private void OnLevelLoaded()
    {
        UpdateRemainingBricksText();
        UpdateScoreText(0);
    }

    private void UpdateScoreText(int increment)
    {
        GameManager.Instance.Score += increment;
        string scoreString = GameManager.Instance.Score.ToString().PadLeft(5, '0');
        ScoreText.text = $"Score: {scoreString}";
    }

    private void OnBrickDestruction(Brick brick)
    {
        UpdateRemainingBricksText();
        int inc = getIncrement(brick);
        UpdateScoreText(inc);
    }

    private int getIncrement(Brick brick)
    {
        if (brick.sr.color == BricksManager.Instance.BrickColors[3])
            return 100;
        else if (brick.sr.color == BricksManager.Instance.BrickColors[2])
            return 40;
        else if (brick.sr.color == BricksManager.Instance.BrickColors[1])
            return 20;
        else return 10;
    }

    private void UpdateRemainingBricksText()
    {
        Target.text = $"Target: {BricksManager.Instance.RemainingBricks.Count}/{BricksManager.Instance.InitialBrickCounts}";
    }

    private void OnDisable()
    {
        Brick.OnBrickDestruction -= OnBrickDestruction;
        BricksManager.OnLevelLoaded -= OnLevelLoaded;
    }
}
