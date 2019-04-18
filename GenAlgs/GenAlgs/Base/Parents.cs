using System;
using System.Collections.Generic;
using System.Text;

namespace GenAlgs.Base
{
    public class PopulationMember
    {
        public List<int> memberSet;
        public double fitnessCoef;

        public PopulationMember()
        {
            memberSet = new List<int>();
            fitnessCoef = -1;
        }
        public void Show()
        {
            foreach (var member in memberSet)
            {
                Console.Write($"{member} ");
            }
            Console.WriteLine($"Fitness coefficient: {fitnessCoef}");
        }
    }
}
