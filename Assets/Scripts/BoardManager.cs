using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance { get; set; }
    private bool[,] allowedMoves { get; set; } // Доступные ходы фигуры

    public Chessman[,] Chessmans { get; set; } // Позиции фигур на доске
    private Chessman selectedChessman; // Выбранная фигура
    Chessman deleteChessman = null; // Пешка, которая находится на противоположном крае доски


    private const float TILE_SIZE = 1.0f;
    private const float TILE_OFSET = 0.5f;

    // Координаты перемещения фигуры на выбранное место
    private int selectionX = -1;
    private int selectionY = -1;


    public List<GameObject> chessmanPrefabs; // Фигуры (модели)
    private List<GameObject> activeChessman = new List<GameObject>(); // Живые фигуры

    
    private Material previousMat; // Стандартный материал
    public Material selectedMat; // Материал выбранной фигуры (выделение)

    // Чей ход (белых или чёрных)
    public bool isWhiteTurn = true;

    private void Start()
    {
        Instance = this;
        SpawnAllChessmans();

    }

    private void Update()
    {
        // Добралась ли какая-то пешка до противоположного края
        if (deleteChessman!=null)
        {
            // Выбор игрока
            if (ReplacePawn.figNum == 1
                || ReplacePawn.figNum == 2
                || ReplacePawn.figNum == 3
                || ReplacePawn.figNum == 4)
            {
                // Запомнить пешку
                var delete = deleteChessman;

                if(deleteChessman.isWhite) // Поставить белую фигуру
                    SpawnChessman(ReplacePawn.figNum, deleteChessman.CurrentX, deleteChessman.CurrentY);
                else // Поставить чёрную фигуру
                    SpawnChessman(ReplacePawn.figNum+6, deleteChessman.CurrentX, deleteChessman.CurrentY);

                // Удалить пешку из списка живых фигур
                activeChessman.Remove(delete.gameObject);
                // Удалить саму пешку
                Destroy(delete.gameObject);

                // Привести переменные к стандартным значениям
                deleteChessman = null;
                ReplacePawn.figNum = 0; 
            }
        }
        else
        {
            DrawChessboard();
            UpdateSelection();
            SetCamera();

            if (Input.GetMouseButtonDown(0))
            {
                if (selectionX >= 0 && selectionY >= 0)
                {
                    if (selectedChessman == null)
                    {
                        SelectChessman(selectionX, selectionY);
                    }
                    else
                    {
                        if (Chessmans[selectionX, selectionY]?.isWhite == selectedChessman.isWhite)
                        {
                            BoardHighLights.Instance.Hidehighlights();
                            selectedChessman.GetComponent<MeshRenderer>().material = previousMat;

                            SelectChessman(selectionX, selectionY);
                        }
                        else
                        {
                            MoveChessman(selectionX, selectionY);
                        }
                    }
                }
            }
        }
    }

    private void SelectChessman(int x, int y)
    {
        // Выбрано пустое поле
        if (Chessmans[x, y] == null)
        {
            return;
        }

        // Выбрана фигура цвет которой не ходит
        if (Chessmans[x, y].isWhite != isWhiteTurn)
        {
            return;
        }

        // Имеет ли фигура хотя бы 1 доступный ход
        bool hasAtLeastOneMove = false;
        // Получить список ходов выбранной фигуры
        allowedMoves = Chessmans[x, y].PossibleMove();
        // Проверить, есть ли хотя бы 1 доступный ход
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8 && !hasAtLeastOneMove; j++)
            {
                if (allowedMoves[i, j])
                    hasAtLeastOneMove = true;
            }
        }
        // Фигура не имеет доступных ходов
        if (!hasAtLeastOneMove) return;
        // Выбрать данную фигуру
        selectedChessman = Chessmans[x, y];
        // Запомнить стандартный материал
        previousMat = selectedChessman.GetComponent<MeshRenderer>().material;
        // Заменить стандартный материал на материал выдления
        if (selectedMat != null)
        {
            selectedMat.mainTexture = previousMat.mainTexture;
            selectedChessman.GetComponent<MeshRenderer>().material = selectedMat;
        }

        // Показать достепные ходы
        BoardHighLights.Instance.HighlightAllowedMoves(allowedMoves);
    }

    /// <summary>
    /// Сделать ход
    /// </summary>
    /// <param name="x">Координата X</param>
    /// <param name="y">Координата Y</param>
    private void MoveChessman(int x, int y)
    {
        // Можно ли сделать ход на выбранную клетку
        if (allowedMoves[x, y])
        {
            // На выбранной клетке нет фигур или есть фигура противоположного цвета
            Chessman c = Chessmans[x, y];
            if (c != null && c.isWhite != isWhiteTurn)
            {
                // На выбранной клетке стоит Король противоположного цвета
                if (c.GetType() == typeof(King))
                {
                    // Конец игры
                    activeChessman.Remove(c.gameObject);
                    Destroy(c.gameObject);
                    EndGame();
                    return;
                }
                
                activeChessman.Remove(c.gameObject);
                Destroy(c.gameObject);
            }


            // Ходит Король
            if (selectedChessman.GetType() == typeof(King) && ((King)selectedChessman).isRavirovka)
            {
                // Король больше не может рокироваться
                ((King)selectedChessman).isRavirovka = false; 
               
                if (x == selectedChessman.CurrentX + 2) // Рокировка вправо
                {
                    Chessman rook = Chessmans[7, selectedChessman.CurrentY];
                    Chessmans[rook.CurrentX, rook.CurrentY] = null;
                    rook.transform.position = GetTileCenter(x - 1, y);
                    rook.SetPosition(x - 1, y);
                    Chessmans[x - 1, y] = rook;

                }
                else if (x == selectedChessman.CurrentX - 2) // Рокировка влево
                {
                    Chessman rook = Chessmans[0, selectedChessman.CurrentY];
                    Chessmans[rook.CurrentX, rook.CurrentY] = null;
                    rook.transform.position = GetTileCenter(x + 1, y);
                    rook.SetPosition(x + 1, y);
                    Chessmans[x + 1, y] = rook;

                }
            }
            // Ход Ладьёй
            else if (selectedChessman.GetType() == typeof(Rook) 
                     && ((Rook)selectedChessman).isRavirovka)
            {
                // Ладья больше не может рокироваться
                ((Rook)selectedChessman).isRavirovka = false;
            }
            

            // Переместить фигуру
            Chessmans[selectedChessman.CurrentX, selectedChessman.CurrentY] = null; // На выбранной клетке больше нет фигуры
            selectedChessman.transform.position = GetTileCenter(x, y); // Переместить выбранную фигуру на новое место
            selectedChessman.SetPosition(x, y); // Установить новое положение на доске у фиуры
            Chessmans[x, y] = selectedChessman; // Установить на данной позиции новую фигуру

            // Замена пешки на другую фигуру, если она дошла до
            // противоположного края доски
            if (selectedChessman.GetType() == typeof(Pawn)
                && (y == 7 || y == 0))
            {
                // Запомнить пешку, которую нужно заменить
                deleteChessman = selectedChessman;
                // Отобразить UI замены пешки
                ReplacePawn.gameIsPaused = true;
                
            }

            // Изменить условие, чей цвет ходит
            isWhiteTurn = !isWhiteTurn;
        }

        // Установить стандартный материал на выбранной фигуре
        selectedChessman.GetComponent<MeshRenderer>().material = previousMat;

        // Скрыть доступные ходы
        BoardHighLights.Instance.Hidehighlights();
        // Сбросить выделенную фигуру (никакая фигура не выбрана)
        selectedChessman = null;
    }

    /// <summary>
    /// Set camera position, show vector chessboard and chosen square
    /// </summary>
    private void UpdateSelection()
    {
        if (!Camera.main)
        {
            return;
        }

        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),
                            out hit, 25.0f, LayerMask.GetMask("ChessPlane")))
        {
            selectionX = (int)hit.point.x;
            selectionY = (int)hit.point.z;
        }
        else
        {
            selectionX = -1;
            selectionY = -1;
        }
    }

    /// <summary>
    /// Spawn figure to its position
    /// </summary>
    /// <param name="index">figure's index in list</param>
    /// <param name="position">position</param>
    public void SpawnChessman(int index, int x, int y)
    {
        // take figure
        GameObject go = Instantiate(chessmanPrefabs[index], GetTileCenter(x, y), Quaternion.identity) as GameObject;
        go.transform.SetParent(transform); // set position
        Chessmans[x, y] = go.GetComponent<Chessman>();
        Chessmans[x, y].SetPosition(x, y);
        activeChessman.Add(go);

    }

    /// <summary>
    /// Spawn all figures
    /// </summary>
    private void SpawnAllChessmans()
    {
        activeChessman = new List<GameObject>();
        Chessmans = new Chessman[8, 8];

        # region Spawn the Light team!

        //King
        SpawnChessman(0, 4, 0);
        //Queen
        SpawnChessman(1, 3, 0);
        //Rooks
        SpawnChessman(2, 0, 0);
        SpawnChessman(2, 7, 0);
        //Bishops
        SpawnChessman(3, 2, 0);
        SpawnChessman(3, 5, 0);
        //Knights
        SpawnChessman(4, 1, 0);
        SpawnChessman(4, 6, 0);

        //Pawns
        for (int i = 0; i < 8; i++)
        {
            SpawnChessman(5, i, 1);
        }

        #endregion

        # region Spawn the Dark team!

        //King
        SpawnChessman(6, 4, 7);
        //Queen
        SpawnChessman(7, 3, 7);
        //Rooks
        SpawnChessman(8, 0, 7);
        SpawnChessman(8, 7, 7);
        //Bishops
        SpawnChessman(9, 2, 7);
        SpawnChessman(9, 5, 7);
        //Knights
        SpawnChessman(10, 1, 7);
        SpawnChessman(10, 6, 7);

        //Pawns
        for (int i = 0; i < 8; i++)
        {
            SpawnChessman(11, i, 6);
        }

        #endregion

    }

    /// <summary>
    /// Get spawn position on board
    /// </summary>
    /// <param name="x">x pozition</param>
    /// <param name="y">y position</param>
    /// <returns></returns>
    private Vector3 GetTileCenter(int x, int y)
    {
        Vector3 origin = Vector3.zero;
        origin.x += (TILE_SIZE * x) + TILE_OFSET;
        origin.z += (TILE_SIZE * y) + TILE_OFSET;

        return origin;
    }


    /// <summary>
    /// Draw the board
    /// </summary>
    private void DrawChessboard()
    {
        Vector3 widthLine = Vector3.right * 8;
        Vector3 heightLine = Vector3.forward * 8;

        // paint chessboard
        for (int i = 0; i <= 8; i++)
        {
            Vector3 start = Vector3.forward * i;
            Debug.DrawLine(start, start + widthLine);
            for (int j = 0; j <= 8; j++)
            {
                start = Vector3.right * j;

                Debug.DrawLine(start, start + heightLine);

            }
        }


        // Drow the selection (chosen square)
        if (selectionX >= 0 && selectionY >= 0)
        {
            Debug.DrawLine(
                Vector3.forward * selectionY + Vector3.right * selectionX,
                Vector3.forward * (selectionY + 1) + Vector3.right * (selectionX + 1)
                ); // from right to left

            Debug.DrawLine(
                Vector3.forward * (selectionY + 1) + Vector3.right * selectionX,
                Vector3.forward * selectionY + Vector3.right * (selectionX + 1)
                ); // from left to right
        }

    }

    /// <summary>
    ///  Конец игры
    /// </summary>
    public void EndGame()
    {
        if (isWhiteTurn)
        {
            Debug.Log("White team wins!");
        }
        else
        {
            Debug.Log("Dark team wins!");
        }

        foreach (GameObject go in activeChessman)
            Destroy(go);

        isWhiteTurn = !isWhiteTurn;

        BoardHighLights.Instance.Hidehighlights();
        SpawnAllChessmans();

        TitleWins.isEndGame = true;
    }

    private void SetCamera()
    {
        if (isWhiteTurn)
        {
            Camera.main.transform.position = new Vector3(4f, 8f, 4f);
            Camera.main.transform.rotation = Quaternion.Euler(new Vector3(90f, 0f, 0f));

        }
        else
        {
            Camera.main.transform.position = new Vector3(4f, 8f, 4f);
            Camera.main.transform.rotation = Quaternion.Euler(new Vector3(90f, 0f, 0f));

        }
    }


}
