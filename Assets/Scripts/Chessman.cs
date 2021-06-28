using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Chessman : MonoBehaviour
{
    // Basics for all chess pieces
    public int CurrentX { set; get; }
    public int CurrentY { set; get; }

    public bool isWhite;

    // set position of chess piece
    public void SetPosition(int x, int y)
    {
        CurrentX = x;
        CurrentY = y;
    }

    // possible moves of chess piece
    public virtual bool[,] PossibleMoves()
    {
        return new bool[8, 8];
    }

    // move the chess piece
    public bool Move(int x, int y, ref bool[,] r)
    {
        if (x >= 0 && x < 8 && y >= 0 && y < 8)
        {
            Chessman c = BoardManager.Instance.Chessmans[x, y];
            if (c == null)
                r[x, y] = true;
            else
            {
                if (isWhite != c.isWhite)
                    r[x, y] = true;
                return true;
            }
        }
        return false;
    }
}
