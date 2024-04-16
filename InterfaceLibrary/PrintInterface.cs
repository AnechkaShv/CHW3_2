using System;
using System.Collections.Generic;
using FileWorkingLibrary;

namespace InterfaceLibrary
{
    public class PrintInterface
    {
        private Movie[] _data;
        private int _num;
        public PrintInterface() { }
        /// <summary>
        /// This constructor initializes data and number of choice how to print data.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="num"></param>
        public PrintInterface(Movie[] data, int num)
        {
            _data = data;
            _num = num;
        }
        /// <summary>
        /// This method prints data in the json format.
        /// </summary>
        public void PrintAsJson()
        {
            int N = _data.Length;

            // If we need to print first N elements or the whole data.
            if (_num == 1 || _num == 2)
            {
                Console.WriteLine($"Enter 1 <= N <= {_data.Length}");

                // Processing wrong numbers.
                while (!int.TryParse(Console.ReadLine(), out N) || N < 1 || N > _data.Length)
                    MainInterface.PrintColor("Wrong number.Please try again.", ConsoleColor.Red, ConsoleColor.DarkRed);
            }
            // Number of empty elements in each row.
            int counter = 0;
            for (int i = 0; i < N; i++)
            {
                if (_data[i] is null)
                {
                    counter += 1;
                }
            }
            // Printing values and keys only if the element isn't empty.
            if (counter != _data.Length)
            {
                // Printing first N elements or the whole data.
                if (_num == 1 || _num == 3)
                {
                    Console.Write("[\n");
                    for (int i = 0; i < N; i++)
                    {
                        if(i!=N-1)
                            Console.WriteLine($"{_data[i].ToJSON()},");
                        else
                            Console.WriteLine($"{_data[i].ToJSON()}");
                    }
                    Console.Write("]\n");
                }

                // Printing last N elements.
                else
                {
                    Console.Write("[\n");
                    for (int i = _data.Length - N; i < _data.Length; i++)
                    {
                        if (i != _data.Length - 1)
                            Console.WriteLine($"{_data[i].ToJSON()},");
                        else
                            Console.WriteLine($"{_data[i].ToJSON()}");
                    }
                    Console.Write("]\n");
                }
            }
        }
    }
}
