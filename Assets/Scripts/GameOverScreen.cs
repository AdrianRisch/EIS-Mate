using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    public Text winText;
    public void Setup(string win)
    {
        gameObject.SetActive(true);
        winText.text = win;
    }

    public void Restart()
    {
        GameObject chessboard = GameObject.Find("Chessboard");
        BoardManager boardManager = chessboard.GetComponent<BoardManager>();
        boardManager.Restart();
        gameObject.SetActive(false);
    }
}
