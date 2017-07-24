﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace CheckersBoard
{
    struct Button
    {
        public string Name;
        public int Row;
        public int Column;
    }

    /// <summary>
    /// CheckersManager, descendent of MainWindow.xaml
    /// </summary>
    public class CheckersManager
    {
        private Move currentMove;
        private String winner;
        private String turn;
        private String Title;
        private Button[,] CheckersGrid;

        public CheckersManager()
        {
            this.Title = "Checkers! Blacks turn!";
            currentMove = null;
            winner = null;
            turn = "Black";
            MakeBoard();
            CheckersGrid = new Button[9,8];
        }

        private void ClearBoard()
        {
            for (int r = 1; r < 9; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Button button = GetGridElement(CheckersGrid, r, c);
                    button.Name = "none";
                }
            }
        }

        private void MakeBoard()
        {
            for (int r = 1; r < 9; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    CheckersGrid[r,c].Row = r;
                    CheckersGrid[r,c].Column = c;
                    CheckersGrid[r,c].Name = "none";
                }
            }

            MakeButtons();
        }

        private void MakeButtons()
        {
            for (int r = 1; r < 9; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Button button = GetGridElement(CheckersGrid, r, c);
                    switch (r)
                    {
                        case 1:
                            if (c % 2 == 1)
                            {
                                button.Name = "buttonRed" + r + c;
                            }
                            break;
                        case 2:
                            if (c % 2 == 0)
                            {
                                button.Name = "buttonRed" + r + c;
                            }
                            break;
                        case 3:
                            if (c % 2 == 1)
                            {
                                button.Name = "buttonRed" + r + c;
                            }
                            break;
                        case 4:
                            if (c % 2 == 0)
                            {
                                button.Name = "none" + r + c;
                            }
                            break;
                        case 5:
                            if (c % 2 == 1)
                            {
                                button.Name = "none" + r + c;
                            }
                            break;
                        case 6:
                            if (c % 2 == 0)
                            {
                                button.Name = "buttonBlack" + r + c;
                            }
                            break;
                        case 7:
                            if (c % 2 == 1)
                            {
                                button.Name = "buttonBlack" + r + c;
                            }
                            break;
                        case 8:
                            if (c % 2 == 0)
                            {
                                button.Name = "buttonBlack" + r + c;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        Button GetGridElement(Button[,] g, int r, int c)
        {
            return g[r,c];
        }

        void button_Click(Button button)
        {
            int row = button.Row;
            int col = button.Column;
            Debug.Log("Row: " + row + " Column: " + col);
            if (currentMove == null)
                currentMove = new Move();
            if (currentMove.piece1 == null)
            {
                currentMove.piece1 = new Piece(row, col);
            }
            else
            {
                currentMove.piece2 = new Piece(row, col);
            }
            if ((currentMove.piece1 != null) && (currentMove.piece2 != null))
            {
                if (CheckMove())
                {
                    MakeMove();
                    turn = "Red";
                    aiMakeMove();
                    turn = "Black";
                    playerMakeMove();
                    turn = "Red";
                    aiMakeMove();
                    turn = "Black";
                }
            }
        }

        private Boolean CheckMove()
        {
            Button button1 = GetGridElement(CheckersGrid, currentMove.piece1.Row, currentMove.piece1.Column);
            Button button2 = GetGridElement(CheckersGrid, currentMove.piece2.Row, currentMove.piece2.Column);

            if ((turn == "Black") && (button1.Name.Contains("Red")))
            {
                currentMove.piece1 = null;
                currentMove.piece2 = null;
                Debug.Log("It is blacks turn.");
                return false;
            }
            if ((turn == "Red") && (button1.Name.Contains("Black")))
            {
                currentMove.piece1 = null;
                currentMove.piece2 = null;
                Debug.Log("It is reds turn.");
                return false;
            }
            if (button1.Equals(button2))
            {
                currentMove.piece1 = null;
                currentMove.piece2 = null;
                return false;
            }
            if(button1.Name.Contains("Black"))
            {
                return CheckMoveBlack(button1, button2);
            }
            else if (button1.Name.Contains("Red"))
            {
                return CheckMoveRed(button1, button2);
            }
            else
            {
                currentMove.piece1 = null;
                currentMove.piece2 = null;
                Debug.Log("False");
                return false;
            }
        }

        private bool CheckMoveRed(Button button1, Button button2)
        {
            CheckerBoard currentBoard = GetCurrentBoard();
            List<Move> jumpMoves = currentBoard.checkJumps("Red");

            if (jumpMoves.Count > 0)
            {
                bool invalid = true;
                foreach (Move move in jumpMoves)
                {
                    if (currentMove.Equals(move))
                        invalid = false;
                }
                if (invalid)
                {
                    Debug.Log("Jump must be taken");
                    currentMove.piece1 = null;
                    currentMove.piece2 = null;
                    Debug.Log("False");
                    return false;
                }
            }

            if (button1.Name.Contains("Red"))
            {
                if (button1.Name.Contains("King"))
                {
                    if ((currentMove.isAdjacent("King")) && (!button2.Name.Contains("Black")) && (!button2.Name.Contains("Red")))
                        return true;
                    Piece middlePiece = currentMove.checkJump("King");
                    if ((middlePiece != null) && (!button2.Name.Contains("Black")) && (!button2.Name.Contains("Red")))
                    {
                        Button middleButton = GetGridElement(CheckersGrid, middlePiece.Row, middlePiece.Column);
                        if (middleButton.Name.Contains("Black"))
                        {
                            addBlackButton(middlePiece);
                            return true;
                        }
                    }
                }
                else
                {
                    if ((currentMove.isAdjacent("Red")) && (!button2.Name.Contains("Black")) && (!button2.Name.Contains("Red")))
                        return true;
                    Piece middlePiece = currentMove.checkJump("Red");
                    if ((middlePiece != null) && (!button2.Name.Contains("Black")) && (!button2.Name.Contains("Red")))
                    {
                        Button middleButton = GetGridElement(CheckersGrid, middlePiece.Row, middlePiece.Column);
                        if (middleButton.Name.Contains("Black"))
                        {
                            addBlackButton(middlePiece);
                            return true;
                        }
                    }
                }
            }
            currentMove = null;
            Debug.Log("Invalid Move. Try Again.");
            return false;
        }

        private bool CheckMoveBlack(Button button1, Button button2)
        {
            CheckerBoard currentBoard = GetCurrentBoard();
            List<Move> jumpMoves = currentBoard.checkJumps("Black");

            if (jumpMoves.Count > 0)
            {
                bool invalid = true;
                foreach (Move move in jumpMoves)
                {
                    if (currentMove.Equals(move))
                        invalid = false;
                }
                if (invalid)
                {
                    Debug.Log("Jump must be taken");
                    currentMove.piece1 = null;
                    currentMove.piece2 = null;
                    Debug.Log("False");
                    return false;
                }
            }

            if (button1.Name.Contains("Black"))
            {
                if (button1.Name.Contains("King"))
                {
                    if ((currentMove.isAdjacent("King")) && (!button2.Name.Contains("Black")) && (!button2.Name.Contains("Red")))
                        return true;
                    Piece middlePiece = currentMove.checkJump("King");
                    if ((middlePiece != null) && (!button2.Name.Contains("Black")) && (!button2.Name.Contains("Red")))
                    {
                        Button middleButton = GetGridElement(CheckersGrid, middlePiece.Row, middlePiece.Column);
                        if (middleButton.Name.Contains("Red"))
                        {
                            addBlackButton(middlePiece);
                            return true;
                        }
                    }
                }
                else
                {
                    if ((currentMove.isAdjacent("Black")) && (!button2.Name.Contains("Black")) && (!button2.Name.Contains("Red")))
                        return true;
                    Piece middlePiece = currentMove.checkJump("Black");
                    if ((middlePiece != null) && (!button2.Name.Contains("Black")) && (!button2.Name.Contains("Red")))
                    {
                        Button middleButton = GetGridElement(CheckersGrid, middlePiece.Row, middlePiece.Column);
                        if (middleButton.Name.Contains("Red"))
                        {
                            addBlackButton(middlePiece);
                            return true;
                        }
                    }
                }
            }
            currentMove = null;
            Debug.Log("Invalid Move. Try Again.");
            return false;
       }

        private void MakeMove()
        {
            if ((currentMove.piece1 != null) && (currentMove.piece2 != null))
            {
                Debug.Log("Piece1 " + currentMove.piece1.Row + ", " + currentMove.piece1.Column);
                Debug.Log("Piece2 " + currentMove.piece2.Row + ", " + currentMove.piece2.Column);
                Button button1 = GetGridElement(CheckersGrid, currentMove.piece1.Row, currentMove.piece1.Column);
                Button button2 = GetGridElement(CheckersGrid, currentMove.piece2.Row, currentMove.piece2.Column);
                string temp1 = button1.Name;
                //string temp2 = button2.Name;
                button1.Name = "none";
                button2.Name = temp1;

                checkKing(currentMove.piece2);
                currentMove = null;
                if (turn == "Black")
                {
                    this.Title = "Checkers! Reds turn!";
                    turn = "Red";
                }
                else if (turn == "Red")
                {
                    this.Title = "Checkers! Blacks turn!";
                    turn = "Black";
                }
                checkWin();
            }
        }

        private void aiMakeMove()
        {
            currentMove = CheckersAI.GetMove(GetCurrentBoard(), "Red");
            if (currentMove != null)
            {
                if (CheckMove())
                {
                    MakeMove();
                }
            }
        }

        private void playerMakeMove()
        {
            currentMove = CheckersAI.GetMove(GetCurrentBoard(), "Black");
            if (currentMove != null)
            {
                if (CheckMove())
                {
                    MakeMove();
                }
            }
        }

        private CheckerBoard GetCurrentBoard()
        {
            CheckerBoard board = new CheckerBoard();
            for (int r = 1; r < 9; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Button button = GetGridElement(CheckersGrid, r, c);

                    {
                        if (button.Name.Contains("Red"))
                        {
                            if (button.Name.Contains("King"))
                                board.SetState(r - 1, c, 3);
                            else
                                board.SetState(r - 1, c, 1);
                        }
                        else if (button.Name.Contains("Black"))
                        {
                            if (button.Name.Contains("King"))
                                board.SetState(r - 1, c, 4);
                            else
                                board.SetState(r - 1, c, 2);
                        }
                        else
                            board.SetState(r - 1, c, 0);

                    }
                }
            }
            return board;
        }

        private void checkKing(Piece tmpPiece)
        {
            Button button = GetGridElement(CheckersGrid, tmpPiece.Row, tmpPiece.Column);

            {
                if ((button.Name.Contains("Black")) && (!button.Name.Contains("King")))
                {
                    if (tmpPiece.Row == 1)
                    {
                        button.Name = "button" + "Black" + "King" + tmpPiece.Row + tmpPiece.Column;
                    }
                }
                else if ((button.Name.Contains("Red")) && (!button.Name.Contains("King")))
                {
                    if (tmpPiece.Row == 8)
                    {
                        button.Name = "button" + "Red" + "King" + tmpPiece.Row + tmpPiece.Column;
                    }
                }
            }
        }
        
        private void addBlackButton(Piece middleMove)
        {
            // This means block out this section as the piece is removed
            Button button = GetGridElement(CheckersGrid, middleMove.Row, middleMove.Column);
            button.Name = "none" + middleMove.Row + middleMove.Column;
        }

        private void checkWin()
        {
            int totalBlack = 0, totalRed = 0;
            for (int r = 1; r < 9; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Button button = GetGridElement(CheckersGrid, r, c);

                    {
                        if (button.Name.Contains("Red"))
                            totalRed++;
                        if (button.Name.Contains("Black"))
                            totalBlack++;
                    }
                }
            }
            if (totalBlack == 0)
                winner = "Red";
            if (totalRed == 0)
                winner = "Black";
            if (winner != null)
            {
                for (int r = 1; r < 9; r++)
                {
                    for (int c = 0; c < 8; c++)
                    {
                        //Button button = GetGridElement(CheckersGrid, r, c);
                        // N/A
                    }
                }
                string str = winner + " is the winner! Would you like to play another? Call newGame() if so";
                Debug.Log(str);
            }
        }

        private void newGame()
        {
            currentMove = null;
            winner = null;
            this.Title = "Checkers! Blacks turn!";
            turn = "Black";
            ClearBoard();
            MakeBoard();
        }

        private void newGame_Click()
        {
            newGame();
        }

        private void exit_Click()
        {
            Application.Quit();
        }
    }
}