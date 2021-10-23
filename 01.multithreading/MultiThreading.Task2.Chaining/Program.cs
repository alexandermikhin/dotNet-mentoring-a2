/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    class Program
    {
        const int Capacity = 10;
        const int MinNumber = 1;
        const int MaxNumber = 100;
        const int MinMultiplier = 2;
        const int MaxMultiplier = 3;

        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            // feel free to add your code
            ExecuteTask();

            Console.ReadLine();
        }

        static void ExecuteTask()
        {
            var task1 = Task.Run(GenerateNumbers);
            var outputTask = task1.ContinueWith((task) => Output("Initial array:", task.Result));
            outputTask.Wait();
            var task2 = task1.ContinueWith((task, state) => MultiplyNumbersRandom((int[])state), task1.Result);
            outputTask = task2.ContinueWith((task) => Output("Multiplied array:", (int[])task.AsyncState));
            outputTask.Wait();
            var task3 = task2.ContinueWith((task, state) => SortNumbers((int[])state), task2.AsyncState);
            outputTask = task3.ContinueWith((task) => Output("Sorted array:", (int[])task.AsyncState));
            outputTask.Wait();
            var task4 = task3.ContinueWith(task => GetAverage((int[])task.AsyncState));
            outputTask = task4.ContinueWith((task) => Output("Average is:", task.Result));
            outputTask.Wait();
        }

        static int[] GenerateNumbers()
        {
            var numbers = new int[Capacity];
            var random = new Random();

            for (int i = 0; i < Capacity; i++)
            {
                numbers[i] = random.Next(MinNumber, MaxNumber);
            }

            return numbers;
        }

        static void MultiplyNumbersRandom(int[] numbers)
        {
            var random = new Random();
            var multiplier = random.Next(MinMultiplier, MaxMultiplier);
            for (int i = 0; i < numbers.Length; i++)
            {
                numbers[i] *= multiplier;
            }
        }

        static void SortNumbers(int[] numbers)
        {
            Array.Sort(numbers);
        }

        static double GetAverage(int[] numbers)
        {
            return numbers.Average();
        }

        static void Output<T>(string title, params T[] numbers)
        {
            Console.WriteLine(title);
            Console.WriteLine(string.Join(' ', numbers));
        }
    }
}
