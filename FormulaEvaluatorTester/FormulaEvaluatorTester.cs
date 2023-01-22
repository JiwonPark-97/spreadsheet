/// <summary>
/// Author:    Jiwon Park
/// Partner:   None
/// Date:      21-Jan-2023
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

using System.Linq.Expressions;
using System.Text.RegularExpressions;
using FormulaEvaluator;

class FormulaEvaluatorTester
{
    static void Main(string[] args)
    {

        // test split
        SplitTest("5+5");
        SplitTest("1 + 2 + 3 / 4 * 5");
        SplitTest("x1/34a*10+0");


        // IsVariableT est // 
        // test vaild variables
        IsVariableTest("X1", true);
        IsVariableTest("XxX2", true);
        IsVariableTest("CD2", true);
        IsVariableTest("cd2345", true);
        IsVariableTest("alsdkflaskjdflasj2398475093287459032874957", true);


        // test invaild variables
        IsVariableTest("X", false);
        IsVariableTest("X2X2", false);
        IsVariableTest("CD2a", false);
        IsVariableTest("cd", false);
        IsVariableTest("cakldjflqjw'ofjlsakdj3lksjdlfkja", false);


        // IsValue Test // 
        // test valid integers
        IsValueTest("1", true);
        IsValueTest("0", true);
        IsValueTest("345", true);
        IsValueTest("230948", true);

        // test invalid integers
        IsValueTest("-1", false);
        IsValueTest(" ", false);
        IsValueTest("398a", false);
        IsValueTest("d394857", false);


        // Evaluate Test //
        // test vaild expressions

        EvaluateTest("1", 1);

        EvaluateTest("5+5", 10);
        //EvaluateTest("5-5", 0);
        //EvaluateTest("5*5", 25);
        //EvaluateTest("5/5", 1);



    }

    public static void SplitTest(string s)
    {
        string[] substrings =
        Regex.Split(s, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

        Console.WriteLine("Testing split. Input: " + s + ". \nResult: ");
        foreach (string t in substrings)
        {
            Console.WriteLine(t);
        }
        Console.WriteLine("\n");

    }

    public static void IsValueTest(string s, bool expected)
    {
        bool result = Evaluator.IsValue(s);
        Console.WriteLine("Testing IsValue. Input: " + s + ". Expected: " + expected + ". Result: " + result);
        if (expected == result)
        {
            Console.WriteLine("TEST PASSED");
        }
        else
        {
            Console.WriteLine("*TEST FAILED*");

        }
        Console.WriteLine("\n");
    }


    public static void IsVariableTest(string s, bool expected)
    {
        bool result = Evaluator.IsVariable(s);
        Console.WriteLine("Testing IsVariable. Input: " + s + ". Expected: " + expected + ". Result: " + result);
        if (expected == result)
        {
            Console.WriteLine("TEST PASSED");
        } else
        {
            Console.WriteLine("*TEST FAILED*");

        }
        Console.WriteLine("\n");

    }

    public static void EvaluateTest(string expression, int expected)
    {
        int result = Evaluator.Evaluate(expression, null);
        Console.WriteLine("Testing Evaluate. Input: " + expression + ". Expected: " + expected + ". Result: " + result);

        if (expected == result)
        {
            Console.WriteLine("TEST PASSED");
        }
        else
        {
            Console.WriteLine("*TEST FAILED*");

        }
        Console.WriteLine("\n");
    }
}