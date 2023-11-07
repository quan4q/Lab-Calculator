using System;
using System.Collections.Generic;

namespace SecondLab
{
    internal class Program
    {
        static List<object> Parse(string expression)
        {
            List<object> result = new List<object>();
            string number = "";

            foreach(char symbol in expression)
            {
                if (symbol != ' ')
                {
                    if (!char.IsDigit(symbol))
                    {
                        if (number != "") result.Add(number);
                        result.Add(symbol);
                        number = "";
                    }
                    else
                    {
                        number += symbol;
                    }
                }
            }
            if (number != "") result.Add(number);

            return result;
        }

        static int CheckPriority(object operation)
        {
            switch (operation)
            {
                case '+': case '-': return 1;
                case '*': case '/': return 2;
                default: return 0;
            }
        }

        static List<object> ConvertToRPN(List<object> expression)
        {
            Stack<object> operators = new Stack<object>();
            List<object> result = new List<object>();

            foreach(object symbol in expression)
            {
                if(symbol is string)
                {
                    result.Add(symbol);
                }
                else if(symbol.Equals('-') || symbol.Equals('+') || symbol.Equals('*') || symbol.Equals('/'))
                {
                    while(operators.Count > 0 && CheckPriority(operators.Peek()) >= CheckPriority(symbol))
                    {
                        result.Add(operators.Pop());
                    }
                    operators.Push(symbol);
                }
                else if (symbol.Equals('('))
                {
                    operators.Push(symbol);
                }
                else if (symbol.Equals(')'))
                {
                    while(operators.Count > 0 && !operators.Peek().Equals('('))
                    {
                        result.Add(operators.Pop());
                    }
                    operators.Pop();
                }
            }

            while(operators.Count > 0)
            {
                result.Add(operators.Pop());
            }

            return result;
        }

        static float Calculate(List<object> ExpInRPN)
        {
            for(int i = 0; i < ExpInRPN.Count; i++)
            {
                if (ExpInRPN[i] is char)
                {
                    float firstNumber = Convert.ToSingle(ExpInRPN[i - 2]);
                    float secondNumber = Convert.ToSingle(ExpInRPN[i - 1]);

                    float result = GetNumber(firstNumber, secondNumber, ExpInRPN[i]);

                    ExpInRPN.RemoveRange(i - 2, 3);
                    ExpInRPN.Insert(i - 2, result);
                    i -= 2;
                }
            }
            float CalculatedExpression = Convert.ToSingle(ExpInRPN[0]);

            return CalculatedExpression;
        }
        public static float GetNumber(float number1, float number2, object operation)
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
        
        static void Main(string[] args)
        {
            string expression = Console.ReadLine();

            List<object> Parsed = Parse(expression);
            List<object> ExpInRPN = ConvertToRPN(Parsed);

            Console.WriteLine(string.Join(" ", ExpInRPN));

            float CalculatedExpression = Calculate(ExpInRPN);
            Console.WriteLine(CalculatedExpression);
        }
    }
}