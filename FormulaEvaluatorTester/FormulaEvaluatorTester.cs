/// <summary>
/// Author:    Jiwon Park
/// Partner:   None
/// Date:      22-Jan-2023
/// Course:    CS 3500, University of Utah, School of Computing
/// Copyright: CS 3500 and Jiwon Park - This work may not 
///            be copied for use in Academic Coursework.
///
/// I, Jiwon Park, certify that I wrote this code from scratch and
/// did not copy it in part or whole from another source.  All 
/// references used in the completion of the assignments are cited 
/// in my README file.
///
/// This file contains a single class that provides tests for Evaluator. 
///
/// There are testing methods called in Main. 
/// </summary>

using System.Linq.Expressions;
using System.Text.RegularExpressions;
using FormulaEvaluator;

/// <summary>
/// This class contains Main and testing methods for Evaluator.
/// </summary>
/// 
class FormulaEvaluatorTester
{
    /// <summary>
    /// This invokes testing methods.
    /// </summary>
    /// <param name="args"></param>
    static void Main(string[] args)
    {

        // test split
        SplitTest("5+5");
        SplitTest("1 + 2 + 3 / 4 * 5");
        SplitTest("x1/34a*10+0");

        // test StrToInt

        StrToIntTest("5", 5);
        StrToIntTest("0", 0);
        StrToIntTest("10", 10);


        // IsVariable Test // 
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
        // simple expression
        EvaluateTest("1", 1);

        EvaluateTest("5+5", 10);
        EvaluateTest("5-5", 0);
        EvaluateTest("5*5", 25);
        EvaluateTest("5/5", 1);

        EvaluateTest("(5+5)", 10);
        EvaluateTest("(5-5)", 0);
        EvaluateTest("(5*5)", 25);
        EvaluateTest("(5/5)", 1);

        // complex expression
        EvaluateTest("(5 +5)/3 +2 ", 5);
        EvaluateTest("(5-5) * 10 - 3", -3);
        EvaluateTest("(5*5) * (6 -2)", 100);
        EvaluateTest("(5/5) / 1 + 2", 3);




    }
    /// <summary>
    /// Tests how split works.
    /// </summary>
    /// <param name="s"> a string to be splitted </param>
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
    /// <summary>
    /// Tests converting string to int.
    /// </summary>
    /// <param name="num"> integer represented in string format </param>
    /// <param name="expected"> expected integer after conversion </param>
    public static void StrToIntTest(string num, int expected)
    {
        int result = Int32.Parse(num);
        Console.WriteLine("Testing StrToInt. Input: " + num + ". Expected: " + expected + ". Result: " + result);
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
    /// <summary>
    /// Tests IsValue method in Evaluator. A vaild value is a non-negative integer.
    /// </summary>
    /// <param name="s"> a string to be checked </param>
    /// <param name="expected"> expected bool value. true if vaild, false otherwise </param>
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

    /// <summary>
    /// Tests IsVariable method in Evaluator. A vaild variable is consisting of one or more letters followed by one or more digits.
    /// </summary>
    /// <param name="s"> a string to be checked </param>
    /// <param name="expected"> expected bool value. true if vaild, false otherwise </param>
    public static void IsVariableTest(string s, bool expected)
    {
        bool result = Evaluator.IsVariable(s);
        Console.WriteLine("Testing IsVariable. Input: " + s + ". Expected: " + expected + ". Result: " + result);
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

    /// <summary>
    /// Test Evaluate method in Evaluator. 
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="expected"></param>
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