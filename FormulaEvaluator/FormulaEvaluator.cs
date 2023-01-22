
using System;
using System.Text.RegularExpressions;
/// <summary>
/// Author:    Jiwon Park
/// Partner:   None
/// Date:      18-Jan-2023
/// Course:    CS 3500, University of Utah, School of Computing
/// Copyright: CS 3500 and Jiwon Park - This work may not 
///            be copied for use in Academic Coursework.
///
/// I, Jiwon Park, certify that I wrote this code from scratch and
/// did not copy it in part or whole from another source.  All 
/// references used in the completion of the assignments are cited 
/// in my README file.
///
/// File Contents
///
///    [... and of course you should describe the contents of the 
///    file in broad terms here ...]
/// </summary>

namespace FormulaEvaluator;
public static class Evaluator
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="variable_name"></param>
    /// <returns></returns>
    public delegate int Lookup(String variable_name);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static bool isValue(string s)
    {
        int value;
        return int.TryParse(s, out value);
    }

    /// <summary>
    /// Check if the given input string is a vaild variable; consisting of one or more letters followed by one or more digits.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static bool isVariable(string s)
    {
        return Regex.IsMatch(s, "^[a-zA-Z]+[0-9]+$");
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="variableEvaluator"></param>
    /// <returns></returns>
    public static int Evaluate(String expression,
                               Lookup variableEvaluator)
    {
        Stack<int> values = new Stack<int>();
        Stack<char> operators = new Stack<char>();

        string[] substrings =
            Regex.Split(expression, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

        foreach(string token in substrings)
        {

            // check if token is an integer or a variable
            if (isValue(token) || isVariable(token))
            {
                // get int values from token
                int tokenVal;

                if (isValue(token))
                {
                    tokenVal = Int32.Parse(token);
                } else
                {
                    tokenVal = variableEvaluator(token);
                }

                // check the operator stack
                // if * or / is at the top
                if ((operators.Peek()).Equals('*') || (operators.Peek()).Equals('/'))
                {
                    // pop the value stack
                    int tempVal = values.Pop();

                    // pop the operator stack
                    char tempOpr = operators.Pop();

                    // apply the popped operator to the popped number and token
                    int tempResult;
                    if (tempOpr == '*')
                    {
                        tempResult = tempVal * tokenVal;
                    } else
                    {
                        tempResult = tempVal / tokenVal;

                    }

                    // push the result onto the value stack
                    values.Push(tempResult);
                }


                // otherwise, push token onto the value stack
                else
                {
                    values.Push(tokenVal);
                }
            }


            // check if token is an operator

            // if token is + or -
            else if (token == "+" || token == "-")
            {
                // if + or - at the top of the operator stack
                if (operators.Peek() == '+' || operators.Peek() == '-')
                {
                    // pop the value stack twice and the operator stack once
                    int val1 = values.Pop();
                    int val2 = values.Pop();
                    char op = operators.Pop();
                    int result;

                    // apply the popped operator to the popped numbers
                    if (op == '+')
                    {
                        result = val1 + val2;
                    } else
                    {
                        result = val1 - val2;
                    }

                    // push the result onto the value stack
                    values.Push(result);

                    // push token onto the operator stack
                    operators.Push(token.ToCharArray()[0]);
                }


            }

            // check if token is * / or (
            else if (token == "*" || token == "/" || token == "(")
            {
                // push token onto the operator stack
                operators.Push(token.ToCharArray()[0]);
            }

            // check if token is )
            else if(token == ")")
            {

                // if + or - at the top of the operator stack
                if (operators.Peek() == '+' || operators.Peek() == '-')
                {
                    // pop the value stack twice and the operator stack once
                    int val1 = values.Pop();
                    int val2 = values.Pop();
                    char op = operators.Pop();
                    int result;

                    // apply the popped operator to the popped numbers
                    if (op == '+')
                    {
                        result = val1 + val2;
                    }
                    else
                    {
                        result = val1 - val2;
                    }

                    // push the result onto the value stack
                    values.Push(result);
                }

                // the top of the operator stack must be (. pop it.
                char tempOp = operators.Pop();
                if (tempOp != '(')
                {
                    Console.WriteLine("No left parenthesis");
                }

                // if * or / at the top of the operator stack
                if (operators.Peek() == '*' || operators.Peek() == '/')
                {
                    // pop the value stack twice and the operator stack once
                    int val1 = values.Pop();
                    int val2 = values.Pop();
                    char op = operators.Pop();
                    int result;

                    // apply the popped operator to the popped numbers
                    if (op == '*')
                    {
                        result = val1 * val2;
                    }
                    else
                    {
                        result = val1 / val2;
                    }

                    // push the result onto the value stack
                    values.Push(result);
                }
            }
        }


        return -1;
    }

}

