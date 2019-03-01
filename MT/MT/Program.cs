using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MT
{
    class Program
    {
        private static int TaskNumber;

        private static List<int> TasksAvailable = new List<int>
        {
            119, 264
        };

        private static Dictionary<int, string> Alphabets = new Dictionary<int, string>()
        {
            {119,  "210"},
            {264,  "()"}
        };

        private static Dictionary<int, string> TableOfConditions = new Dictionary<int, string>()
        {
            {119, "CurState:Q1 CurSym:0 SymToReplace:0 Move:1 NextState:Q1 " +
                  "CurState:Q1 CurSym:2 SymToReplace:2 Move:1 NextState:Q1 " +
                  "CurState:Q1 CurSym:1 SymToReplace:1 Move:1 NextState:Q2 " +
                  "CurState:Q2 CurSym:0 SymToReplace:0 Move:1 NextState:Q2 " +
                  "CurState:Q2 CurSym:2 SymToReplace:2 Move:1 NextState:Q2 " +
                  "CurState:Q2 CurSym:1 SymToReplace:1 Move:1 NextState:Q1 " +
                  "CurState:Q1 CurSym:e SymToReplace:e Move:0 NextState:Q0 " +
                  "CurState:Q2 CurSym:e SymToReplace:e Move:0 NextState:Q-1"
            },
            {264,  "CurState:Q1 CurSym:( SymToReplace:A Move:1 NextState:Q2 " +
                   "CurState:Q1 CurSym:A SymToReplace:A Move:1 NextState:Q1 " +
                   "CurState:Q2 CurSym:( SymToReplace:( Move:1 NextState:Q2 " +
                   "CurState:Q2 CurSym:A SymToReplace:A Move:1 NextState:Q2 " +
                   "CurState:Q2 CurSym:) SymToReplace:A Move:2 NextState:Q3 " +
                   "CurState:Q3 CurSym:( SymToReplace:( Move:2 NextState:Q3 " +
                   "CurState:Q3 CurSym:A SymToReplace:A Move:2 NextState:Q3 " +
                   "CurState:Q3 CurSym:) SymToReplace:A Move:2 NextState:Q3 " +
                   "CurState:Q3 CurSym:e SymToReplace:e Move:1 NextState:Q1 " +
                   "CurState:Q1 CurSym:e SymToReplace:e Move:0 NextState:Q0 " +
                   "CurState:Q1 CurSym:) SymToReplace:) Move:0 NextState:Q-1 "}
        };

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
            string helper = string.Empty;
            if (currSymbol == ')' || currSymbol == '(')
            {
                helper = $@"\{currSymbol}";
            }
            else
            {
                helper = $"{currSymbol}";
            }


            Regex reg = new Regex($@"CurState:Q{currState} CurSym:{helper} SymToReplace:(\S*) Move:(\S*) NextState:Q(\S*)");
            MatchCollection matches = reg.Matches(TableOfConditions[TaskNumber]);
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
                Console.WriteLine($"CurrSym:{currString[currSymPos]}, symbol that would appear: {iteration.Symbol}, curr state:{currState}, next state: {iteration.StateNumber}, step: {iteration.Step}");
                currString[currSymPos] = iteration.Symbol;
                currState = iteration.StateNumber;
                currSymPos = GetCurrentSymbolPos(iteration.Step, currSymPos);
                Console.WriteLine();
            }

            if (currState == 0) Console.WriteLine("True");
            else Console.WriteLine("False");

            Console.WriteLine(currString);
        }

        private static bool IsWordValid(string word)
        {
            foreach (char c in word)
            {
                if (!Alphabets[TaskNumber].Contains(c.ToString()))
                {
                    Console.WriteLine("Word contains symbols not listed in alphabet. Please, enter correct word");
                    return false;
                }
            }

            return true;
        }

        private static void GreetUserAndGetTaskWorkingOn()
        {
            int i = 0;
            while (!TasksAvailable.Contains((i)))
            {
                Console.WriteLine($"Available tasks: {String.Join(",", TasksAvailable)}");
                Console.WriteLine("Enter the task number you want to be solved: ");
                var str = Console.ReadLine();
                Int32.TryParse(str,out i);
            }

            TaskNumber = i;
        }

        static void Main(string[] args)
        {
            GreetUserAndGetTaskWorkingOn();
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
