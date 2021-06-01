using System.Collections;
using UnityEngine;

public abstract class Chessman : MonoBehaviour
{
    public int CurrentX { set; get; }
    public int CurrentY { set; get; }
    public bool isWhite;

    public void SetPosition(int x, int y)
    {
        CurrentX = x;
        CurrentY = y;
    }

    public virtual bool PossibleMove(int x, int y)
    {
        return true;
    }
}
