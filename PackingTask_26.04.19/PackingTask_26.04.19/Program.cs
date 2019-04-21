using System;

namespace PackingTask
{
    public class Program
    {
        private static double EnterFraction()
        {
            Console.WriteLine("Enter the divident number:");
            double val0 = EnterNumber();
            if(val0 == -1)
            {
                return -1;
            }
            Console.WriteLine("Enter the divisor number:");
            double val1 = EnterNumber();
            if (val1 == -1 || val1 == 0)
            {
                return -1;
            }
                Console.WriteLine($"You entered: {val0 / val1}");
                return val0 / val1;
        }

        private static double EnterSquareRoot()
        {
            double val0 ;
            Console.WriteLine("Enter the number you want to get root of:");
            val0 = EnterNumber();
            if (val0 > 0)
            {
                Console.WriteLine($"Number you want to add: {Math.Sqrt(val0)}");
                return Math.Sqrt(val0);
            }
            Console.WriteLine("Invalid input");
            return -1;
        }

        private static double EnterNumber(bool flag = false)
        {
            double val0;
            if(flag)
            Console.WriteLine("Enter the number you want to add:");
            if (!double.TryParse(Console.ReadLine(), out val0))
            {
                Console.WriteLine("Incorrect input, try again");
                return -1;
            };
            return val0;
        }

        public static void Main(string[] args)
        {
            double[] items = new double[]
            {
                0.1, 0.3, 6*1.0/7, 8*1.0/109, 0.5, 0.7, 0.456, 1*1.0/3, 0.0001
            };
            Pack pack = new Pack();
            foreach(var el in items)
            {
                Console.WriteLine($"Iteration {Array.IndexOf(items, el)}");
                pack.AddItem(el);
                pack.ShowPacksLoad();
            }
            Console.ReadKey();

            //Create a menu from this code
            double result;
            bool flag = true;
            while (flag)
            {
                Console.WriteLine("Please, choose an option(0,1 or 2) to enter fraction, square root or decimal.If you want to leave - enter 3.");
                string optionLine = Console.ReadLine();
                int option;
                Int32.TryParse(optionLine,out option);
                result = -1;
                switch (option)
                {
                    case 0:
                        result = EnterFraction();
                        break;
                    case 1:
                        result = EnterSquareRoot();
                        break;
                    case 2:
                        result = EnterNumber(true);
                        break;
                    case 3:
                        flag = false;
                        break;
                    default:
                        Console.WriteLine("Value should be equal to 0,1,2,3. Try again");
                        break;
                }
                if (flag)
                {
                    if (result != -1)
                    {
                        pack.AddItem(result);
                        pack.ShowPacksLoad();
                    }
                }

            }
            Console.WriteLine("Press enter to close this window");
            Console.ReadKey();
        }
    }
}
