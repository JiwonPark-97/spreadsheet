
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
        Stack<string> operators = new Stack<string>();

        string[] substrings =
            Regex.Split(expression, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

        foreach(string token in substrings)
        {
            if(token == )
        }

        return -1;
    }

}

