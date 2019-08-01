using E05_exercise_11_TicTacToe.IO_Manager;

namespace E05_exercise_11_TicTacToe.TicTacToeClasses
{

    class Player
    {
        #region Costruttori
        public Player(string name, char sign, bool ia)
        {
            Sign = sign;
            IA = ia;
            Name = name;
            Score = 0;
        }
        #endregion

        #region Costanti, campi e proprietà
        public char Sign { get; }
        public bool IA { get; }
        public string Name { get; }
        public int Score { get; set; }
        #endregion

        public override string ToString() => (IA ? $"[{Name}] " : $"{Name} ") + $"(simbolo {Sign})";

        public static Player SetNewPlayer(char sign, IIOManager myIO)
        {
            myIO.Notify($"Impostazioni per il giocatore {sign}");
            myIO.LittleBreak();
            var ia = myIO.AcceptBool("Il giocatore è controllato dal computer (s/n): ", false);
            var name = myIO.AcceptString("Nome del giocatore: ", $"Player {sign}");
            myIO.LittleBreak();

            var player = new Player(name, sign, ia);
            myIO.Notify($"Ok, generato giocatore {player}");

            return player;
        }
    }
}
