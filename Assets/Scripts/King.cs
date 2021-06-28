using System.Collections;
using UnityEngine;

public class King : Chessman
{
    public override bool[,] PossibleMoves()
    {
        bool[,] r = new bool[8, 8];

        // up
        Move(CurrentX + 1, CurrentY, ref r);
        // down
        Move(CurrentX - 1, CurrentY, ref r);
        // left
        Move(CurrentX, CurrentY - 1, ref r);
        // right
        Move(CurrentX, CurrentY + 1, ref r);
        // up left
        Move(CurrentX + 1, CurrentY - 1, ref r);
        // down left
        Move(CurrentX - 1, CurrentY - 1, ref r);
        // up right
        Move(CurrentX + 1, CurrentY + 1, ref r);
        // down right
        Move(CurrentX - 1, CurrentY + 1, ref r); 

        return r;
    }



}
