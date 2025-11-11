using UnityEngine;
using UnityEngine.Events;

public class GameStateManager : MonoSingleton<GameStateManager>
{

    public UnityEvent OnGamePaused;
    public UnityEvent OnGameResumed;

    private bool isPaused = false;

    public void TogglePause()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        if (isPaused) return;

        isPaused = true;
        Time.timeScale = 0f;

        // 触发暂停事件
        //OnGamePaused?.Invoke();

        Debug.Log("游戏已暂停");
    }

    public void ResumeGame()
    {
        if (!isPaused) return;

        isPaused = false;
        Time.timeScale = 1f;

        // 触发恢复事件
        //OnGameResumed?.Invoke();

        Debug.Log("游戏已恢复");
    }

    public bool IsGamePaused()
    {
        return isPaused;
    }
}

