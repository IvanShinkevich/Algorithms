using System;
using System.Collections.Generic;
using System.Text;

namespace GenAlgs.Base
{
    public abstract class GenAlgStructureInterface
    {
        //Current population
        public List<Parents> population;
        //Best found ever population
        public List<int> bestPopulation;
        //Property used to continue running cycle
        public bool areConditionsValid = true;
        //Field used for counting iterations amount so that we could stop if it specified as CONDITION OF BREAK
        private long iterationsAmount;
        //Fitness coefficient of previous population(so that it could be decided what steps to be done)
        public double fitnessCoef;
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

        public void Mutate()
        {
        }

        public void CheckConditions()
        {
        }
    }
}