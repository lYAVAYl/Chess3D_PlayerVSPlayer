using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReplacePawn : MonoBehaviour
{
    public GameObject replacePawnUI; // сам UI
    public static int figNum; // Фигура, которой нужно заменить пешку

    public static bool gameIsPaused; // Условие паузы

    private void Update()
    {
        // Пауза
        if (gameIsPaused)
        {
            // Вывести UI замены пешки на другую фигуру
            replacePawnUI.SetActive(true);
            // Остановить игру
            Time.timeScale = 0f;
        }
    }

    /// <summary>
    /// Поставить Королеву
    /// </summary>
    public void SpawnQueen()
    {
        figNum = 1; 
        ResumeGame();

    }
    /// <summary>
    /// Поставить Ладью
    /// </summary>
    public void SpawnRook()
    {
        figNum = 2; 
        ResumeGame();

    }
    /// <summary>
    /// Поставить Ферзя
    /// </summary>
    public void SpawnBishop()
    {
        figNum = 3;
        ResumeGame();

    }
    /// <summary>
    /// Поставить коня
    /// </summary>
    public void SpawnKnight()
    {
        figNum = 4;
        ResumeGame();

    }

    private void ResumeGame()
    {
        replacePawnUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

}
