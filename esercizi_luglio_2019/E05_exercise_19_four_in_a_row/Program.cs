using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyUtilities;

namespace E05_exercise_19_four_in_a_row
{
    // ## 19 - FOUR IN A ROW
    // 
    // Create a `Game` class, with a grid 6x7.
    // 
    // Two players choose alternatively a column to put a coin in.
    // 
    // The player that get to put a 4-coins alignment(row, columnm, diagonal) wins.
    // 
    // Realize the game with the same logic of the `Chess` game:
    // - there is a `run()` method that has an infinite loop and asks to the current player which column he wants the coin to be put in, and the coin is put on that column in the lowest row possible.
    // - at every turn, the grid is printed
    // 
    // Implement the algorithm that finds if there is a 4-coins alignement. In that case, the while breaks and the winner is printed in the console.
    // 
    // Every input of a player is "safe": if the player enters a column that does not exist (-3, or 14 for example) the Console keeps asking for a correct column.A column that has already filled all its 6 rows is also considered invalid as choice.

    // Ho scelto questo come secondo esercizio perché inizia ad usare le classi ma senza tante complicazioni

    class Program
    {
        static void Main(string[] args)
        {
            // Ho impostato il titolo sulla finestra di Console invece che usare il mio ConsoleUtilities.PrintTitle
            // perché poi faccio il clear della schermata ad ogni turno (per creare l'illusione del momivmento)
            Console.Title = "Forza Quattro!";

            bool goOn = true;
            while (goOn)
            {
                var myGame = new Game(6, 7);
                myGame.Run();

                ConsoleUtilities.PrintSmallSeparator();
                goOn = ConsoleUtilities.AcceptGoOn();
                ConsoleUtilities.PrintBigSeparator();
            }

            ConsoleUtilities.MessageExit();
        }
    }


    class Game
    {
        #region costruttori
        // Ho scelto di riferirmi alla dimensione verticale della griglia come a "profondità" invece che "riga"
        // perchè la griglia del forza4 funziona dal basso verso l'alto
        public Game(int deep, int columns)
        {
            this.grid = new CoinType[deep,columns];   // tutte le caselle della griglia vengono inizializzate a 'NoOne'
        }
        #endregion

        #region campi
        private CoinType[,] grid;        // una volta creata la griglia di gioco, non è più modificabile dall'esterno
        private CoinType actualPlayer;   // ho bisogno di tenere traccia del giocatore in turno
        #endregion

        #region proprietà
        // Scrivendo il codice mi seono accorta che spesso avevo bisogno di riferirmi all'ultimo indice per 
        // profondità o colonne; usare le proprietà mi sembra renda tutto più leggibile
        // Li ho lasciati public, anche se in effetti li uso solo all'interno della classe, perché logicamente 
        // dall'esterno posso vedere quanto è grande la griglia (ma ovviamente non posso modificarla)
        // P.s. la sintassi me l'ha suggerita C#, io avevo scritto:
        // public int MaxDeep { private get { return this.grid.GetLength(0) - 1; } }
        public int MaxDeep => this.grid.GetLength(0) - 1;
        public int MaxColumn => this.grid.GetLength(1) - 1;
        public int DeepCount => this.grid.GetLength(0);
        public int ColumnCount => this.grid.GetLength(1);
        #endregion

        public void Run()
        {
            var gameEnded = false;
            var winner = CoinType.NoOne;

            while (!gameEnded)
            {
                // stampo la situazione attuale
                Console.Clear();
                Console.WriteLine(this.ToString());

                // accetto la prossima mossa
                SetActualPlayer();
                var colNumber = AcceptNextMove(out int freePosition);
                this.grid[freePosition, colNumber] = actualPlayer;

                // verifico se con l'ultima mossa è stato creato un allineamento di 4 gettoni
                if (CheckFour(freePosition, colNumber))
                {
                    winner = actualPlayer;
                    gameEnded = true;
                }

                // verifico se ho ancora spazio di gioco nella griglia
                if (OutOfSpace()) gameEnded = true;
            }

            // all'uscita dal ciclo, stampa la situazione finale
            Console.Clear();
            Console.WriteLine(this.ToString());
            if (winner == CoinType.NoOne) Console.WriteLine($"Partita finita in pareggio.");
            else Console.WriteLine($"Quattro in fila! Il giocatore {actualPlayer.ToString()} vince!");
            Console.WriteLine();
        }

