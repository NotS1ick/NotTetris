using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnTetromino : MonoBehaviour
{
    public static SpawnTetromino Instance { get; private set; }

    public GameObject[] Tetrominoes;
    public Transform previewTransform;
    public GameObject[] ColoredBlockPrefabs;
    private GameObject previewPiece;
    private int currentIndex;
    private int nextIndex;
    public GameObject GameOverScreen;

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

    void Start()
    {
        currentIndex = Random.Range(0, Tetrominoes.Length);
        nextIndex = Random.Range(0, Tetrominoes.Length);

        CreatePreview();
        SpawnCurrentPiece();

        GameOverScreen.SetActive(false);
    }

    private void SpawnCurrentPiece()
    {
        if (!TetrisBlock.gameOver)
        {
            GameObject spawnedTetromino = Instantiate(Tetrominoes[currentIndex], transform.position, Quaternion.identity);
            spawnedTetromino.AddComponent<ClearLine>();
            UpdateTetrominoColor(spawnedTetromino);
        }
    }
    
    public void UpdateTetrominoColor(GameObject tetromino)
    {
        if (ColoredBlockPrefabs.Length == 0) return;

        int currentLevel = TetrisBlock.LinesCleared / TetrisBlock.linesPerLevel;
        int colorIndex = currentLevel % ColoredBlockPrefabs.Length;

        foreach (Transform child in tetromino.transform)
        {
            SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = ColoredBlockPrefabs[colorIndex].GetComponent<SpriteRenderer>().sprite;
            }
        }
    }

    public void NewTetromino()
    {
        if (!TetrisBlock.gameOver && !ClearLine.isLineClearAnimationInProgress)
        {
            currentIndex = nextIndex;
            nextIndex = Random.Range(0, Tetrominoes.Length);

            SpawnCurrentPiece();

            if (previewPiece != null)
            {
                Destroy(previewPiece);
            }
            CreatePreview();
        }
    }

    private void CreatePreview()
    {
        switch (nextIndex)
        {
            case 0:
                previewPiece = Instantiate(Tetrominoes[nextIndex], new Vector3(12.5f, 16.73f, 1), Quaternion.identity);
                break;
            case 3:
                previewPiece = Instantiate(Tetrominoes[nextIndex], new Vector3(12.5f, 15.73f, 1), Quaternion.identity);
                break;
            case 2:
            case 6:
                previewPiece = Instantiate(Tetrominoes[nextIndex], new Vector3(12, 16.73f, 1), Quaternion.identity);
                break;
            case 5:
                previewPiece = Instantiate(Tetrominoes[nextIndex], new Vector3(13, 15.73f, 1), Quaternion.identity);
                break;
            default:
                previewPiece = Instantiate(Tetrominoes[nextIndex], previewTransform.position, Quaternion.identity);
                break;
        }

        UpdateTetrominoColor(previewPiece);

        TetrisBlock previewBlock = previewPiece.GetComponent<TetrisBlock>();
        if (previewBlock != null)
        {
            previewBlock.enabled = false;
        }
    }

    public void GameOver()
    {
        TetrisBlock.gameOver = true;
        GameOverScreen.SetActive(true);
        AudioManager.Instance.backGroundAudioSource.Stop();
        Debug.Log("Game Over");
    }
}
