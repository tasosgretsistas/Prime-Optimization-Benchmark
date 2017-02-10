using System;
using System.Collections.Specialized;
using System.Diagnostics;

namespace BenchmarkPrimeStrategy
{
    class Program
    {
        static void Main(string[] args)
        {
            int loops = 1000;

            Console.WriteLine(new BenchmarkStandard(loops).Output());

            Console.WriteLine(new BenchmarkOptimized(loops).Output());

            Console.ReadKey();
        }

        abstract class Benchmark
        {
            protected string Name { get; set; }

            protected Stopwatch Time { get; set; }

            protected int Loops { get; set; }

            public Benchmark(int loops)
            {
                Time = new Stopwatch();
                Loops = loops;
            }

            public abstract bool IsPrime(int number);

            protected bool IsEven(int number)
            {
                return !((number & 1) == 1);
            }

            public TimeSpan Bench()
            {
                Time.Start();

                for (int i = 1; i <= Loops; i++)
                {
                    IsPrime(i);
                }

                Time.Stop();

                return Time.Elapsed;
            }

            public string Test()
            {
                string output = "";

                for (int i = 1; i <= Loops; i++)
                {
                    output += i + " = ";

                    if (IsPrime(i))
                        output += "Prime\n";
                    else
                        output += "Not Prime\n";
                }

                return output;
            }

            public string Output()
            {
                return Name + " time: " + Bench();
            }
        }

        class BenchmarkStandard : Benchmark
        {
            public BenchmarkStandard(int loops) : base(loops) { Name = "Standard"; }

            public override bool IsPrime(int number)
            {
                bool result = true;

                if (number == 1) return false;
                if (number == 2) return true;

                for (int i = 3; i < number; i++)
                {
                    if (number % i == 0)
                    {
                        result = false;
                        break;
                    }
                }

                return result;
            }
        }

        class BenchmarkOptimized : Benchmark
        {
            public BenchmarkOptimized(int loops) : base(loops) { Name = "Optimized"; }

            public override bool IsPrime(int number)
            {
                bool result = true;

                if (number == 1) return false;
                if (number == 2) return true;

                if (IsEven(number)) return false;

                for (int i = 3; i < number; i += 2)
                {
                    if (number % i == 0)
                    {
                        result = false;
                        break;
                    }
                }

                return result;
            }
        }
    }
}