        public override string ToString()
        {
            var gridToString = "\r\n";

            // contenuto della griglia
            for (int deep = 0; deep <= this.MaxDeep; deep++) 
            {
                for (int column = 0; column <= this.MaxColumn; column++)
                {
                    var box = grid[deep, column];
                    switch (box)
                    {
                        case CoinType.NoOne:
                            gridToString += " - ";
                            break;
                        case CoinType.X:      
                        case CoinType.O:
                            gridToString += $" {box.ToString()} ";
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
                gridToString += "\r\n";
            }

            // riga di separazione
            gridToString += new string ('=', this.ColumnCount * 3) + "\r\n"; 

            // indicatori di colonna
            for (int i = 1; i <= this.ColumnCount; i++)
            {
                gridToString += $" {i} ";
            }
            gridToString += "\r\n";

            return gridToString;
        }

        private void SetActualPlayer()
        {
            switch (actualPlayer)
            {
                case CoinType.NoOne:
                case CoinType.X:
                    actualPlayer = CoinType.O;
                    break;
                case CoinType.O:
                    actualPlayer = CoinType.X;
                    break;
            }

            Console.WriteLine($"E' il turno del giocatore {actualPlayer.ToString()}");
        }

        private int AcceptNextMove(out int freePosition)
        {
            while (true)
            {
                Console.Write("Inserisci la colonna nella quale infilare il tuo gettone: ");
                var input = Console.ReadLine();

                if (int.TryParse(input, out int value))   // l'input deve essere convertibile in int
                {
                    if ((value >= 1) && (value <= this.ColumnCount))  // l'input deve corrispondere a un numero di colonna
                    {
                        var colNumber = value - 1;
                        freePosition = FindFreePosition(colNumber);  // FindFreePosition restituisce la più bassa casella libera nella colonna, o -1 se la colonna è piena

                        if (freePosition >= 0) return colNumber;     // la colonna indicata deve avere ancora spazio disponibile
                        else Console.WriteLine($"Input non valido; la colonna {value} è già colma.");
                    }
                    else  Console.WriteLine($"Input non valido; atteso numero integer tra 1 e {this.ColumnCount}.");
                }
                else Console.WriteLine($"Input non valido; atteso numero integer tra 1 e {this.ColumnCount}.");
            }
        }

        private int FindFreePosition(int colNumber)
        {
            // scorro la colonna fino a trovare la prima posizione libera
            for (int deep = this.MaxDeep; deep >= 0; deep--)
            {
                if (this.grid[deep, colNumber] == CoinType.NoOne) return deep;
            }
            return -1;
        }

        private bool CheckFour(int deep, int colNumber)   
        // passo al metodo le coordinate dell'ultima mossa: non ho bisogno di controllare tutta la griglia per sapere se ho vinto,
        // mi basta verificare se ho fatto un 4 con l'ultimo gettone inserito
        {
            int count;

            // controllo in orizzontale: conto quanti gettoni a destra e a sinistra sono uguali a quello della mossa corrente
            count = 1;   // inizio contando se stesso
            count += CountLeadingCoins(deep, colNumber, Direction.Left);
            count += CountLeadingCoins(deep, colNumber, Direction.Right);
            if (count >= 4) return true;
            
            // controllo in verticale: conto quanti gettoni sotto sono uguali a quello della mossa corrente (non è possibile che ce ne siano sopra)
            count = 1;   // inizio contando se stesso
            count += CountLeadingCoins(deep, colNumber, Direction.Down);
            if (count >= 4) return true;
            
            // controllo in diagonale ascendente
            count = 1;   // inizio contando se stesso
            count += CountLeadingCoins(deep, colNumber, Direction.LowerLeft);
            count += CountLeadingCoins(deep, colNumber, Direction.TopRight);
            if (count >= 4) return true;

            // controllo in diagonale discendente
            count = 1;   // inizio contando se stesso
            count += CountLeadingCoins(deep, colNumber, Direction.TopLeft);
            count += CountLeadingCoins(deep, colNumber, Direction.LowerRight);
            if (count >= 4) return true;

            // se non sono in nessuno dei casi precedenti, non ho fatto 4
            return false;
        }

        private int CountLeadingCoins(int deep, int colNumber, Direction direction)
        {
            var coinChecked = this.grid[deep, colNumber];
            var count = 0;

            // per ogni direzione, imposto un "modificatore di movimento" sull'asse X e Y: 
            // 1 se devo avanzare, 0 se devo restare fermo, -1 se devo indietreggiare
            int orizzontalModifier;
            int verticalModifier;
            switch (direction)
            {
                case Direction.Up:
                    orizzontalModifier = 0;
                    verticalModifier   = -1;
                    break;
                case Direction.Down:
                    orizzontalModifier = 0;
                    verticalModifier   = 1;
                    break;
                case Direction.Left:
                    orizzontalModifier = -1;
                    verticalModifier   = 0;
                    break;
                case Direction.Right:
                    orizzontalModifier = 1;
                    verticalModifier   = 0;
                    break;
                case Direction.LowerLeft:
                    orizzontalModifier = -1;
                    verticalModifier   = 1;
                    break;
                case Direction.TopLeft:
                    orizzontalModifier = -1;
                    verticalModifier   = -1;
                    break;
                case Direction.LowerRight:
                    orizzontalModifier = 1;
                    verticalModifier   = 1;
                    break;
                case Direction.TopRight:
                    orizzontalModifier = 1;
                    verticalModifier   = -1;
                    break;
                default:
                    throw new NotImplementedException();
            }

            // avanzo di una posizione alla volta, in base alla direzione indicata, finchè
            // non finisco la griglia oppure trovo una casella con gettone diverso da quello di partenza
            int shift = 0;
            while (true)
            {
                shift++;
                
                int colChecked = colNumber + (shift * orizzontalModifier);
                int deepChecked = deep + (shift * verticalModifier);

                if ((colChecked < 0) || (colChecked > this.MaxColumn)) break;  
                if ((deepChecked < 0) || (deepChecked > this.MaxDeep)) break;

                if (this.grid[deepChecked, colChecked] == coinChecked) count++;
                else break;
            }

            return count;
        }

        private bool OutOfSpace()
        {
            for (int col = 0; col <= MaxColumn; col++)
            {
                if (FindFreePosition(col) >= 0) return false;
            }

            return true;
        }
    }

    enum CoinType
    {
        NoOne,
        X,
        O,
    }

    enum Direction
    {
        Unknown,
        Up,
        Down,
        Left,
        Right,
        LowerLeft,
        TopLeft,
        LowerRight,
        TopRight,
    }
}



namespace MyUtilities
{
    // Ho creato una serie di utilities per la gestione della console
    class ConsoleUtilities
    {
        #region costanti ConsoleColor
        const ConsoleColor WRITING_COLOR = ConsoleColor.Gray;
        const ConsoleColor READING_COLOR = ConsoleColor.Cyan;
        const ConsoleColor HINT_COLOR = ConsoleColor.DarkGray;
        const ConsoleColor EVIDENCE_COLOR = ConsoleColor.White;
        #endregion

