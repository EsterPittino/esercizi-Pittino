using System;

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

        private static readonly ConsoleColor _baseForegrounfColor = Console.ForegroundColor;
        #endregion

        // Scrive *title* centrato, evidenziato, e incorniciato
        public static void PrintTitle(string title)
        {
            Console.ForegroundColor = EVIDENCE_COLOR;
            var upperTitle = "*   " + title.ToUpper() + "   *";
            var frame = new string('*', upperTitle.Length);

            PrintCentered(frame);
            PrintCentered(upperTitle);
            PrintCentered(frame);
            Console.WriteLine();

            Console.ForegroundColor = _baseForegrounfColor;
        }

        // Scrive *str* centrato sulla console 
        public static void PrintCentered(string str)
        {
            var strToPrint = CenterAlign(str, Console.BufferWidth - 1);
            Console.WriteLine(strToPrint);
        }

        // Formatta *s* su *length* caratteri, allineandola al centro. 
        // Se la lunghezza di *s* è superiore a *length*, la parte eccedente viente troncata e sostituita da asterischi
        public static string CenterAlign(string s, int length)
        {
            var diff = length - s.Length;
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
            Console.ForegroundColor = EVIDENCE_COLOR;
            var s = new string('=', (Console.BufferWidth - 1));
            Console.WriteLine();
            Console.WriteLine(s);
            Console.WriteLine();

            Console.ForegroundColor = _baseForegrounfColor;
        }

        // Stampa una riga di '-'
        public static void PrintSmallSeparator()
        {
            Console.ForegroundColor = WRITING_COLOR;
            var s = new string('-', (Console.BufferWidth - 1));
            Console.WriteLine(s);

            Console.ForegroundColor = _baseForegrounfColor;
        }

        // Chiede all'utente (con *message*) di inserire una stringa, proponendo un valore di default
        // Nel caso la stringa inserita sua nulla o composta da soli spazi viene restituito *defaultValue*
        public static string AcceptStringWithDefault(string message, string defaultValue)
        {
            Console.ForegroundColor = WRITING_COLOR;
            Console.Write(message);
            var inputPoint = Console.CursorLeft;

            Console.ForegroundColor = HINT_COLOR;
            Console.Write(defaultValue);

            Console.ForegroundColor = READING_COLOR;
            Console.CursorLeft = inputPoint;
            var input = Console.ReadLine();

            Console.ForegroundColor = _baseForegrounfColor;

            if (string.IsNullOrWhiteSpace(input))
                return defaultValue;
            else
                return input;
        }

        // Chiede all'utente (con *message*) di inserire una numero Int, proponendo un valore di default
        // Nel caso la stringa inserita sua nulla o composta da soli spazi viene restituito *defaultValue*
        // Se il valore inserito non è converitibile in Int, ne chiede il re-inserimento
        public static int AcceptIntWithDefault(string message, int defaultValue)
        {
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

                Console.ForegroundColor = _baseForegrounfColor;

                if (string.IsNullOrWhiteSpace(input)) return defaultValue;
                else if (int.TryParse(input, out int value)) return value;
                else
                {
                    Console.ForegroundColor = WRITING_COLOR;
                    Console.WriteLine($"Input non valido; atteso numero integer.");
                    Console.ForegroundColor = _baseForegrounfColor;
                }
            }
        }

        // Chiede all'utente (con *message*) di inserire una numero Double, proponendo un valore di default
        // Nel caso la stringa inserita sua nulla o composta da soli spazi *defaultValue*
        // Se il valore inserito non è converitibile in Double, ne chiede il re-inserimento
        public static double AcceptDoubleWithDefault(string message, double defaultValue)
        {
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
                Console.ForegroundColor = _baseForegrounfColor;

                if (string.IsNullOrWhiteSpace(input)) return defaultValue;
                else if (double.TryParse(input, out double value)) return value;
                else
                {
                    Console.ForegroundColor = WRITING_COLOR;
                    Console.WriteLine($"Input non valido; atteso numero double.");
                    Console.ForegroundColor = _baseForegrounfColor;
                }
            }
        }

        // Chiede all'utente (con *message*) di inserire una numero Decimal, proponendo un valore di default
        // Nel caso la stringa inserita sua nulla o composta da soli spazi *defaultValue*
        // Se il valore inserito non è converitibile in Decimal, ne chiede il re-inserimento
        public static decimal AcceptDecimalWithDefault(string message, decimal defaultValue)
        {
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
                Console.ForegroundColor = _baseForegrounfColor;

                if (string.IsNullOrWhiteSpace(input)) return defaultValue;
                else if (decimal.TryParse(input, out decimal value)) return value;
                else
                {
                    Console.ForegroundColor = WRITING_COLOR;
                    Console.WriteLine($"Input non valido; atteso numero decimal.");
                    Console.ForegroundColor = _baseForegrounfColor;
                }
            }
        }

        // Chiede all'utente se desidera continuare o no
        public static bool AcceptGoOn()
        {
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
                Console.ForegroundColor = _baseForegrounfColor;

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
            Console.ForegroundColor = WRITING_COLOR;
            Console.Write("Finito. Premi un tasto qualunque per uscire.");
            Console.Read();

            Console.ForegroundColor = _baseForegrounfColor;
        }
    }
}

