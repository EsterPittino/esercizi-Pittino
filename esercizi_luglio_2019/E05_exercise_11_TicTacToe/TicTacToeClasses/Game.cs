using E05_exercise_11_TicTacToe.IO_Manager;
using System;

namespace E05_exercise_11_TicTacToe.TicTacToeClasses
{
    class TicTacToeGame
    {
        #region Costruttori
        public TicTacToeGame(IIOManager iOManager, Player playerX, Player playerO)
        {
            IOManager = iOManager;
            PlayerX = playerX;
            PlayerO = playerO;
            Board = new Board();
        }
        #endregion

        #region Costanti, campi e proprietà
        private static readonly Random randomizer = new Random();
        private Player ActualPlayer { get; set; }
        private Move ActualMove { get; set; }

        public IIOManager IOManager { get; set; }
        public Board Board { get; }
        public Player PlayerX { get; }
        public Player PlayerO { get; }
        public Player Winner { get; private set; }
        #endregion


        public static TicTacToeGame SetNewGame(ConsoleManager myConsole)
        {
            myConsole.HighNotify("Imposta i giocatori:");
            myConsole.MediumBreak();
            var playerX = Player.SetNewPlayer('X', myConsole);
            myConsole.MediumBreak();
            var playerO = Player.SetNewPlayer('O', myConsole);
            myConsole.LargeBreak();
            myConsole.Wait("Fine impostazione giocatori.");

            return new TicTacToeGame(myConsole, playerX, playerO);
        }

        public void StartNewGame()
        {
            Board.Initialize();
            Winner = null;

            while (Winner is null && !Board.OutOfSpace())
            {
                PrintActualSituation();
                SetActualPlayer();
                PlayNextMove();
                if (ActualMove.IsATicTacToe) Winner = ActualPlayer;
            }

            CloseGame();
        }

        private void PrintActualSituation()
        {
            IOManager.Refresh();

            IOManager.Notify($"{PlayerX} - partite vinte: {PlayerX.Score}");
            IOManager.Notify($"{PlayerO} - partite vinte: {PlayerO.Score}");
            IOManager.LargeBreak();

            IOManager.Notify($"{Board}");
        }

        private void SetActualPlayer()
        {
            if (ActualPlayer == PlayerX)
            {
                ActualPlayer = PlayerO;
            }
            else if (ActualPlayer == PlayerO)
            {
                ActualPlayer = PlayerX;
            }
            else
            {
                switch (randomizer.Next(2))
                {
                    case 0: ActualPlayer = PlayerO; break;
                    case 1: ActualPlayer = PlayerX; break;
                    default: throw new NotImplementedException();
                }
            }

            IOManager.Notify($"E' il turno di {ActualPlayer}");
        }

        private void PlayNextMove()
        {
            if (ActualPlayer.IA) ActualMove = Move.CalculateMove(IOManager, Board, ActualPlayer);
            else ActualMove = Move.AcceptMove(IOManager, Board, ActualPlayer);

            ActualMove.Apply();
        }

        private void CloseGame()
        {
            PrintActualSituation();
            IOManager.LargeBreak();

            if (Winner is null)
            {
                IOManager.HighNotify("Pareggio!");
            }
            else
            {
                IOManager.HighNotify($"Vince {Winner}!");
                Winner.Score++;
            }

            IOManager.LargeBreak();
        }

    }


}