        // Scrive *title* centrato, evidenziato, e incorniciato
        public static void PrintTitle(string title)
        {
            var actualColor = Console.ForegroundColor;

            Console.ForegroundColor = EVIDENCE_COLOR;
            var upperTitle = "*   " + title.ToUpper() + "   *";
            var frame = new string('*', upperTitle.Length);

            PrintCentered(frame);
            PrintCentered(upperTitle);
            PrintCentered(frame);
            Console.WriteLine();

            Console.ForegroundColor = actualColor;
        }

        // Scrive *str* centrato sulla console 
        public static void PrintCentered(string str)
        {
            var strToPrint = CenterAllign(str, Console.BufferWidth - 1);
            Console.WriteLine(strToPrint);
        }

        // Formatta *s* su *lenght* caratteri, allineandola al centro. 
        // Se la lunghezza di *s* è superiore a *lenght*, la parte eccedente viente troncata e sostituita da asterischi
        public static string CenterAllign(string s, int lenght)
        {
            var diff = lenght - s.Length;
            var absDiff = Math.Abs(diff);
            var sxDelta = (absDiff / 2);
            var dxDelta = (absDiff / 2) + (absDiff % 2); // se la differenza è dispari, aggiungo uno a destra

            if (diff == 0)
            {
                return s;
            }
            else if (diff > 0)
            {
                var sxIndent = new string(' ', sxDelta);
                var dxIndent = new string(' ', dxDelta);
                return sxIndent + s + dxIndent;
            }
            else
            {
                var substrStart = sxDelta;
                var substrLen = s.Length - substrStart - dxDelta - 2; // tolgo altre due posizioni per gli '*' in testa e in coda
                return "*" + s.Substring(substrStart, substrLen) + "*";
            }

        }

