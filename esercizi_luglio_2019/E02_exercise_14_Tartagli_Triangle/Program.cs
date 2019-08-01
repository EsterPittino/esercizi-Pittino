using System;
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
                foreach (var item in row) line += ConsoleUtilities.CenterAlign(item.ToString(), 5) + " ";
                // Con " ConsoleUtilities.PrintCentered(line)" stampo la stringa centrata sulla console
                ConsoleUtilities.PrintCentered(line);
                Console.WriteLine();
            }
        }

    }

}