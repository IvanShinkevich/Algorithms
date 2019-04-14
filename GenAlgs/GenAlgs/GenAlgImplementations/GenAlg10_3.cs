using GenAlgs.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GenAlgs.GenAlgImplementations
{
    public class GenAlg10_3 : GenAlgStructureInterface
    {
        //Equation coeffs
        private int powU, powW, powX, powY, powZ, powU1, powW1, powX1, powY1, powZ1, powU2, powW2, powX2, powY2, powZ2,
            powU3, powW3, powX3, powY3, powZ3, powU4, powW4, powX4, powY4, powZ4, Result, u, w, x, y, z,
            variablesAmo = 5, parentsInPopulationAmo = 5;

        public void SetEquationCoeffs()
        {
            powU = 0;
            powW = 0;
            powX = 0;
            powY = 1;
            powZ = 0;
            powU1 = 1;
            powW1 = 1;
            powX1 = 1;
            powY1 = 0;
            powZ1 = 0;
            powU2 = 2;
            powW2 = 2;
            powX2 = 2;
            powY2 = 0;
            powZ2 = 1;
            powU3 = 2;
            powW3 = 2;
            powX3 = 1;
            powY3 = 1;
            powZ3 = 0;
            powU4 = 2;
            powW4 = 1;
            powX4 = 0;
            powY4 = 0;
            powZ4 = 2;
            Result = 22;
            min = -300;
            max = 300;
        }

        private int GetRandomNumber()
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        public override void CreateFirstPopulation()
        {
            for(int i=0; i< parentsInPopulationAmo; i++)
            {
                for (int j = 0; j < variablesAmo; j++)
                {
                    population[i].parentsSet[j] = GetRandomNumber();
                }
            }
        }

        private double SubstituteInEquation(int u,int w,int x,int y,int z)
        {
            return Math.Abs(Math.Pow(u, powU) * Math.Pow(w, powW) * Math.Pow(x, powX) * Math.Pow(y, powY) * Math.Pow(z, powZ) +
            Math.Pow(u, powU1) * Math.Pow(w, powW1) * Math.Pow(x, powX1) * Math.Pow(y, powY1) * Math.Pow(z, powZ1) +
            Math.Pow(u, powU2) * Math.Pow(w, powW2) * Math.Pow(x, powX2) * Math.Pow(y, powY2) * Math.Pow(z, powZ2) +
            Math.Pow(u, powU3) * Math.Pow(w, powW3) * Math.Pow(x, powX3) * Math.Pow(y, powY3) * Math.Pow(z, powZ3) +
            Math.Pow(u, powU4) * Math.Pow(w, powW4) * Math.Pow(x, powX4) * Math.Pow(y, powY4) * Math.Pow(z, powZ4) - Result);
        }

        public void GetFitness()
        {
            double sum = 0;
            foreach(var parent in population)
            {
                parent.fitnessCoef = SubstituteInEquation(parent.parentsSet[0], parent.parentsSet[1],
                    parent.parentsSet[2], parent.parentsSet[3], parent.parentsSet[4]);
                if(parent.fitnessCoef == 0)
                {
                    Console.WriteLine($"Solution found!!!\n{parent.parentsSet[0]}, {parent.parentsSet[1]}," +
                    $"{ parent.parentsSet[2]}, {parent.parentsSet[3]}, {parent.parentsSet[4]}");
                    break;
                }
                parent.fitnessCoef = 1 / parent.fitnessCoef;
                sum += parent.fitnessCoef;
            }
            foreach (var parent in population)
            {
                parent.fitnessCoef = parent.fitnessCoef / sum;
                fitnessCoef += parent.fitnessCoef;
            }
            fitnessCoef /= parentsInPopulationAmo;
        }
    }
}
