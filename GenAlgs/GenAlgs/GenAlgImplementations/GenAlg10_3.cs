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
        double p1 = 0.005, p2 = 0.35, numberToSimplifyMutationProbabilityCalcs = 1000;

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
                    population[i].memberSet[j] = GetRandomNumber();
                }
            }
            fitnessCoef = GetFitness(population);
            bestPopulation = population;
            bestFitnessCoef = fitnessCoef;
        }

        private double SubstituteInEquation(int u,int w,int x,int y,int z)
        {
            return Math.Abs(Math.Pow(u, powU) * Math.Pow(w, powW) * Math.Pow(x, powX) * Math.Pow(y, powY) * Math.Pow(z, powZ) +
            Math.Pow(u, powU1) * Math.Pow(w, powW1) * Math.Pow(x, powX1) * Math.Pow(y, powY1) * Math.Pow(z, powZ1) +
            Math.Pow(u, powU2) * Math.Pow(w, powW2) * Math.Pow(x, powX2) * Math.Pow(y, powY2) * Math.Pow(z, powZ2) +
            Math.Pow(u, powU3) * Math.Pow(w, powW3) * Math.Pow(x, powX3) * Math.Pow(y, powY3) * Math.Pow(z, powZ3) +
            Math.Pow(u, powU4) * Math.Pow(w, powW4) * Math.Pow(x, powX4) * Math.Pow(y, powY4) * Math.Pow(z, powZ4) - Result);
        }

        public double GetFitness(List<PopulationMember> population)
        {
            double sum = 0;
            double fitnessCoefficient = 0;
            foreach(var parent in population)
            {
                parent.fitnessCoef = SubstituteInEquation(parent.memberSet[0], parent.memberSet[1],
                    parent.memberSet[2], parent.memberSet[3], parent.memberSet[4]);
                if(parent.fitnessCoef == 0)
                {
                    Console.WriteLine($"Solution found!!!\n{parent.memberSet[0]}, {parent.memberSet[1]}," +
                    $"{ parent.memberSet[2]}, {parent.memberSet[3]}, {parent.memberSet[4]}");
                    bestFitnessCoef =0;
                    break;
                }
                parent.fitnessCoef = 1 / parent.fitnessCoef;
                sum += parent.fitnessCoef;
            }
            foreach (var parent in population)
            {
                parent.fitnessCoef = parent.fitnessCoef / sum;
                fitnessCoefficient += parent.fitnessCoef;
            }
            return (fitnessCoefficient /= parentsInPopulationAmo);
        }

        //consider sending the lowest val as first parameter due to random functioning?
        private int GetRandomWithProbabilitySelected(double val1, double val2)
        {
            double val;
            Random random = new Random();
            val = random.Next(0, 1000);
            if (val / 1000 < val1 / (val1 + val2))
            {
                return 0;
            }
            return 1;
        }

        private int[] GetRandomSelectedPairs()
        {
            List<int> parentsNumbers=new List<int>();
            for(int i = 0; i < parentsInPopulationAmo; i++)
            {
                parentsNumbers[i] = i;
            }
            Random random = new Random();
            //check if borders are included in random!!!floor - largest not bigger than!
            int val = (int)Math.Floor(random.Next(0, (parentsNumbers.Count) * 1000)*1.0/1000);
            parentsNumbers.Remove(val);
            random = new Random();
            int val1 = (int)Math.Floor(random.Next(0, (parentsNumbers.Count) * 1000) * 1.0 / 1000);
            return new int[]{val, val1};
        }

        private List<int[]> RandomSelection()
        {
            List<int[]> selectedPairs = new List<int[]>();
            //Apply of random selection - select parents to be paired
            for(int i = 0; i < parentsInPopulationAmo; i++)
            {
                selectedPairs[i] = GetRandomSelectedPairs();
            }
            return selectedPairs;
        }

        private List<PopulationMember> FuckParents()
        {
            List<PopulationMember> childs = new List<PopulationMember>();
            List<int[]> selectedPairs = RandomSelection();
            for(int i = 0; i < selectedPairs.Count; i++)
            {
                var val1 = population[selectedPairs[i][0]].fitnessCoef;
                var val2 = population[selectedPairs[i][1]].fitnessCoef;
                for (int j = 0; j < variablesAmo; j++)
                {
                    childs[i].memberSet[j] = population[selectedPairs[i][GetRandomWithProbabilitySelected(val1, val2)]].memberSet[j];
                }
            }
            childFitnessCoef = GetFitness(childs);
            return childs;
        }

        private void ApplyMutationWithSpecifiedProbability(double probability, PopulationMember childs)
        {
            for(int i=0;i<childs.memberSet.Count;i++)
            {
                if (GetRandomWithProbabilitySelected(probability, 1 - probability) == 0)
                {
                    childs.memberSet[i] = GetRandomNumber();
                }
            }
        }

        public override void Mutate(List<PopulationMember> childs)
        {
            foreach(var child in childs)
            {
                if (child.fitnessCoef <= childFitnessCoef) {
                    ApplyMutationWithSpecifiedProbability(p2, child);
                }
                else
                {
                    ApplyMutationWithSpecifiedProbability(p1, child);
                }
            }
        }

        private double GetFitnessCoeffsSum(List<double> fitnessCoeffs)
        {
            double sum = 0;
            foreach(var el in fitnessCoeffs)
            {
                sum += el;
            }
            return sum;
        }

        private void GetRandomWithProbabilitySelectionGlobal(List<PopulationMember> parents, List<PopulationMember> children)
        {
            List<int> parentsNumbers = new List<int>();
            List<double> fitnessCoefs = new List<double>();
            List<PopulationMember> newPopulation = new List<PopulationMember>();
            for (int i = 0; i < parentsInPopulationAmo * 2; i++)
            {
                parentsNumbers[i] = i;
                if (i < parentsInPopulationAmo) {
                    fitnessCoefs.Add(parents[i].fitnessCoef);
                } else { fitnessCoefs.Add(children[i].fitnessCoef); }
            }

            for(int k = 0; k < parentsInPopulationAmo; k++)
            {
                //create a separate method for this
                Random random = new Random();
                int val = random.Next(0, (parentsNumbers.Count) * (int)numberToSimplifyMutationProbabilityCalcs);
                double sum = 0;
                double coef = 1000 / GetFitnessCoeffsSum(fitnessCoefs);
                int position = 0;
                for (int i = 0; val <= sum; i++)
                {
                    sum += fitnessCoefs[i] * coef;
                    position = i - 1;
                }
                if (position < parentsInPopulationAmo)
                {
                    newPopulation.Add(parents[position]);
                }
                else
                {
                    newPopulation.Add(children[position]);
                }
                parentsNumbers.Remove(position);
                fitnessCoefs.Remove(position);
            }
            var newFitnessCoeff = GetFitness(newPopulation);
        }

        public void Replace()
        {
            //GetRandomWithProbabilitySelectionGlobal
        }

        //this method would be called in a while(condition) as a condition.
        public void CheckIfConditionsAreValid()
        {
            //Max iterations - just call the stuff specified in a cycle with limited amount of iterations;not good soulution
            //if(fitnessCoef==0) => success!
            // if for several iterations we are not improving our fitness coeff => then stop.
        }
    }
}