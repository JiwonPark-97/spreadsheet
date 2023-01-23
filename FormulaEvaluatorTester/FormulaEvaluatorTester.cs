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
/// The Main method calls various testing methods to test (helper) methods in Evaluator.
/// </summary>

using System.Linq.Expressions;
using System.Text.RegularExpressions;
using FormulaEvaluator;

/// <summary>
/// This class contains Main method and various testing methods that are called in Main.
/// A SimpleLookUp method is provided to pass in to the Evaluate method via the 2nd parameter.
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

        // Split Test //
        Console.WriteLine("********** Test Split **********");
        Console.WriteLine("\n");

        SplitTest("5+5");
        SplitTest("1 + 2 + 3 / 4 * 5");
        SplitTest("x1/34a*10+0");


        // RemoveWhiteSpace Test //
        Console.WriteLine("********** Test RemoveWhiteSpace **********");
        Console.WriteLine("\n");

        RemoveWhitespaceTest("5 5");
        RemoveWhitespaceTest("1 + 2 + 3 / 4 * 5");
        RemoveWhitespaceTest("x1/ 34 a*10+0 ");


        // StrToInt Test //
        Console.WriteLine("********** Test StrToInt **********");
        Console.WriteLine("\n");

        StrToIntTest("5", 5);
        StrToIntTest("0", 0);
        StrToIntTest("10", 10);
        StrToIntTest("-1", -1);
        StrToIntTest("-10", -10);



        // IsVariable Test //
        Console.WriteLine("********** Test IsVariable **********");
        Console.WriteLine("\n");

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
        Console.WriteLine("********** Test IsValue **********");
        Console.WriteLine("\n");

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
        Console.WriteLine("********** Test Evaluate **********");
        Console.WriteLine("\n");

        // test vaild expressions
        // simple expressions
        EvaluateTest("1", 1);
        EvaluateTest("(1)", 1);

        EvaluateTest("5+5", 10);
        EvaluateTest("5-5", 0);
        EvaluateTest("5*5", 25);
        EvaluateTest("5/5", 1);

        // simple expressions with parentheses
        EvaluateTest("(5+5)", 10);
        EvaluateTest("(5-5)", 0);
        EvaluateTest("(5*5)", 25);
        EvaluateTest("(5/5)", 1);

        // simple expressions that result in negative values
        EvaluateTest("0-10", -10);
        EvaluateTest("(1) - 2", -1);
        EvaluateTest("5-10", -5);

        // complex expressions
        EvaluateTest("(5 +5)/3 +2 ", 5);
        EvaluateTest("(5-5) * 10 - 3", -3);
        EvaluateTest("(5*5) * (6 -2)", 100);
        EvaluateTest("(5/5) / 1 + 2", 3);
        EvaluateTest("(3/5 * 20) / (4) * (2 + 1)", 0);
        EvaluateTest("(2+ 3) / 7 + 2", 2);

        // expressions with variables
        EvaluateWithVariableTest("X1", 10);
        EvaluateWithVariableTest("X1 + X2", 30);
        EvaluateWithVariableTest("X1/ X3", 0);
        EvaluateWithVariableTest("((X1) +X2) / X3", 1);


        // test invaild expressions (simply check if ArgumentException is thrown)
        //EvaluateTest("(5+5", -1);
        //EvaluateTest("1 1", -1);
        //EvaluateTest(") 1 + 2", -1);
        //EvaluateTest("12 3", -1);
        //EvaluateTest("abcc1a + 3", -1);
        //EvaluateTest("sldkj234a", -1);
        //EvaluateWithVariableTest("X3/ (X1 -X1)", -1);
        //EvaluateWithVariableTest("(X1 + X2)/X4", -1);

    }


    /// <summary>
    /// Returns the following variables
    ///     X1 = 10
    ///     X2 = 20
    ///     X3 = 30
    ///     Everything else is bad
    /// </summary>
    /// <param name="variable_name"> the name of the variable to look up.
    /// shoule be consisting of one or more letters followed by one or more digits </param>
    /// <returns> a corresponding value to the variable name </returns>
    /// <exception cref="ArgumentException"> thrown when a bad variable name is passed in </exception>
    public static int SimpleLookup(string variable_name)
    {
        if (variable_name.Equals("X1"))
        {
            return 10;
        }
        else if (variable_name.Equals("X2"))
        {
            return 20;
        } else if (variable_name.Equals("X3"))
        {
            return 30;
        } else
        {
            throw new ArgumentException();
        }
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
    /// Tests removing leading and trailing whitespaces from substrings.
    /// </summary>
    /// <param name="s"> an original string to be splitted and whitespaces gets removed from </param>
    public static void RemoveWhitespaceTest(string s)
    {
        string[] substrings =
        Regex.Split(s, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

        for (int i = 0; i < substrings.Length; i++)
        {
            string token = substrings[i];
            substrings[i] = Evaluator.RemoveWhiteSpace(token);
        }

        Console.WriteLine("Testing removing whitespace. Input: " + s + ". \nResult: ");
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
    /// <param name="expression"> a string of expression to be evaluated </param>
    /// <param name="expected"> expected integer value after evaluation </param>
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

    /// <summary>
    /// Test Evaluate method with variables. 
    /// </summary>
    /// <param name="expression"> a string of expression to be evaluated </param>
    /// <param name="expected"> expected integer value after evaluation</param>
    public static void EvaluateWithVariableTest(string expression, int expected)
    {
        int result = Evaluator.Evaluate(expression, SimpleLookup);
        Console.WriteLine("Testing Evaluate with variables. Input: " + expression + ". Expected: " + expected + ". Result: " + result);

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