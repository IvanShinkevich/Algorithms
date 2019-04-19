using System;
using System.Collections.Generic;
using System.Text;

namespace GenAlgs.Base
{
    public abstract class GenAlgStructureInterface
    {
        //Current population
        public List<PopulationMember> population;
        //Best found ever population
        public List<PopulationMember> bestPopulation;
        //Property used to continue running cycle
        public bool areConditionsValid = true;
        //Field used for counting iterations amount so that we could stop if it specified as CONDITION OF BREAK
        public long iterationsAmount;
        public long maxIterationsAmount;
        public long amountOfPopulationsLessSucessfullThenTheBestOne;
        public long maxAmountOfPopulationsLessSucessfullThenTheBestOne = 10;

        public PopulationMember solution;
        public double bestFitnessCoef;
        //Fitness coefficient of previous population(so that it could be decided what steps to be done)
        public double fitnessCoef;
        //Fitness coefficient of child population(so that it could be decided what steps to be done)
        public double childFitnessCoef;
        //Ends of interval where the numbers should be chosen from
        public int min, max;

        public abstract void CreateFirstPopulation();

        public void GetFitness(List<List<int>> population)
        {
        }

        public void ApplySelection()
        {
        }

        public void CreateNewPopulation()
        {
        }

        public abstract void Mutate(List<PopulationMember> childs);

        public void CheckConditions()
        {
        }
    }
}