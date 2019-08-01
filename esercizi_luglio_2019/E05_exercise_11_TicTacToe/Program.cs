using E05_exercise_11_TicTacToe.TicTacToeClasses;
using E05_exercise_11_TicTacToe.IO_Manager;

class Program
{
    static void Main(string[] args)
    {
        var myConsole = new ConsoleManager("Tria");
        var ticTacToe = TicTacToeGame.SetNewGame(myConsole);

        bool goOn = true;
        while (goOn)
        {
            ticTacToe.StartNewGame();
            goOn = myConsole.AcceptBool("Un'altra partita? (s/n): ", true);
        }

        myConsole.LittleBreak();
        myConsole.Wait("Fine.");
    }
}