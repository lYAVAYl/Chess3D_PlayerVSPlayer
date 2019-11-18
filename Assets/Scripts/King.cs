using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Chessman
{
    public bool isRavirovka = true;

    public override bool[,] PossibleMove()
    {
        bool[,] r = new bool[8, 8];

        Chessman c;
        int i, j;

        //Top Side
        i = CurrentX - 1;
        j = CurrentY + 1;
        if (CurrentY != 7)
        {
            for (int k = 0; k < 3; k++)
            {
                if (i >= 0 && i < 8)
                {
                    c = BoardManager.Instance.Chessmans[i, j];

                    if (c == null || c.isWhite != isWhite)
                        r[i, j] = true;
                }

                i++;
            }
        }

        // Down Side
        i = CurrentX - 1;
        j = CurrentY - 1;
        if (CurrentY != 0)
        {
            for (int k = 0; k < 3; k++)
            {
                if (i >= 0 && i < 8)
                {
                    c = BoardManager.Instance.Chessmans[i, j];

                    if (c == null || c.isWhite != isWhite)
                        r[i, j] = true;
                }

                i++;
            }
        }

        // Middle Right
        if (CurrentX != 7)
        {
            c = BoardManager.Instance.Chessmans[CurrentX + 1, CurrentY];
            if (c == null || c.isWhite != isWhite)
                r[CurrentX + 1, CurrentY] = true;

            if (CurrentX == 4)
            {
                // Условия короткой рокировки-------------------------------------------
                if ((CurrentY == 0 || CurrentY == 7) && c == null && ShortVilka(isWhite)
                   && BoardManager.Instance.Chessmans[CurrentX + 2, CurrentY] == null)
                {
                    Chessman rook = BoardManager.Instance.Chessmans[7, CurrentY];
                    if (rook != null && rook.GetType() == typeof(Rook) && isRavirovka)
                    {
                        if (((Rook)rook).isRavirovka)
                            r[CurrentX + 2, CurrentY] = true;

                    }
                }
                // Условия короткой рокировки-------------------------------------------
            }

        }

        // Middle Left
        if (CurrentX != 0)
        {
            c = BoardManager.Instance.Chessmans[CurrentX - 1, CurrentY];
            if (c == null || c.isWhite != isWhite)
                r[CurrentX - 1, CurrentY] = true;

            if (CurrentX == 4)
            {
                // Условия длинной рокировки--------------------------------------------
                var chessmans = BoardManager.Instance.Chessmans;
                if ((CurrentY == 0 || CurrentY == 7) && c == null && LongVilka(isWhite)
                   && chessmans[CurrentX - 2, CurrentY] == null
                   && chessmans[CurrentX - 3, CurrentY] == null)
                {
                    Chessman rook = BoardManager.Instance.Chessmans[0, CurrentY];
                    if (rook != null && rook.GetType() == typeof(Rook) && isRavirovka)
                    {
                        if (((Rook)rook).isRavirovka)
                            r[CurrentX - 2, CurrentY] = true;

                    }

                }
                // Условия длинной рокировки--------------------------------------------
            }

        }

        return r;
    }

   
    private bool LongVilka(bool white)
    {
        bool RokirIsPoss = true;
        var chessmans = BoardManager.Instance.Chessmans;

        foreach (var figure in chessmans)
        {
            if (figure && figure.isWhite != white && figure.GetType() != typeof(King))
            {
                var allowedMoves = figure.PossibleMove();

                if (white)
                {
                    RokirIsPoss = !(allowedMoves[CurrentX, CurrentY]
                                || allowedMoves[CurrentX - 1, CurrentY]
                                || allowedMoves[CurrentX - 2, CurrentY]);
                }
                else
                {
                    RokirIsPoss = !(allowedMoves[CurrentX, CurrentY]
                                || allowedMoves[CurrentX + 1, CurrentY]
                                || allowedMoves[CurrentX + 2, CurrentY]);
                }
                

                if (!RokirIsPoss) break;

            }
        }
        return RokirIsPoss;
    }

    private bool ShortVilka(bool white)
    {
        bool RokirIsPoss = true;
        var chessmans = BoardManager.Instance.Chessmans;

        foreach (var figure in chessmans)
        {
            if (figure && figure.isWhite != white && figure.GetType() != typeof(King))
            {
                var allowedMoves = figure.PossibleMove();

                if (white)
                {
                    RokirIsPoss = !(allowedMoves[CurrentX, CurrentY]
                                || allowedMoves[CurrentX + 1, CurrentY]
                                || allowedMoves[CurrentX + 2, CurrentY]);
                }
                else
                {
                    RokirIsPoss = !(allowedMoves[CurrentX, CurrentY]
                                || allowedMoves[CurrentX - 1, CurrentY]
                                || allowedMoves[CurrentX - 2, CurrentY]);
                }

                if (!RokirIsPoss) break;

            }
        }
        return RokirIsPoss;
    }


}
