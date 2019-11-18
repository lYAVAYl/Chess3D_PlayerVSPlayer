using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Chessman
{
    public override bool[,] PossibleMove()
    {
        bool[,] r = new bool[8, 8];

        Chessman c;

        int i, j;

        // Right
        i = CurrentX;
        while (true)
        {
            i++;
            if (i >= 8)
                break;

            c = BoardManager.Instance.Chessmans[i, CurrentY];

            if (c == null)
                r[i, CurrentY] = true;
            else
            {
                if (c.isWhite != isWhite)
                    r[i, CurrentY] = true;

                break;
            }
        }
        // Left
        i = CurrentX;
        while (true)
        {
            i--;
            if (i < 0)
                break;

            c = BoardManager.Instance.Chessmans[i, CurrentY];

            if (c == null)
                r[i, CurrentY] = true;
            else
            {
                if (c.isWhite != this.isWhite)
                    r[i, CurrentY] = true;

                break;
            }
        }
        // Up
        i = CurrentY;
        while (true)
        {
            i++;
            if (i >= 8)
                break;

            c = BoardManager.Instance.Chessmans[CurrentX, i];

            if (c == null)
                r[CurrentX, i] = true;
            else
            {
                if (c.isWhite != this.isWhite)
                    r[CurrentX, i] = true;

                break;
            }
        }
        // Down
        i = CurrentY;
        while (true)
        {
            i--;
            if (i < 0)
                break;

            c = BoardManager.Instance.Chessmans[CurrentX, i];

            if (c == null)
                r[CurrentX, i] = true;
            else
            {
                if (c.isWhite != this.isWhite)
                    r[CurrentX, i] = true;

                break;
            }
        }



        // Top Left
        i = CurrentX;
        j = CurrentY;

        while (true)
        {
            i--;
            j++;
            if (i < 0 || j >= 8)
                break;

            c = BoardManager.Instance.Chessmans[i, j];
            if (QueenMoove(i, j, c, ref r))
                break;

        }

        // Top Right
        i = CurrentX;
        j = CurrentY;

        while (true)
        {
            i++;
            j++;
            if (i >= 8 || j >= 8)
                break;

            c = BoardManager.Instance.Chessmans[i, j];
            if (QueenMoove(i, j, c, ref r))
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

            if (QueenMoove(i, j, c, ref r))
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

            if (QueenMoove(i, j, c, ref r))
                break;

        }

        return r;
    }

    private bool QueenMoove(int i, int j, Chessman c, ref bool[,] r)
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