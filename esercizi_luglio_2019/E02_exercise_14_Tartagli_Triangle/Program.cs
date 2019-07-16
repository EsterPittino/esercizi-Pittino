using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyUtilities;

namespace E02_exercise_14_Tartaglia_Triangle
{
    class Program
    {
        static void Main(string[] args)
        {
            // Implementare un programma che stampi a video il triangolo di Tartaglia fino alla riga n, dove n è un numero inserito dall'utente.

            // Ho scelto questo come primo esercizio perché mi sembra un buon esempio di chiamate ricorsive, e inoltre faccio uso dei Jugged Array

            ConsoleUtilities.PrintTitle("Triangolo di Tartaglia");

            bool goOn = true;
            while (goOn)
            {
                var count = ConsoleUtilities.AcceptIntWithDefault("Quante righe vuoi generare? ",10);
                ConsoleUtilities.PrintSmallSeparator();

                if (count > 0)
                { 
                    var myTriangle = new int[count][];  // JAGGED ARRAY: N righe contententi array di interi di lunghezza variabile
                    GenerateTartagliaTriangle(myTriangle);
                    PrintTartagliaTriangle(myTriangle);
                }
                ConsoleUtilities.PrintSmallSeparator();

                goOn = ConsoleUtilities.AcceptGoOn();
                ConsoleUtilities.PrintBigSeparator();
            }

            ConsoleUtilities.MessageExit();
        }

        private static void GenerateTartagliaTriangle(int[][] actualTriangle)
        {
            var newRow = new int[actualTriangle.Length];    // la nuova riga avrà un numero di elementi pari al numero di riga

            if (actualTriangle.Length == 1)    // caso base: se ho una riga sola, deve contenere un array di un unico valore = 1
            {
                newRow[0] = 1;
            }
            else   // se ho più di una riga il nuovo triangolo sarà uguale al precedente più la riga nuova
            {
                var precTriangle = new int[actualTriangle.Length - 1][];   
                GenerateTartagliaTriangle(precTriangle);                   // genero il triangolo di Tartaglia senza l'ultima riga

                for (int i = 0; i < precTriangle.Length; i++) actualTriangle[i] = precTriangle[i];   // copio il triangolo precedente nell'attuale

                GenerateNewRow(precTriangle[precTriangle.Length - 1], newRow);  // genero la nuova riga basandomi sulla precedente
            }
            
            actualTriangle[actualTriangle.Length - 1] = newRow;  // aggiungo la nuova riga come ultima del triangolo attuale
            return;
        }

        private static void GenerateNewRow(int[] prevRow, int[] newRow)
        {
            for (int i = 0; i < newRow.Length; i++)   // ogni elemento della riga è la somma degli elementi sx e dx della riga precedente
            {
                var sxMember = 0;
                var dxMember = 0;

                if (i > 0) sxMember = prevRow[i - 1];              // per il primo elemento, sxMember rimane = 0
                if (i < prevRow.Length) dxMember = prevRow[i];     // per l'ultimo elemento, dxMember rimane = 0

                newRow[i] = sxMember + dxMember;
            }
        }

        private static void PrintTartagliaTriangle(int[][] tartagliaTriangle)
        {
            foreach (var row in tartagliaTriangle)
            {
                var line = "";
                // Con "ConsoleUtilities.CenterAllign(item.ToString(), 5)" formatto il dato da stampare (item.ToString()) su 5 caratteri, con allineamento centrato
                foreach (var item in row) line += ConsoleUtilities.CenterAllign(item.ToString(), 5) + " ";
                // Con " ConsoleUtilities.PrintCentered(line)" stampo la stringa centrata sulla console
                ConsoleUtilities.PrintCentered(line);
                Console.WriteLine();
            }
        }

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