        // Stampa una riga di '=' evidenziati
        public static void PrintBigSeparator()
        {
            var actualColor = Console.ForegroundColor;

            Console.ForegroundColor = EVIDENCE_COLOR;
            var s = new string('=', (Console.BufferWidth - 1));
            Console.WriteLine();
            Console.WriteLine(s);
            Console.WriteLine();

            Console.ForegroundColor = actualColor;
        }

        // Stampa una riga di '-'
        public static void PrintSmallSeparator()
        {
            var actualColor = Console.ForegroundColor;

            Console.ForegroundColor = WRITING_COLOR;
            var s = new string('-', (Console.BufferWidth - 1));
            Console.WriteLine(s);

            Console.ForegroundColor = actualColor;
        }

        // Chiede all'utente (con *message*) di inserire una stringa, proponendo un valore di default
        // Nel caso la stringa inserita sua nulla o composta da soli spazi viene restituito *defaultValue*
        public static string AcceptStringWithDefault(string message, string defaultValue)
        {
            var actualColor = Console.ForegroundColor;

            Console.ForegroundColor = WRITING_COLOR;
            Console.Write(message);
            var inputPoint = Console.CursorLeft;

            Console.ForegroundColor = HINT_COLOR;
            Console.Write(defaultValue);

            Console.ForegroundColor = READING_COLOR;
            Console.CursorLeft = inputPoint;
            var input = Console.ReadLine();

            Console.ForegroundColor = actualColor;

            if (string.IsNullOrWhiteSpace(input)) return defaultValue;
            else return input;
        }

        // Chiede all'utente (con *message*) di inserire una numero Int, proponendo un valore di default
        // Nel caso la stringa inserita sua nulla o composta da soli spazi viene restituito *defaultValue*
        // Se il valore inserito non è converitibile in Int, ne chiede il re-inserimento
        public static int AcceptIntWithDefault(string message, int defaultValue)
        {
            var actualColor = Console.ForegroundColor;

            while (true)
            {
                Console.ForegroundColor = WRITING_COLOR;
                Console.Write(message);
                var inputPoint = Console.CursorLeft;

                Console.ForegroundColor = HINT_COLOR;
                Console.Write(defaultValue);

                Console.ForegroundColor = READING_COLOR;
                Console.CursorLeft = inputPoint;
                var input = Console.ReadLine();

                Console.ForegroundColor = actualColor;

                if (string.IsNullOrWhiteSpace(input)) return defaultValue;
                else if (int.TryParse(input, out int value)) return value;
                else
                {
                    Console.ForegroundColor = WRITING_COLOR;
                    Console.WriteLine($"Input non valido; atteso numero integer.");
                    Console.ForegroundColor = actualColor;
                }
            }
        }
            
