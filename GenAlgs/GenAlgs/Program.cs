using System;
using GenAlgs.GenAlgImplementations;

namespace GenAlgs
{
    class Program
    {
        static void Main(string[] args)
        {
            GenAlg10_3 solver = new GenAlg10_3();
            var watch = System.Diagnostics.Stopwatch.StartNew();
            solver.SolveDiophEquation();
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine($"Time spent:{elapsedMs/1000}");
            Console.ReadKey();
        }
    }
}
