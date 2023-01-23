
using System;
using System.Text.RegularExpressions;
/// <summary>
/// Author:    Jiwon Park
/// Partner:   None
/// Date:      23-Jan-2023
/// Course:    CS 3500, University of Utah, School of Computing
/// Copyright: CS 3500 and Jiwon Park - This work may not 
///            be copied for use in Academic Coursework.
///
/// I, Jiwon Park, certify that I wrote this code from scratch and
/// did not copy it in part or whole from another source.  All 
/// references used in the completion of the assignments are cited 
/// in my README file.
///
/// This file contains a single class Evaluator that provides the delegate declaration of Lookup
/// and the Evaluate method along with the helper methods (RemoveWhiteSpace, IsValue, and IsVariable)
/// </summary>

namespace FormulaEvaluator;
public static class Evaluator
{
    /// <summary>
    /// Converts a variables to values.
    /// </summary>
    /// <param name="variable_name"> a variable name </param>
    /// <returns> a corresponding value to input variable name </returns>
    public delegate int Lookup(String variable_name);

    /// <summary>
    /// Removes leading and trailing whitespaces from the given input string. 
    /// </summary>
    /// <param name="s"> a string that whitespaces get removed from </param>
    /// <returns> a string with no leading and trailing whitespaces </returns>
    public static string RemoveWhiteSpace(string s)
    {
        return s.Trim();
    }

    /// <summary>
    /// Determines if the given string is a vaild value (non-negative integer)
    /// </summary>
    /// <param name="s"> a string to be checked </param>
    /// <returns> true if vaild, false otherwise </returns>
    public static bool IsValue(string s)
    {
        int value;
        return (int.TryParse(s, out value) && value >= 0);
    }

    /// <summary>
    /// Determines if the given input string is a vaild variable; consisting of one or more letters followed by one or more digits.
    /// </summary>
    /// <param name="s"> a string to be checked </param>
    /// <returns> true if vaild, false otherwise </returns>
    public static bool IsVariable(string s)
    {
        return Regex.IsMatch(s, "^[a-zA-Z]+[0-9]+$");
    }



    /// <summary>
    /// Evaluates valid mathematic expressions. 
    /// </summary>
    /// <param name="expression"> an expression to be evaluated </param>
    /// <param name="variableEvaluator"> a delegate for converting variables to values </param>
    /// <returns> an integer value after evaluation </returns>
    public static int Evaluate(String expression,
                               Lookup variableEvaluator)
    {
        Stack<int> values = new Stack<int>();
        Stack<char> operators = new Stack<char>();

        string[] substrings =
            Regex.Split(expression, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

        // remove leading and trailing whitespaces from each token
        for (int i = 0; i < substrings.Length; i++)
        {
            string token = substrings[i];
            substrings[i] = RemoveWhiteSpace(token);
        }

        // proceed tokens from left to right
        foreach (string token in substrings)
        {

            if (IsValue(token) || IsVariable(token))
            {
                // get integer values from the token
                int tokenVal = 0;

                if (IsValue(token))
                {
                    tokenVal = Int32.Parse(token);
                }
                else
                {
                    tokenVal = variableEvaluator(token);
                }

                // check the operator stack for '*' or '/' at the top
                if (operators.Count != 0)
                {
                    if ((operators.Peek()) == '*' || (operators.Peek() == '/'))
                    {
                        // pop the value and operator stack and apply with the token
                        int tempVal = values.Pop();
                        char tempOpr = operators.Pop();
                        int tempResult;

                        if (tempOpr == '*')
                        {
                            tempResult = tempVal * tokenVal;
                        }

                        // if operator is '/'
                        else
                        {
                            if (tokenVal != 0)
                            {
                                tempResult = tempVal / tokenVal;

                            // prevent division by 0
                            }
                            else
                            {
                                throw new ArgumentException();

                            }
                        }

                        // push the result onto the value stack
                        values.Push(tempResult);
                    }

                    // if there isn't '*' or '/' at the top of the operator stack
                    else
                    {
                        values.Push(tokenVal);
                    }
                }

                // if operator stack is empty
                else
                {
                    values.Push(tokenVal);
                }
            }

            else if (token == "+" || token == "-")
            {

                // if + or - at the top of the operator stack
                if (operators.Count() != 0)
                {
                    if (operators.Peek() == '+' || operators.Peek() == '-')
                    {
                        // pop the value stack twice and the operator stack once
                        int val1 = values.Pop();
                        int val2 = values.Pop();
                        char op = operators.Pop();
                        int result;

                        // apply the popped operator to the popped numbers and push the result onto the value stack
                        if (op == '+')
                        {
                            result = val2 + val1;
                        }
                        else
                        {
                            result = val2 - val1;
                        }

                        values.Push(result);
                    }
                }

                // push token onto the operator stack
                operators.Push(token.ToCharArray()[0]);
            }

            else if (token == "*" || token == "/" || token == "(")
            {
                // push token onto the operator stack
                operators.Push(token.ToCharArray()[0]);
            }

            else if (token == ")")
            {
                // there should be '(' on the operator stack. if not, throw an exception
                if (!operators.Contains('('))
                {
                    throw new ArgumentException();
                }

                // if + or - at the top of the operator stack
                else if (operators.Peek() == '+' || operators.Peek() == '-')
                {
                    // pop the value stack twice and the operator stack once
                    int val1 = values.Pop();
                    int val2 = values.Pop();
                    char op = operators.Pop();
                    int result;

                    // apply the popped operator to the popped numbers and push the result onto the value stack
                    if (op == '+')
                    {
                        result = val2 + val1;
                    }

                    // op == '-'
                    else
                    {
                        result = val2 - val1;
                    }
                    values.Push(result);
                }


                // the top of the operator stack must be '('. pop it
                // throw an exception if there isn't '(' on the operator stack
                if (!operators.Contains('('))
                {
                    throw new ArgumentException();
                }
                else
                {
                    char tempOp = operators.Pop();

                }

                // if * or / at the top of the operator stack
                if (operators.Count() != 0)
                {
                    if (operators.Peek() == '*' || operators.Peek() == '/')
                    {
                        // pop the value stack twice and the operator stack once
                        int val1 = values.Pop();
                        int val2 = values.Pop();
                        char op = operators.Pop();
                        int result;

                        // apply the popped operator to the popped numbers and push the result onto the value stack
                        if (op == '*')
                        {
                            result = val2 * val1;
                        }

                        // op == '/'
                        else
                        {
                            if (val1 != 0)
                            {
                                result = val2 / val1;

                            }

                            // prevent division by 0
                            else
                            {
                                throw new ArgumentException();
                            }
                        }
                        values.Push(result);
                    }
                }

            }
        }
        // last token proceeded

        // if operator stack is empty, there should be a single value
        if (operators.Count() == 0)
        {
            if (values.Count() == 1)
            {
                return values.Pop();
            }

            // if there isn't exactly one value on the value stack
            else
            {
                throw new ArgumentException();
            }
        }

        // if operator stack is not empty, there should be exactly one operator (+ or -) and two values.
        else
        {
            if (operators.Count() == 1 && values.Count() == 2)
            {
                char op = operators.Pop();
                int val1 = values.Pop();
                int val2 = values.Pop();

                if (op == '+')
                {
                    return val2 + val1;

                }
                // should be '-'
                else
                {
                    return val2 - val1;
                }

            }
            // if there isn't exactly one operator and two values
            else
            {
                throw new ArgumentException();
            }

        }
        throw new ArgumentException();
    }

}

