using E05_exercise_11_TicTacToe.IO_Manager;
using System;
using System.Collections.Generic;
using System.Linq;

namespace E05_exercise_11_TicTacToe.TicTacToeClasses
{
    class Move
    {
        #region Costruttori
        public Move(Board board, Player player, int row, int col)
        {
            Board = board;
            Player = player;
            Row = row;
            Col = col;
            Rating = 0;
            IsATicTacToe = false;
        }
        #endregion

        #region Costanti, campi e proprietà
        private static readonly Random randomizer = new Random();

        public Board Board { get; }       // una mossa ha significato in riferimento a una specifica griglia di gioco
        public Player Player { get; }
        public int Row { get; }
        public int Col { get; }
        public int Rating { get; private set; }
        public bool IsATicTacToe { get; private set; }
        #endregion

        public override string ToString() => $"{(char)(Board.InitialRow + Row)}{Board.InitialCol + Col}";

        public static Move CalculateMove(IIOManager myIO, Board board, Player player)
        {
            var possibleMoves = PossibleMoves(board, player);  // passo anche il giocatore perché calcolo il rating per ogni mossa relativamente a quel giocatore

            var maxRating = possibleMoves.Max(x => x.Rating);
            var bestMoves = possibleMoves.FindAll(x => x.Rating == maxRating);
            var chioicedMove = bestMoves.ElementAt(randomizer.Next(bestMoves.Count()));

            myIO.Notify($"{player} muove in {chioicedMove}.");
            myIO.LittleBreak();

            myIO.Wait();

            return chioicedMove;
        }

        private static List<Move> PossibleMoves(Board board, Player player)
        {
            var possibleMoves = new List<Move>();
            for (int row = 0; row < board.NumberOfRows; row++)
            {
                for (int col = 0; col < board.NumberOfCols; col++)
                {
                    var move = new Move(board, player, row, col);
                    move.Evaluate();
                    if (move.Rating >= 0)
                    {
                        possibleMoves.Add(move);
                    }
                }
            }
            return possibleMoves;
        }

        public static Move AcceptMove(IIOManager myIO, Board board, Player player)
        {
            string inputCoordinates;

            while (true)
            {
                inputCoordinates = myIO.AcceptString("Inserisci la tua mossa (es. 'a1'): ", "");
                var move = ValidateMove(myIO, board, player, inputCoordinates);
                if (move != null)
                {
                    return move;
                }
            }
        }

        private static Move ValidateMove(IIOManager myIO, Board board, Player player, string inputCoordinates)
        {
            // rimuovo eventuali spazi, e controllo che la stringa passata contenga esattamente due caratteri
            inputCoordinates = inputCoordinates.Trim(' ');
            if (inputCoordinates.Length != 2)
            {
                myIO.Notify("Input non valido, inserisci le cordinate in formato {char}{int}.");
                return null;
            }

            // l'indice di riga deve essere un indicatore di riga valido (a, b o c)
            char myRow = inputCoordinates.ElementAt(0);
            if (myRow < board.InitialRow || myRow > board.LastRow)
            {
                myIO.Notify($"Input non valido, la prima coordinata (indicatore di riga) deve essere compresa tra '{(char)board.InitialRow}' e '{(char)board.LastRow}'.");
                return null;
            }
            int rowValue = myRow;

            // l'indice di colonna deve essere un indicatore di colonna valido (1, 2 o 3)
            char myCol = inputCoordinates.ElementAt(1);
            if (!int.TryParse(myCol.ToString(), out int colValue))
            {
                myIO.Notify("Input non valido, la seconda coordinata (indicatore di colonna) deve essere numerica.");
                return null;
            }
            if (colValue < board.InitialCol || colValue > board.LastCol)
            {
                myIO.Notify($"Input non valido, la seconda coordinata (indicatore di colonna) deve essere compresa tra {board.InitialCol} e {board.LastCol}.");
                return null;
            }

            // tolgo il valore iniziale dagli indicatori, per ottenere l'indice di matrice
            rowValue -= board.InitialRow;
            colValue -= board.InitialCol;

            // la cella deve essere libera
            if (board.Grid[rowValue, colValue] != (char)0)
            {
                myIO.Notify($"Input non valido, la cella {inputCoordinates} è già occupata.");
                return null;
            }

            // se arrivo qui ho superato tutte le validazioni
            var move = new Move(board, player, rowValue, colValue);
            move.Evaluate();
            return move;
        }

