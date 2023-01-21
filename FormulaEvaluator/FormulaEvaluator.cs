
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

    public delegate int Lookup(String variable_name);

    public static int Evaluate(String expression,
                               Lookup variableEvaluator)
    {
        Stack<int> values = new Stack<int>();
        Stack<char> operators = new Stack<char>();

        string[] substrings =
            Regex.Split(expression, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

        foreach(string token in substrings)
        {
            int value;
            bool isValue = int.TryParse(token, out value);

            // check if token is an integer
            if(isValue)
            {
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
                        tempResult = tempVal * Int32.Parse(token);
                    } else
                    {
                        tempResult = tempVal / Int32.Parse(token);

                    }
                    // push the result onto the value stack
                    values.Push(tempResult);
                }


                // otherwise, push token onto the value stack
                else
                {
                    values.Push(Int32.Parse(token));
                }
            }


            // check if token is a variable
            else if(Regex.IsMatch(token, "[a-zA-Z]"))
            {
                // get lookup value of token

                // check the operator stack
                // if * or / is at the top
                // pop the value stack
                // pop the operator stack
                // apply the popped operator to the popped number and lookup value of token
                // push the result onto the value stack

                // otherwise, push the lookup value of token onto the value stack
            }


            // check if token is an operator

            // if token is + or -
            else if (token == "+" || token == "-")
            {
                // if + or - at the top of the operator stack
                // pop the value stack twice
                // apply the popped operator to the popped numbers
                // push the result onto the value stack

                // push token onto the operator stack

            }

            // check if token is * or /
            else if (token == "*" || token == "/")
            {
                // push token onto the operator stack
            }

            // check if token is (
            else if (token == "(")
            {
                // push token onto the operator stack
            }

            // check if token is )
            else if(token == ")")
            {
                // if + or - is at the top of the operator stack
                // pop the value stack twice and the operator stack once
                // apply the popped operator to the popped numbers
                // push the result onto the value stack

                // the top of the operator stack must be (. pop it.

                // if * or / at the top of the operator stack
                // pop the value stack twice and the operator stack once
                // apply the popped operator to the popped numbers
                // push the result onto the value stack
            }
        }

        return -1;
    }

    private bool isValue(string s)
    {
        int value;
        return int.TryParse(s, out value);
    }

}

