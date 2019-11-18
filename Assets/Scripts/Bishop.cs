using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Chessman
{
    public override bool[,] PossibleMove()
    {
        bool[,] r = new bool[8, 8];

        Chessman c;
        int i, j;

        // Top Left
        i = CurrentX;
        j = CurrentY;

        while (true)
        {
            i--;
            j++;
            if(i < 0 || j >= 8)
                break;

            c = BoardManager.Instance.Chessmans[i, j];
            if (BishopMove(i, j, c, ref r))
                break;

        }

        // Top Right
        i = CurrentX;
        j = CurrentY;

        while (true)
        {
            i++;
            j++;
            if (i >=8 || j >= 8)
                break;

            c = BoardManager.Instance.Chessmans[i, j];
            if (BishopMove(i, j, c, ref r))
                break;

        }

        // Down Right
        i = CurrentX;
        j = CurrentY;

        while (true)
        {
            i++;
            j--;
            if (i >= 8 || j < 0)
                break;

            c = BoardManager.Instance.Chessmans[i, j];

            if (BishopMove(i, j, c, ref r))
                break;

        }

        // Down Left
        i = CurrentX;
        j = CurrentY;

        while (true)
        {
            i--;
            j--;
            if (i < 0 || j < 0)
                break;

            c = BoardManager.Instance.Chessmans[i, j];

            if (BishopMove(i, j, c, ref r))
                break;

        }


        return r;
    }

    private bool BishopMove(int i, int j, Chessman c, ref bool[,] r)
    {
        if (c == null)
            r[i, j] = true;
        else
        {
            if (c.isWhite != isWhite)
                r[i, j] = true;

            return true;
        }

        return false;
    }
}