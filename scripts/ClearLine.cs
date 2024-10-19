using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearLine : MonoBehaviour
{
    public static bool isLineClearAnimationInProgress = false;
    private static int linesClearingCount = 0;
    private static Transform[,] grid = new Transform[TetrisBlock.width, TetrisBlock.height];
    [SerializeField] private string lineClearAnimationTrigger = "LineClear";
    [SerializeField] private AnimationClip lineClearClip;
    [SerializeField] private Animator blockAnimator;
    private static bool RowDownIsComplete = true;

    private static ClearLine instance;

    void Awake()
    {
        blockAnimator = GetComponent<Animator>();
        lineClearClip = GetComponent<AnimationClip>();
        if (blockAnimator == null)
            Debug.LogError("Animator component not found on the Tetris block!");

        if (lineClearClip == null)
            Debug.LogError("Line Clear AnimationClip is not assigned!");

        instance = this;
        
    }
    

    public static void CheckForLines()
    {
        List<int> fullLines = new List<int>();
        for (int i = TetrisBlock.height - 1; i >= 0; i--)
        {
            if (HasLine(i))
            {
                TetrisBlock.LinesCleared++;
                fullLines.Add(i);
                if (TetrisBlock.LinesCleared % TetrisBlock.linesPerLevel == 0)
                {
                    TetrisBlock.LevelUp();
                }
            }
        }
        if (fullLines.Count > 0)
        {
            if (fullLines.Count > 2)
            {
                AudioManager.Instance.SeceretLineClearSound();
            }
            else
            {
                AudioManager.Instance.LineClearSound();
            }
            linesClearingCount += fullLines.Count;
            instance.StartCoroutine(ClearMultipleLines(fullLines));
            isLineClearAnimationInProgress = true;
        } 
    }

    private static bool HasLine(int i)
    {
        for (int j = 0; j < TetrisBlock.width; j++)
        {
            if (grid[j, i] == null)
                return false;
        }
        return true;
    }
    
/* !TODO Check out if there is a bug and fix it if there is (maybe leave it as a feature)
   ^^^^^^^^^^^^^^^^^^^^^ */
// Possibly a bug, idk. 
    
    private static IEnumerator ClearMultipleLines(List<int> linesToClear)
    {
        isLineClearAnimationInProgress = true;
        float animationLength = instance.lineClearClip != null ? instance.lineClearClip.length : 1f;
        float totalDelay = animationLength + 0.5f;

        foreach (int line in linesToClear)
        {
            for (int j = 0; j < TetrisBlock.width; j++)
            {
                if (grid[j, line] != null)
                {
                    Animator animator = grid[j, line].GetComponent<Animator>();
                    animator?.SetTrigger(instance.lineClearAnimationTrigger);
                }
            }
        }

        yield return new WaitForSeconds(totalDelay);

        foreach (int line in linesToClear)
        {
            for (int j = 0; j < TetrisBlock.width; j++)
            {
                if (grid[j, line] != null)
                {
                    Object.Destroy(grid[j, line].gameObject);
                    grid[j, line] = null;
                }
            }
        }
        foreach (int line in linesToClear)
        {
            RowDown(line);
        }

        TetrisBlock.UpdateUi();
        linesClearingCount -= linesToClear.Count;
        isLineClearAnimationInProgress = false;

        if (!TetrisBlock.gameOver && RowDownIsComplete)
        {
            yield return new WaitForSeconds(0.5f);
            SpawnTetromino.Instance.NewTetromino();
        }
    }

    private static void RowDown(int i)
    {
        RowDownIsComplete = false;
        
        for (int y = i; y < TetrisBlock.height - 1; y++)
        {
            for (int j = 0; j < TetrisBlock.width; j++)
            {
                if (grid[j, y + 1] != null)
                {
                    grid[j, y] = grid[j, y + 1];
                    grid[j, y + 1] = null;
                    grid[j, y].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
        HandleFloatingBlocks();
            CheckForLines();
            RowDownIsComplete = true;
    }
    
// ----------------------------------------------------------------------------------------------

    private static void HandleFloatingBlocks()
    {
        bool blocksMoved;
        do
        {
            blocksMoved = false;
            for (int y = 0; y < TetrisBlock.height - 1; y++)
            {
                for (int x = 0; x < TetrisBlock.width; x++)
                {
                    if (grid[x, y] != null && IsFloating(x, y))
                    {
                        grid[x, y - 1] = grid[x, y];
                        grid[x, y] = null;
                        grid[x, y - 1].transform.position -= new Vector3(0, 1, 0);
                        blocksMoved = true;
                    }
                }
            }
        } while (blocksMoved);

        if (!isLineClearAnimationInProgress)
        {
            CheckForLines();
        }
    }

    private static bool IsFloating(int x, int y)
    {
        if (y > 0 && grid[x, y - 1] == null)
        {
            bool noLeftBlock = (x == 0) || (grid[x - 1, y] == null);
            bool noRightBlock = (x == TetrisBlock.width - 1) || (grid[x + 1, y] == null);
            bool noTopBlock = (y == TetrisBlock.height - 1) || (grid[x, y + 1] == null);
            return noLeftBlock && noRightBlock && noTopBlock;
        }
        return false;
    }

    public static void AddToGrid(Transform block)
    {
        
        AudioManager.Instance.AddToGridSound();
        foreach (Transform child in block)
        {
            int roundedX = Mathf.RoundToInt(child.transform.position.x);
            int roundedY = Mathf.RoundToInt(child.transform.position.y);
            if (roundedX >= 0 && roundedX < TetrisBlock.width && roundedY >= 0 && roundedY < TetrisBlock.height)
            {
                grid[roundedX, roundedY] = child;
            }

            if (child.position.y >= 19.5f)
            {
                AudioManager.Instance.GameOverSound();
                SpawnTetromino.Instance.GameOver();
            }
        }
    }

    public static bool ValidMove(Transform block)
    {
        foreach (Transform child in block)
        {
            int roundedX = Mathf.RoundToInt(child.transform.position.x);
            int roundedY = Mathf.RoundToInt(child.transform.position.y);
            if (roundedX < 0 || roundedX >= TetrisBlock.width || roundedY < 0 || roundedY >= TetrisBlock.height)
            {
                return false;
            }
            if (grid[roundedX, roundedY] != null)
                return false;
        }
        return true;
    }

    public static void ResetGrid()
    {
        grid = new Transform[TetrisBlock.width, TetrisBlock.height];
    }
}
