using System;

namespace E05_exercise_11_TicTacToe.IO_Manager
{
    class ConsoleManager : IIOManager
    {
        #region Costruttori
        public ConsoleManager(string title)
        {
            Console.Title = title;
        }
        #endregion

        #region Costanti, Campi e Proprietà
        const ConsoleColor READING_COLOR = ConsoleColor.Cyan;
        const ConsoleColor NORMAL_COLOR = ConsoleColor.Gray;
        const ConsoleColor LOW_COLOR = ConsoleColor.DarkGray;
        const ConsoleColor EVIDENCE_COLOR = ConsoleColor.White;
        
        private static readonly ConsoleColor _baseForegroundColor = Console.ForegroundColor;

        private int LineLenght => Console.BufferWidth - 1;
        #endregion

        #region Output
        public void Refresh() => Console.Clear();

        public void Notify(string message) => Console.WriteLine(message);
        public void HighNotify(string message)
        {
            Console.ForegroundColor = EVIDENCE_COLOR;
            Notify(message);
            Console.ForegroundColor = _baseForegroundColor;
        }
        public void LowNotify(string message)
        {
            Console.ForegroundColor = LOW_COLOR;
            Notify(message);
            Console.ForegroundColor = _baseForegroundColor;
        }

        public void LittleBreak() => Console.WriteLine(); // stampa una riga vuota
        public void MediumBreak()  // stampa una riga di '-' con una riga vuota sopra e sotto
        {
            LittleBreak();
            Notify(new string('-', LineLenght));
            LittleBreak();
        }
        public void LargeBreak() // stampa una riga di '=' evidenziati, con una riga vuota sopra e sotto
        {
            LittleBreak();
            HighNotify(new string('=', LineLenght));
            LittleBreak();
        }
        #endregion

        #region Attesa
        public void Wait() => Wait("");   // se chiamo il wait senza parametri, è come se lo chiamassi con stringa vuota
        public void Wait(string message)  // stampa il messaggio e aspetta un read
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                Notify(message);
            }
            LowNotify("Premi un tasto qualsiasi per continuare.");
            Console.ReadLine();
        }
        #endregion

        #region Input
        public string AcceptString(string message, string defaultValue)
        {
            while (true)
            {
                var input = AskValue(message, defaultValue);

                if (string.IsNullOrEmpty(input))
                {
                    return defaultValue;
                }
                else
                {
                    return input;
                }
            }
        }

        public bool AcceptBool(string message, bool defaultValue)
        {
            while (true)
            {
                var defaultValueToString = defaultValue ? "s" : "n";
                var input = AskValue(message, defaultValueToString);

                if (string.IsNullOrEmpty(input)) return defaultValue;
            
                switch (input)
                {
                    case "s":
                    case "S":
                        return true;
                    case "n":
                    case "N":
                        return false;
                    default:
                        Notify("Input non valido; atteso s/n.");
                        break;
                }
            }
        }

        public int AcceptInt(string message, int defaultValue)
        {
            while (true)
            {
                var input = AskValue(message, defaultValue.ToString());

                if (string.IsNullOrEmpty(input)) return defaultValue;


                if (int.TryParse(input, out int value)) return value;
                else Notify("Input non valido; atteso numero integer.");
            }
        }
        
        private static string AskValue(string message, string defaultValue)
        {
            Console.Write(message);
            var inputPoint = Console.CursorLeft;

            Console.ForegroundColor = LOW_COLOR;
            Console.Write(defaultValue);
            Console.CursorLeft = inputPoint;

            Console.ForegroundColor = READING_COLOR;
            var input = Console.ReadLine();

            Console.ForegroundColor = _baseForegroundColor;

            return input;
        }
        #endregion
    }
}
