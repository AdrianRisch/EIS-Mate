﻿using System.Collections;
using UnityEngine;

public class Bishop : Chessman
{
    public override bool[,] PossibleMoves()
    {
        bool[,] r = new bool[8, 8];

        int i, j;

        // Top left
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i--;
            j++;
            if (i < 0 || j >= 8) break;

            if (Move(i, j, ref r)) break;
        }

        // Top right
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i++;
            j++;
            if (i >= 8 || j >= 8) break;

            if (Move(i, j, ref r)) break;
        }

        // Down left
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i--;
            j--;
            if (i < 0 || j < 0) break;

            if (Move(i, j, ref r)) break;
        }

        // Down right
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i++;
            j--;
            if (i >= 8 || j < 0) break;

            if (Move(i, j, ref r)) break;
        }


        return r;
    }

}


