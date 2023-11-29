using System;
using System.Collections.Generic;

namespace SecondLab
{
    class Token
    {

    }

    class Number: Token
    {
        public double number;
    }

    class Operator: Token
    {
        public char operation;
    }

    class Parenthesis: Token
    {
        public bool bracket;
    }
    internal class Program
    {
        static List<Token> Parse(string expression)
        {
            List<Token> result = new List<Token>();
            string number = "";

            foreach(char symbol in expression)
            {
                if (symbol != ' ')
                {
                    if (!char.IsDigit(symbol))
                    {
                        if (number != "")
                        {
                            Number num = new Number();
                            num.number = Convert.ToDouble(number);
                            result.Add(num);
                        }
                        if (symbol.Equals('-') || symbol.Equals('+') || symbol.Equals('*') || symbol.Equals('/'))
                        {
                            Operator op = new Operator();
                            op.operation = symbol;
                            result.Add(op);
                        }
                        else if (symbol.Equals('('))
                        {
                            Parenthesis par = new Parenthesis();
                            par.bracket = true;
                            result.Add(par);
                        }
                        else
                        {
                            Parenthesis par = new Parenthesis();
                            par.bracket = false;
                            result.Add(par);
                        }
                        number = "";
                    }
                    else
                    {
                        number += symbol;
                    }
                }
            }
            if (number != "")
            {
                Number num = new Number();
                num.number = Convert.ToDouble(number);
                result.Add(num);
            }

            return result;
        }

        static int CheckPriority(Token operation)
        {
            if (operation is Operator)
            {
                switch (((Operator)operation).operation)
                {
                    case '+': case '-': return 1;
                    case '*': case '/': return 2;
                    default: return 0;
                }
            }
            else return 0;
        }

        static List<Token> ConvertToRPN(List<Token> expression)
        {
            Stack<Token> operators = new Stack<Token>();
            List<Token> result = new List<Token>();

            foreach(Token symbol in expression)
            {
                if (symbol is Number)
                {
                    result.Add((Number)symbol);
                }
                else if (symbol is Operator)
                {
                    while (operators.Count > 0 && CheckPriority(operators.Peek()) >= CheckPriority(symbol))
                    {
                        result.Add(operators.Pop());
                    }
                    operators.Push((Operator)symbol);
                }
                else if (symbol is Parenthesis)
                {
                    if (((Parenthesis)symbol).bracket)
                    {
                        operators.Push((Parenthesis)symbol);
                    }

                    else
                    {
                        while (operators.Count > 0 && !(operators.Peek() is Parenthesis))
                        {
                            result.Add(operators.Pop());
                        }
                        operators.Pop();
                    }
                }
            }

            while(operators.Count > 0)
            {
                result.Add(operators.Pop());
            }

            return result;
        }

        static double Calculate(List<Token> ExpInRPN)
        {
            Stack<double> numbers = new();

            foreach (Token token in ExpInRPN)
            {
                if (token is Number number)
                {
                    numbers.Push(number.number);
                }
                else if (token is Operator operation)
                {
                    double second = numbers.Pop();
                    double first = numbers.Pop();
                    Number firstNum = new();
                    firstNum.number = first;
                    Number secondNum = new();
                    secondNum.number = second;

                    double resultedNum = (GetNumber((Number)firstNum, (Number)secondNum, (Operator)token)).number;
                    numbers.Push(resultedNum);
                }
            }

            return numbers.Pop();
        }
        static Number GetNumber(Number number1, Number number2, Operator operation)
        {
            Number result = new();
            if(operation.operation == '+')
            {
                result.number = number1.number + number2.number;
            }
            if (operation.operation == '-')
            {
                result.number = number1.number - number2.number;
            }
            if (operation.operation == '*')
            {
                result.number = number1.number * number2.number;
            }
            if (operation.operation == '/')
            {
                result.number = number1.number / number2.number;
            }
            return result;
        }

        static void Print(List<Token> ListToPrint)
        {
            foreach(Token e in ListToPrint)
            {
                if(e is Number)
                {
                    Number num = new();
                    num = (Number)e;
                    Console.Write(num.number);
                    Console.Write(" ");
                }
                else if(e is Operator)
                {
                    Operator op = new();
                    op = (Operator)e;
                    Console.Write(op.operation);
                    Console.Write(" ");
                }
                else
                {
                    Parenthesis bracket = new();
                    bracket = (Parenthesis)e;
                    if (bracket.bracket) Console.Write("( ");
                    else Console.Write(") ");
                }
            }
            Console.Write("\n");
        }
        
        static void Main(string[] args)
        {
            string expression = Console.ReadLine();

            List<Token> Parsed = Parse(expression);
            List<Token> ExpInRPN = ConvertToRPN(Parsed);

            Console.Write("Parsed expression: ");
            Print(Parsed);

            Console.Write("Expression in RPN: ");
            Print(ExpInRPN);

            double CalculatedExpression = Calculate(ExpInRPN);
            Console.Write("Calculated expression: ");
            Console.WriteLine(CalculatedExpression);
        }
    }
}