using UnityEngine;
using TMPro;

// note for self, nekad vairāk nerakstit 400 liniju script, ja tas nav needed.

public class TetrisBlock : MonoBehaviour
{
    public static byte LinesCleared;
    public static int height = 21;
    public static int width = 10;
    public static float fallTimeDecrease;
    public static float minFallTime = 0.1f;
    public static float fallTime;
    public static float CurrentFallTime { get; private set; } = 1f;
    public static byte linesPerLevel = 10;
    public static bool gameOver = false;

    private static float initialFallTime = 1f;
    private float previousTime;
    private static int level = 0;

    private GameManager gameManager;
    private SpawnTetromino spawnTetromino;
    private static TextMeshProUGUI linesClearedText;
    private static TextMeshProUGUI levelText;
    private AudioSource audioSource;
    public Vector3 rotationPoint;
    
    private static TetrisBlock instance;
    public AudioClip moveSound;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        spawnTetromino = FindObjectOfType<SpawnTetromino>();
        linesClearedText = GameObject.Find("Lines_Cleared").GetComponent<TextMeshProUGUI>();
        levelText = GameObject.FindWithTag("Text_Level").GetComponent<TextMeshProUGUI>();
        fallTime = CurrentFallTime;
        
        if (levelText == null)
        {
            Debug.LogError("No level text found");
        }
        
        Debug.Log(SceneController.noFunPressed);

        ResetGame();
        SetupAudioSource();
        CalculateFallDecrement();

        instance = this;
        moveSound = AudioManager.Instance.audioClips[0];
    }

    private void Update()
    {
        if (gameManager.StartMenu != null && !gameManager.StartMenu.activeSelf)
        {
            HandleInput();
        }
    }

    void CalculateFallDecrement()
    {
        if (SceneController.noFunPressed)
        {
            fallTimeDecrease = 0.9f;
        } 
        else if (SceneController.sweatPressed)
        {
            fallTimeDecrease = 0.4f;
        }
        else if (SceneController.hardPressed)
        {
            fallTimeDecrease = 0.2f;
        }
        else if (SceneController.SkillIssuePressed)
        {
            fallTimeDecrease = 0.05f;
        }
        else
        {
            fallTimeDecrease = 0.1f;
        }
    }

    private void ResetGame()
    {
        if (SceneController.hasRestartedScene)
        {
            CurrentFallTime = initialFallTime;
            level = 0;
            LinesCleared = 0;
            UpdateUi();
            SceneController.hasRestartedScene = false;
            Debug.Log($"The current fall time is: {CurrentFallTime}");
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            MoveHorizontal(-1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            MoveHorizontal(1);
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            Rotate();
        }
        if (Time.time - previousTime > (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S) ? fallTime / 10 : fallTime))
        {
            MoveDown();
        }
    }

    private void MoveHorizontal(int direction)
    {
        AudioManager.Instance.PlayMoveSound();
        transform.position += new Vector3(direction, 0, 0);
        if (!ClearLine.ValidMove(transform))
            transform.position -= new Vector3(direction, 0, 0);
    }

    private void Rotate()
    {
        AudioManager.Instance.PlayRotateSound();
        transform.RotateAround(transform.TransformPoint(rotationPoint), Vector3.forward, 90);
        if (!ClearLine.ValidMove(transform))
            transform.RotateAround(transform.TransformPoint(rotationPoint), Vector3.forward, -90);
    }

    private void MoveDown()
    {
        transform.position += Vector3.down;
        if (!ClearLine.ValidMove(transform))
        {
            transform.position -= Vector3.down;
            ClearLine.AddToGrid(transform);
            ClearLine.CheckForLines();
            this.enabled = false;

            if (!ClearLine.isLineClearAnimationInProgress && !gameOver)
            {
                spawnTetromino.NewTetromino();
            }
        }
        previousTime = Time.time;
    }

    public static void UpdateUi()
    {
        if (linesClearedText != null)
        {
            linesClearedText.text = "Lines Cleared: " + LinesCleared.ToString();
        }
        if (levelText != null)
        {
            levelText.text = "Level: " + level.ToString();
        }
        else
        {
            Debug.LogError("Level TextMeshProUGUI component not found!");
        }
    }

    

    public static void LevelUp()
    {
        AudioManager.Instance.LevelUpSound();
        int newLevel = LinesCleared / linesPerLevel;
        
        if (newLevel > level)
        {
            int levelDifference = newLevel - level;
            for (int i = 0; i < levelDifference; i++)
            {
                CurrentFallTime = Mathf.Max(CurrentFallTime - fallTimeDecrease, minFallTime);
            }
            
            level = newLevel;
            UpdateUi();
            
            if (newLevel == 157)
            {
                GameManager.Instance.WaitWhat();
            }

            TetrominoColorManager.Instance.UpdateColorOnLevelUp();
        }
        Debug.Log($"Current fall time: {CurrentFallTime}, Current level: {level}");
    }

    public static void ResetStaticVariables()
    {
        ClearLine.ResetGrid();
        LinesCleared = 0;
        gameOver = false;
        UpdateUi();
    }

    private void SetupAudioSource()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.clip = moveSound;
        audioSource.playOnAwake = false;
    }
}
