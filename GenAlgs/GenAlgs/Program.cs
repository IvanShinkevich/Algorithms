using System;
using GenAlgs.GenAlgImplementations;

namespace GenAlgs
{
    class Program
    {
        static void Main(string[] args)
        {
            GenAlg10_3 solver = new GenAlg10_3();
            solver.SolveDiophEquation();
            Console.ReadKey();
        }
    }
}
