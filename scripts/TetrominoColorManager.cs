using UnityEngine;

public class TetrominoColorManager : MonoBehaviour
{
    public static TetrominoColorManager Instance { get; private set; }

    private int currentLevel = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Update()
    {
        int newLevel = TetrisBlock.LinesCleared / TetrisBlock.linesPerLevel;
        if (newLevel != currentLevel)
        {
            currentLevel = newLevel;
            UpdateAllTetrominoColors();
        }
    }

    private void UpdateAllTetrominoColors()
    {
        GameObject[] tetrominoes = GameObject.FindGameObjectsWithTag("Tetromino");
        foreach (GameObject tetromino in tetrominoes)
        {
            SpawnTetromino.Instance.UpdateTetrominoColor(tetromino);
        }
    }

    public void UpdateColorOnLevelUp()
    {
        currentLevel = TetrisBlock.LinesCleared / TetrisBlock.linesPerLevel;
        UpdateAllTetrominoColors();
    }
}