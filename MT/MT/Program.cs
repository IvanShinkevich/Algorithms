using System;
using System.Text;
using System.Text.RegularExpressions;

namespace MT
{
    class Program
    {
        private static string Alphabet = "ab";
        private static string TableOfConditions = "CurState:Q1 CurSym:a SymToReplace:a Move:1 NextState:Q1 " +
"CurState:Q1 CurSym:b SymToReplace:a Move:1 NextState:Q-1 " +
"CurState:Q1 CurSym:e SymToReplace:e Move:0 NextState:Q0";
        public struct Iteration
        {
            public char Symbol;
            public int Step;
            public int StateNumber;

            public Iteration(char symbol, int step, int stateNumber)
            {
                Symbol = symbol;
                Step = step;
                StateNumber = stateNumber;
            }
        }

        private static Iteration GetIteration(int currState, char currSymbol)
        {
            Regex reg = new Regex($@"CurState:Q{currState} CurSym:{currSymbol} SymToReplace:(\S*) Move:(\S*) NextState:Q(\S*)");
            MatchCollection matches = reg.Matches(TableOfConditions);
            if (matches.Count == 1)
            {
                if (matches[0].Groups.Count == 4)
                {
                    return new Iteration(matches[0].Groups[1].Value[0],
                        Convert.ToInt32(matches[0].Groups[2].Value),
                        Convert.ToInt32(matches[0].Groups[3].Value));
                }
            }

            return new Iteration();
        }

        private static int GetCurrentSymbolPos(int stepDirective, int symPos)
        {
            switch (stepDirective)
            {
                case 0:
                    break;
                case 1:
                    symPos++;
                    break;
                case 2:
                    symPos--;
                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }

            return symPos;
        }

        private static void MT(string word)
        {
            int currState = 1;
            int currSymPos = 1;
            StringBuilder currString = new StringBuilder($"e{word}e");
            while (currState != 0 && currState != -1)
            {

                var iteration = GetIteration(currState, currString[currSymPos]);
                currString[currSymPos] = iteration.Symbol;
                currState = iteration.StateNumber;
                currSymPos = GetCurrentSymbolPos(iteration.Step, currSymPos);
            }

            if (currState == 0) Console.WriteLine("True");
            else Console.WriteLine("False");

            Console.WriteLine(currString);
        }

        private static bool IsWordValid(string word)
        {
            foreach (char c in word)
            {
                if (!Alphabet.Contains(c.ToString()))
                {
                    Console.WriteLine("Word contains symbols not listed in alphabet. Please, enter correct word");
                    return false;
                }
            }

            return true;
        }

        static void Main(string[] args)
        {
            bool isWordValid = false;
            string word = String.Empty;
            Console.WriteLine("Enter the word, please:");
            while (!isWordValid)
            {
                word = Console.ReadLine();
                isWordValid = IsWordValid(word);
            }

            MT(word);

            Console.ReadKey();
        }
    }
}
