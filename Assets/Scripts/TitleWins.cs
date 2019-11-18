using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleWins : MonoBehaviour
{
    public static bool isEndGame;
    public GameObject darkWinUI;
    public GameObject lightWinUI;
    GameObject correctUI;

    private void LateUpdate()
    {
        if (isEndGame)
        {
            ShowWinScreen();
        }
    }

    public void BackToMenu()
    {
        isEndGame = false;
        SceneManager.LoadScene("title");
    }

    public void ContinuePlaying()
    {
        correctUI.SetActive(false);
        isEndGame = false;
    }

    void ShowWinScreen()
    {
        if (BoardManager.Instance.isWhiteTurn)
        {
            correctUI = darkWinUI;
        }
        else
        {
            correctUI = lightWinUI;
        }

        correctUI.SetActive(true);

    }
}