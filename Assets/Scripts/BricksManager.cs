using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BricksManager : MonoBehaviour
{
    #region Singleton

    private static BricksManager _instance;

    public static BricksManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else _instance = this;
    }
    #endregion

    public Brick brickPrefab;
    public Sprite[] Sprites;
    public Color[] BrickColors;

    private int maxRows = 17;
    private int maxCols = 12;
    private GameObject bricksContainer;
    private float shiftAmountX = 1f;
    private float shiftAmountY = 0.75f;
    private float shiftSpace = 0.05f;

    public List<int[,]> levelsData { get; set; }

    public List<Brick> RemainingBricks { get; set; }

    public int InitialBrickCounts { get; set; }

    private float initialBrickSpawnPositionX = -5.75f;
    private float initialBrickSpawnPositionY = 3.6f;

    public int CurrentLevel;

    private void Start()
    {

        this.bricksContainer = new GameObject("BricksContainer");
        this.RemainingBricks = new List<Brick>(); 
        this.levelsData = this.LoadLevelsData();
        this.GenerateBricks();
    }

    private void GenerateBricks()
    {
        this.RemainingBricks = new List<Brick>();
        int[,] currentLevelData = this.levelsData[this.CurrentLevel];
        float currentSpawnX = initialBrickSpawnPositionX;
        float currentSpawnY = initialBrickSpawnPositionY;
        float zShift = 0;

        for (int row = 0; row < this.maxRows; row++)
        {
            for (int col = 0; col < maxCols; col++)
            {
                int brickType = currentLevelData[row, col];

                if (brickType > 0)
                {
                    Color BrickColor;

                    if (brickType - 1 == 3) BrickColor = this.BrickColors[2];
                    else if (brickType - 1 > 3) BrickColor = this.BrickColors[3];
                    else BrickColor = this.BrickColors[brickType - 1];

                    Brick newBrick = Instantiate(brickPrefab, new Vector3(currentSpawnX, currentSpawnY, 0.0f - zShift), Quaternion.identity) as Brick;
                    if (brickType != -1 && brickType != 30)
                        newBrick.Init(bricksContainer.transform, this.Sprites[brickType - 1], BrickColor, brickType);
                    else newBrick.Init(bricksContainer.transform, this.Sprites[4], BrickColor, brickType);

                    this.RemainingBricks.Add(newBrick);
                    zShift += 0.0001f;
                }
                else if (brickType == -1)
                {
                    Color BrickColor;
                    BrickColor = this.BrickColors[0]; //yellow by default
                    Brick newBrick = Instantiate(brickPrefab, new Vector3(currentSpawnX, currentSpawnY, 0.0f - zShift), Quaternion.identity) as Brick;
                    newBrick.Init(bricksContainer.transform, this.Sprites[0], BrickColor, Mathf.Abs(brickType));

                    Rigidbody2D newBrickRb = newBrick.gameObject.AddComponent<Rigidbody2D>();
                    newBrickRb.gravityScale = 0f;

                    float horizontalSpeed = 5f;

                    newBrickRb.sharedMaterial = new PhysicsMaterial2D
                    {
                        bounciness = 1f,
                        friction = 0f
                    };

                    newBrickRb.AddForce(new Vector2(horizontalSpeed, 0f), ForceMode2D.Impulse);

                    this.RemainingBricks.Add(newBrick);
                    zShift += 0.0001f;
                }

                currentSpawnX += shiftAmountX + shiftSpace;
                if (col + 1 == this.maxCols)
                {
                    currentSpawnX = initialBrickSpawnPositionX;
                }
            }

            currentSpawnY -= shiftAmountY + shiftSpace;
        }

        this.InitialBrickCounts = this.RemainingBricks.Count;
        //OnLevelLoaded?.Invoke();
    }

    private List<int[,]> LoadLevelsData()
    {
        TextAsset text = Resources.Load("levels") as TextAsset;

        string[] rows = text.text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

        List<int[,]> levelsData = new List<int[,]>();
        int[,] currentLevel = new int[maxRows, maxCols];
        int currentRow = 0;

        for (int row = 0; row < rows.Length; row++)
        {
            string line = rows[row];

            if (line.IndexOf("--") == -1)
            {
                string[] bricks = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int col = 0; col < bricks.Length; col++)
                {
                    currentLevel[currentRow, col] = int.Parse(bricks[col]);
                }

                currentRow++;

            }
            else
            {
                currentRow = 0;
                levelsData.Add(currentLevel);

                currentLevel = new int[maxRows, maxCols];
            }
        }

        return levelsData;
    }
}
