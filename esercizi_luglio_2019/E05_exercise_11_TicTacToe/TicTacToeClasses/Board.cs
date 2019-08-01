namespace E05_exercise_11_TicTacToe.TicTacToeClasses
{
    class Board
    {
        #region Costruttori
        public Board()
        {
            Grid = new char[NUMBER_OF_ROWS, NUMBER_OF_COLS];
        }
        #endregion

        #region Costanti, campi e proprietà
        const int INITIAL_ROW = 'a';
        const int INITIAL_COL = 1;
        const int NUMBER_OF_ROWS = 3;
        const int NUMBER_OF_COLS = 3;

        public char[,] Grid { get; private set; }

        public int NumberOfRows => NUMBER_OF_ROWS;
        public int NumberOfCols => NUMBER_OF_COLS;
        public int InitialRow => INITIAL_ROW;
        public int InitialCol => INITIAL_COL;
        public int LastRow => INITIAL_ROW + NUMBER_OF_ROWS - 1;
        public int LastCol => INITIAL_COL + NUMBER_OF_COLS - 1;
        #endregion

        public override string ToString()
        {
            var colHeading = "   ";
            for (int col = 0; col < NUMBER_OF_COLS; col++)
            {
                colHeading += $" {INITIAL_COL + col}  ";
            }

            var rowBreak = "  +";
            for (int col = 0; col < NUMBER_OF_COLS; col++)
            {
                rowBreak += $"---+";
            }

            var boardInString = colHeading + "\r\n" + rowBreak + "\r\n";

            for (int row = 0; row < NUMBER_OF_ROWS; row++)
            {
                var line = $"{(char)(INITIAL_ROW + row)} |";
                for (int col = 0; col < NUMBER_OF_COLS; col++)
                {
                    var cell = Grid[row, col];
                    line += $" {cell} |";
                }
                boardInString += line + "\r\n" + rowBreak + "\r\n";
            }

            return boardInString;
        }

        public void Initialize() => Grid = new char[NUMBER_OF_ROWS, NUMBER_OF_COLS];

        public bool OutOfSpace()
        {
            for (int row = 0; row < NUMBER_OF_ROWS; row++)
            {
                for (int col = 0; col < NUMBER_OF_COLS; col++)
                {
                    if (Grid[row, col] == (char)0)
                    {
                        return false;
                    }
                }
            }
            
            // se arrivo qui, non ho trovato nemmeno una cella vuota nella griglia
            return true;
        }       
    }
}