        public void Evaluate()
        {
            // Verifico se la mossa è un Tic Tac Toe
            // Inoltre assegno un valore numerico che indica quanto 'favorevele' è la mossa:
            //  - 0: rating base
            //  - -1: mossa non valida
            //  - rating totale: sommatoria del rating per ogni allineamento possibile (orizzontale, veriticale, diagonale)

            Rating = 0;

            // se indici fuori tabella ==> mossa non valida
            if ((Row < 0) || (Row > Board.NumberOfRows)
               || (Col < 0) || (Col > Board.NumberOfCols))
            {
                Rating = -1;
                return;
            }

            // se cella già occupata ==> mossa non valida
            if (Board.Grid[Row, Col] != (char)0)
            {
                Rating = -1;
                return;
            }


            List<char> allignedCell = new List<char>();

            // verifico l'allineamento nella riga
            allignedCell.Clear();
            for (int col = 0; col < Board.NumberOfCols; col++)  // cerco tutte le caselle nella riga corrente
            {
                if (col != Col) // escludo se stesso
                {
                    var cellValue = Board.Grid[Row, col];
                    allignedCell.Add(cellValue);
                }
            }
            Rating += RateAllign(allignedCell);

            // verifico l'allineamento nella colonna
            allignedCell.Clear();
            for (int row = 0; row < Board.NumberOfRows; row++) // cerco tutte le caselle nella colonna corrente
            {
                if (row != Row) // escludo se stesso
                {
                    var cellValue = Board.Grid[row, Col];
                    allignedCell.Add(cellValue);
                }
            }
            Rating += RateAllign(allignedCell);

            // se la cella fa parte della diagonale discendente, ne verifico l'allineamento
            // nella diagonale discendente, gli indici di riga e colonna sono uguali (a1, b2, c3)
            if (Row == Col)
            {
                allignedCell.Clear();
                for (int row = 0; row < Board.NumberOfRows; row++)
                {
                    if (row != Row)
                    {
                        var col = row;
                        var cellValue = Board.Grid[row, col];
                        allignedCell.Add(cellValue);
                    }
                }
                Rating += RateAllign(allignedCell);
            }

            // se la cella fa parte della diagonale ascendente, ne verifico l'allineamento
            // nella diagonale ascendente, gli indici di riga e colonna sono inversi (a3, b2, c1)
            if (Row == ((Board.NumberOfRows - 1) - Col))
            {
                allignedCell.Clear();
                for (int row = 0; row < Board.NumberOfRows; row++)
                {
                    if (row != Row)
                    {
                        var col = (Board.NumberOfRows - 1) - row;
                        var cellValue = Board.Grid[row, col];
                        allignedCell.Add(cellValue);
                    }
                }
                Rating += RateAllign(allignedCell);
            }
        }

        private int RateAllign(List<char> allignedCell)
        {
            // Conto nell'allineamento: i segni uguali al giocatore, quelli diversi, e le celle vuote
            var countEqual = 0;
            var countDifferent = 0;
            var countVoid = 0;
            foreach (var cell in allignedCell)
            {
                if (cell == Player.Sign)
                {
                    countEqual++;
                }
                else if (cell == (char)0)
                {
                    countVoid++;
                }
                else
                {
                    countDifferent++;
                }
            }

            // Se ho solo celle del mio allineamento, è una mossa vincente
            if ((countEqual > 0) && (countVoid == 0) && (countDifferent == 0))
            {
                IsATicTacToe = true;
                return 10000;
            }

            // Se ho solo celle diverse, sto bloccando una tria dell'avversario
            if ((countEqual == 0) && (countVoid == 0) && (countDifferent > 0))
            {
                return 1000;
            }

            // Se ci sono solo celle uguali o vuote, sto continuando una tria già iniziata
            if ((countEqual > 0) && (countVoid > 0) && (countDifferent == 0))
            {
                return 100;
            }

            // Se ci sono solo celle diverse o vuote, sto bloccando una tria avversaria
            if ((countEqual == 0) && (countVoid > 0) && (countDifferent > 0))
            {
                return 10;
            }

            // Se tutte le celle sono vuote, sto iniziando una tria
            if ((countEqual == 0) && (countVoid > 0) && (countDifferent == 0))
            {
                return 1;
            }

            // Se ci sono sia simboli uguali che diversi, la mossa è inutile (non posso fare tria, né bloccare l'avversario)
            if ((countEqual > 0) && (countDifferent > 0)) return 0;

            // se arrivo qui, mi sono persa qualche caso
            throw new NotImplementedException();
        }

        public void Apply() => Board.Grid[Row, Col] = Player.Sign;
    }
}
