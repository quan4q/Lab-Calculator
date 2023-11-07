using System;
using System.Collections.Generic;

namespace SecondLab
{
    internal class Program
    {
        public static void Parse(List<double> numbers, List<char> operations, string input)
        {
            string number = "";

            for (int i = 0; i < input.Length; i++)
            {
                if (!char.IsDigit(input[i]))
                {
                    numbers.Add(double.Parse(number));
                    operations.Add(input[i]);
                    number = "";
                }
                else
                {
                    number += input[i];
                }
            }

            numbers.Add(double.Parse(number));
        }
        public static double GetNumber(double number1, double number2, char operation)
        {
            switch (operation)
            {
                case '+': return number1 + number2;
                case '-': return number1 - number2;
                case '*': return number1 * number2;
                case '/': return number1 / number2;
                default: return 0;
            }
        }
        public static void Calculate(List<double> numbers, List<char> operations)
        {
            while (operations.Count > 0)
            {
                if (operations.Contains('*') || operations.Contains('/'))
                {
                    int multiplicationIndex = operations.IndexOf('*');
                    int divisionIndex = operations.IndexOf('/');
                    int currentOperationIndex;

                    if (multiplicationIndex == -1) currentOperationIndex = divisionIndex;
                    else if (divisionIndex == -1) currentOperationIndex = multiplicationIndex;
                    else currentOperationIndex = Math.Min(divisionIndex, multiplicationIndex);

                    double firstNumber = numbers[currentOperationIndex];
                    double secondNumber = numbers[currentOperationIndex + 1];
                    char operation = operations[currentOperationIndex];

                    double newNumber = GetNumber(firstNumber, secondNumber, operation);

                    numbers.RemoveAt(currentOperationIndex + 1);
                    numbers.RemoveAt(currentOperationIndex);
                    operations.RemoveAt(currentOperationIndex);

                    numbers.Insert(currentOperationIndex, newNumber);
                }
                else if (operations.Contains('+') || operations.Contains('-'))
                {
                    double newNumber = GetNumber(numbers[0], numbers[1], operations[0]);

                    numbers.RemoveAt(1);
                    numbers.RemoveAt(0);
                    operations.RemoveAt(0);

                    numbers.Insert(0, newNumber);
                }
            }
        }
        static void Main(string[] args)
        { 
            string input = Console.ReadLine().Replace(" ", "");

            List<double> numbers = new List<double>();
            List<char> operations = new List<char>();

            Parse(numbers, operations, input);
            Calculate(numbers, operations);

            Console.WriteLine(string.Join(" ", numbers));
            Console.ReadLine();
        }
    }
}