        // Chiede all'utente (con *message*) di inserire una numero Double, proponendo un valore di default
        // Nel caso la stringa inserita sua nulla o composta da soli spazi *defaultValue*
        // Se il valore inserito non è converitibile in Double, ne chiede il re-inserimento
        public static double AcceptDoubleWithDefault(string message, double defaultValue)
            {
                var actualColor = Console.ForegroundColor;

                while (true)
                {
                    Console.ForegroundColor = WRITING_COLOR;
                    Console.Write(message);
                    var inputPoint = Console.CursorLeft;

                    Console.ForegroundColor = HINT_COLOR;
                    Console.Write(defaultValue);

                    Console.ForegroundColor = READING_COLOR;
                    Console.CursorLeft = inputPoint;
                    var input = Console.ReadLine();
                    Console.ForegroundColor = actualColor;

                    if (string.IsNullOrWhiteSpace(input)) return defaultValue;
                    else if (double.TryParse(input, out double value)) return value;
                    else
                    {
                        Console.ForegroundColor = WRITING_COLOR;
                        Console.WriteLine($"Input non valido; atteso numero double.");
                        Console.ForegroundColor = actualColor;
                    }
                }
            }

        // Chiede all'utente (con *message*) di inserire una numero Decimal, proponendo un valore di default
        // Nel caso la stringa inserita sua nulla o composta da soli spazi *defaultValue*
        // Se il valore inserito non è converitibile in Decimal, ne chiede il re-inserimento
        public static decimal AcceptDecimalWithDefault(string message, decimal defaultValue)
        {
            var actualColor = Console.ForegroundColor;

            while (true)
            {
                Console.ForegroundColor = WRITING_COLOR;
                Console.Write(message);
                var inputPoint = Console.CursorLeft;

                Console.ForegroundColor = HINT_COLOR;
                Console.Write(defaultValue);

                Console.ForegroundColor = READING_COLOR;
                Console.CursorLeft = inputPoint;
                var input = Console.ReadLine();
                Console.ForegroundColor = actualColor;

                if (string.IsNullOrWhiteSpace(input)) return defaultValue;
                else if (decimal.TryParse(input, out decimal value)) return value;
                else
                {
                    Console.ForegroundColor = WRITING_COLOR;
                    Console.WriteLine($"Input non valido; atteso numero decimal.");
                    Console.ForegroundColor = actualColor;
                }
            }
        }

        // Chiede all'utente se desidera continuare o no
        public static bool AcceptGoOn()
        {
            var actualColor = Console.ForegroundColor;

            while (true)
            {
                Console.ForegroundColor = WRITING_COLOR;
                Console.Write("Continuare (s/n)? ");
                var inputPoint = Console.CursorLeft;

                Console.ForegroundColor = HINT_COLOR;
                Console.Write("s");

                Console.ForegroundColor = READING_COLOR;
                Console.CursorLeft = inputPoint;
                string goOn = Console.ReadLine();
                Console.ForegroundColor = actualColor;

                switch (goOn)
                {
                    case "":
                    case "s":
                    case "S":
                        return true;

                    case "n":
                    case "N":
                        return false;

                    default:
                        Console.WriteLine("Risposta non valida; atteso 's'/'n'/''.");
                        break;
                }
            }
        }

        // Informa l'utente della conclusione del programma
        public static void MessageExit()
        {
            var actualColor = Console.ForegroundColor;

            Console.ForegroundColor = WRITING_COLOR;
            Console.Write("Finito. Premi un tasto qualunque per uscire.");
            Console.Read();

            Console.ForegroundColor = actualColor;
        }
    }
}