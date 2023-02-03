/// <summary>
/// Author:    Jiwon Park
/// Partner:   None
/// Date:      3-Feb-2023
/// Course:    CS 3500, University of Utah, School of Computing
/// Copyright: CS 3500 and Jiwon Park - This work may not 
///            be copied for use in Academic Coursework.
///
/// I, Jiwon Park, certify that I wrote this code from scratch and
/// did not copy it in part or whole from another source.  All 
/// references used in the completion of the assignments are cited 
/// in my README file.
///
/// This file contains a single class that provides unit tests for Formula. 
///
/// </summary>

using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;

namespace FormulaTests;

[TestClass]
public class FormulaTests
{

    // **************** GetTokens Tests **************** //

    /// <summary>
    /// A helper method in Formula class
    /// </summary>
    /// <param name="formula"> a formula expression </param>
    /// <returns> IEnumerable of composed tokens </returns>
    private static IEnumerable<string> GetTokens(String formula)
    {
        String lpPattern = @"\(";
        String rpPattern = @"\)";
        String opPattern = @"[\+\-*/]";
        String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
        String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
        String spacePattern = @"\s+";

        String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                        lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

        foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
        {
            if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
            {
                yield return s;
            }
        }
    }

    /// <summary>
    /// Test how GetTokens works
    /// </summary>
    [TestMethod]
    public void TestGetTokens1()
    {
        string s = "abc / 123* (x1+x2)";
        List<string> tokens = GetTokens(s).ToList();
        Assert.AreEqual(9, tokens.Count());
        Assert.AreEqual("abc", tokens[0]);
        Assert.AreEqual("/", tokens[1]);
        Assert.AreEqual("123", tokens[2]);
        Assert.AreEqual("*", tokens[3]);
        Assert.AreEqual("(", tokens[4]);
        Assert.AreEqual("x1", tokens[5]);
        Assert.AreEqual("+", tokens[6]);
        Assert.AreEqual("x2", tokens[7]);
        Assert.AreEqual(")", tokens[8]);
    }

    /// <summary>
    /// Test how GetTokens works with decimal numbers
    /// </summary>
    [TestMethod]
    public void TestGetTokens2()
    {
        string s = "1 + 2.0-(3.5)";
        List<string> tokens = GetTokens(s).ToList();
        Assert.AreEqual(7, tokens.Count());
        Assert.AreEqual("1", tokens[0]);
        Assert.AreEqual("+", tokens[1]);
        Assert.AreEqual("2.0", tokens[2]);
        Assert.AreEqual("-", tokens[3]);
        Assert.AreEqual("(", tokens[4]);
        Assert.AreEqual("3.5", tokens[5]);
        Assert.AreEqual(")", tokens[6]);
    }

    /// <summary>
    /// Test how GetTokens works with number(s) followed by letter(s)
    /// It separates letter(s) from number(s)
    /// </summary>
    [TestMethod]
    public void TestGetTokens3()
    {
        string s = "2a + 33x/(_123) * 5_";
        List<string> tokens = GetTokens(s).ToList();
        Assert.AreEqual(12, tokens.Count());
        Assert.AreEqual("2", tokens[0]);
        Assert.AreEqual("a", tokens[1]);
        Assert.AreEqual("+", tokens[2]);
        Assert.AreEqual("33", tokens[3]);
        Assert.AreEqual("x", tokens[4]);
        Assert.AreEqual("/", tokens[5]);
        Assert.AreEqual("(", tokens[6]);
        Assert.AreEqual("_123", tokens[7]);
        Assert.AreEqual(")", tokens[8]);
        Assert.AreEqual("*", tokens[9]);
        Assert.AreEqual("5", tokens[10]);
        Assert.AreEqual("_", tokens[11]);
    }

    /// <summary>
    /// Test if an empty string can be passed in GetTokens
    /// </summary>
    [TestMethod]
    public void TestGetTokensEmpty()
    {
        string s = "";
        List<string> tokens = GetTokens(s).ToList();
        Assert.AreEqual(0, tokens.Count());
    }


    // **************** Constructor Tests **************** //

    // Test Valid Expressions //

    /// <summary>
    /// Test constructor with simple expression
    /// </summary>
    [TestMethod]
    public void TestSimpleConstructor1()
    {
        string s = "1+2+3";
        Formula f = new Formula(s);
    }

    /// <summary>
    /// Test constructor with simple expression
    /// </summary>
    [TestMethod]
    public void TestSimpleConstructor2()
    {
        string s = "(1+2)*3";
        Formula f = new Formula(s);
    }

    /// <summary>
    /// Test constructor with variables and decimal numbers
    /// </summary>
    [TestMethod]
    public void TestComplexConstructor1()
    {
        string s = "(1.0+2)*3.5/x1_ * (_123_) - 1.2";
        Formula f = new Formula(s);
    }

    /// <summary>
    /// Test constructor with variables and scientific notations
    /// </summary>
    [TestMethod]
    public void TestComplexConstructor2()
    {
        string s = "3.5 - e/0+(_variable1 *variable2) - e";
        Formula f = new Formula(s);
    }


    // Test Invalid Expression //

    /// <summary>
    /// Test Specific Token Rule
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestInvalidConstructor1()
    {
        string s = "123^ - %_12";
        Formula f = new Formula(s);
    }


    /// <summary>
    /// Test One Token Rule
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestInvalidConstructor2()
    {
        string s = " ";
        Formula f = new Formula(s);
    }

    /// <summary>
    /// Test Balanced Parentheses Rule
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestInvalidConstructor3()
    {
        string s = "(1+2*3";
        Formula f = new Formula(s);
    }

    /// <summary>
    /// Test Right Parentheses Rule
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestInvalidConstructor4()
    {
        string s = "(1)+2*3)";
        Formula f = new Formula(s);
    }

    /// <summary>
    /// Test Starting Token Rule
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestInvalidConstructor5()
    {
        string s = "+ 123 -10/x_1";
        Formula f = new Formula(s);
    }

    /// <summary>
    /// Test Ending Token Rule
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestInvalidConstructor6()
    {
        string s = "123 -10/x_1 - ";
        Formula f = new Formula(s);
    }

    /// <summary>
    /// Test Parenthesis/Operator Following Rule
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestInvalidConstructor7()
    {
        string s = "((abc -()))";
        Formula f = new Formula(s);
    }

    /// <summary>
    /// Test Extra Following Rule
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestInvalidConstructor8()
    {
        string s = "1.23 (abc_ + 5) 1";
        Formula f = new Formula(s);
    }


    // **************** Evaluate Tests **************** //

    // Test without variables //

    [TestMethod]
    public void TestSimpleEvaluate1()
    {
        string s = "1 + 2";
        Formula f = new Formula(s);
        Assert.AreEqual(3.0, f.Evaluate(null));
    }

    [TestMethod]
    public void TestSimpleEvaluate2()
    {
        string s = "1 - 2";
        Formula f = new Formula(s);
        Assert.AreEqual(-1.0, f.Evaluate(null));
    }

    [TestMethod]
    public void TestSimpleEvaluate3()
    {
        string s = "1 * 2";
        Formula f = new Formula(s);
        Assert.AreEqual(2.0, f.Evaluate(null));
    }

    [TestMethod]
    public void TestSimpleEvaluate4()
    {
        string s = "1 / 2";
        Formula f = new Formula(s);
        Assert.AreEqual(0.5, f.Evaluate(null));
    }

    [TestMethod]
    public void TestComplexEvaluate1()
    {
        string s = "((1+2) / 3) * 10";
        Formula f = new Formula(s);
        Assert.AreEqual(10.0, f.Evaluate(null));
    }

    [TestMethod]
    public void TestComplexEvaluate2()
    {
        string s = "(1) +3/3-(10-9 )";
        Formula f = new Formula(s);
        Assert.AreEqual(1.0, f.Evaluate(null));
    }

    [TestMethod]
    public void TestFormulaErrorEvaluate1()
    {
        string s = "1/0";
        Formula f = new Formula(s);
        Assert.IsInstanceOfType(f.Evaluate(null), typeof(FormulaError));
    }

    [TestMethod]
    public void TestFormulaErrorEvaluate2()
    {
        string s = "3 /(1 - (1 / 1))";
        Formula f = new Formula(s);
        Assert.IsInstanceOfType(f.Evaluate(null), typeof(FormulaError));
    }

    [TestMethod]
    public void TestFormulaErrorEvaluate3()
    {
        string s = "(10) /((1 +2)-3)";
        Formula f = new Formula(s);
        Assert.IsInstanceOfType(f.Evaluate(null), typeof(FormulaError));
    }


    // Test with variables, normalizer, and validator //

    /// <summary>
    /// A lookup for variables. Everything else other than the variable names defined below is bad.
    /// </summary>
    /// <param name="variable_name"> the name of the variable to look up.
    /// any letter or underscore followed by any number of letters and/or digits
    /// and/or underscores would form valid variable namess </param>
    /// <returns> a corresponding value to the variable name </returns>
    /// <exception cref="ArgumentException"> thrown when a bad variable name is passed in </exception>
    public static double Lookup(string variable_name)
    {
        if (variable_name.Equals("x1"))
        {
            return 10;
        }
        else if (variable_name.Equals("x2"))
        {
            return 20;
        }
        else if (variable_name.Equals("x3"))
        {
            return 30;
        }
        else if (variable_name.Equals("_variable_"))
        {
            return 1;
        }
        else if (variable_name.Equals("_x1"))
        {
            return 2;
        }
        else
        {
            throw new ArgumentException();
        }
    }

    /// <summary>
    /// A normalizer for variables. Returns a lowercased variable name
    /// </summary>
    /// <param name="variable_name"> a variable name to be normalized </param>
    public static string Normalizer(string variable_name)
    {
        return variable_name.ToLower();
    }

    /// <summary>
    /// Returns if a given variable name is valid.
    /// Any lowercase letter or underscore followed by any number of lowercase letters and/or digits and/or underscores would form valid variable names.
    /// </summary>
    /// <param name="variable_name"> a variable name to be determined </param>
    public static bool Validator(string variable_name)
    {
        string pattern = string.Format(@"[a-z_](?: [a-z_]|\d)*");
        return (Regex.IsMatch(variable_name, pattern));
    }

    /// <summary>
    /// Test for simple variables
    /// </summary>
    [TestMethod]
    public void TestVariableEvaluate1()
    {
        string s = "x1 + x2 * x3";
        Formula f = new Formula(s, Normalizer, Validator);
        Assert.AreEqual(610.0, f.Evaluate(Lookup));
    }

    /// <summary>
    /// Test for yet-normalized variables
    /// </summary>
    [TestMethod]
    public void TestVariableEvaluate2()
    {
        string s = "X1 + 2";
        Formula f = new Formula(s,Normalizer, Validator);
        Assert.AreEqual(12.0, f.Evaluate(Lookup));
    }

    /// <summary>
    /// Test for yet-normalized variables
    /// </summary>
    [TestMethod]
    public void TestVariableEvaluate3()
    {
        string s = "(X1 + X2)/(X3)";
        Formula f = new Formula(s, Normalizer, Validator);
        Assert.AreEqual(1.0, f.Evaluate(Lookup));
    }

    /// <summary>
    /// Test for yet-normalized variables
    /// </summary>
    [TestMethod]
    public void TestVariableEvaluate4()
    {
        string s = "(_X1 + X2)/(_Variable_)";
        Formula f = new Formula(s, Normalizer, Validator);
        Assert.AreEqual(22.0, f.Evaluate(Lookup));
    }

    /// <summary>
    /// Test for division by 0
    /// </summary>
    [TestMethod]
    public void TestVariableEvaluate5()
    {
        string s = "(x2)/(X1-x1)";
        Formula f = new Formula(s, Normalizer, Validator);
        Assert.IsInstanceOfType(f.Evaluate(Lookup), typeof(FormulaError));
    }

    /// <summary>
    /// Test for undefined variable
    /// </summary>
    [TestMethod]
    public void TestVariableEvaluate6()
    {
        string s = "(xx2)/(X1-_variable_)";
        Formula f = new Formula(s, Normalizer, Validator);
        Assert.IsInstanceOfType(f.Evaluate(Lookup), typeof(FormulaError));
    }


    // **************** GetVariables Tests **************** //

    /// <summary>
    /// Test GetVariables method
    /// </summary>
    [TestMethod]
    public void TestGetVariables1()
    {
        string s = "x1 + x2";
        Formula f = new Formula(s, Normalizer, Validator);
        List<string> variables = f.GetVariables().ToList();
        Assert.AreEqual(2, variables.Count());
        Assert.AreEqual("x1", variables[0]);
        Assert.AreEqual("x2", variables[1]);
    }

    /// <summary>
    /// Test GetVariables method with yet-normalized variables
    /// </summary>
    [TestMethod]
    public void TestGetVariables2()
    {
        string s = "(_X1 + X2)/(_Variable_)";
        Formula f = new Formula(s, Normalizer, Validator);
        List<string> variables = f.GetVariables().ToList();
        Assert.AreEqual(3, variables.Count());
        Assert.AreEqual("_x1", variables[0]);
        Assert.AreEqual("x2", variables[1]);
        Assert.AreEqual("_variable_", variables[2]);
    }

    // **************** ToString Tests **************** //

    /// <summary>
    /// Test ToString with whitespaces
    /// </summary>
    [TestMethod]
    public void TestToString1()
    {
        string s = "x1 + x2-3";
        Formula f = new Formula(s, Normalizer, Validator);
        Assert.AreEqual("x1+x2-3", f.ToString());
    }

    /// <summary>
    /// Test ToString with yet-normalized variables
    /// </summary>
    [TestMethod]
    public void TestToString2()
    {
        string s = "X1 + X2 +(3)";
        Formula f = new Formula(s, Normalizer, Validator);
        Assert.AreEqual("x1+x2+(3)", f.ToString());
    }

    /// <summary>
    /// Test ToString with same formulas
    /// </summary>
    [TestMethod]
    public void TestToString3()
    {
        string s = "x1+x2-x3/_variable_";
        Formula f = new Formula(s, Normalizer, Validator);

        string expected = "x1+x2-x3/_variable_";
        Assert.AreEqual(expected, f.ToString());
    }

    /// <summary>
    /// Test ToString with same formulas (after nomalized)
    /// </summary>
    [TestMethod]
    public void TestToString4()
    {
        string s = "X1+X2-x3/_VARIABLE_";
        Formula f = new Formula(s, Normalizer, Validator);

        string expected = "x1+x2-x3/_variable_";
        Assert.AreEqual(expected, f.ToString());
    }

    // **************** Equals Tests **************** //

    /// <summary>
    /// Test Equals with same string inputs
    /// </summary>
    [TestMethod]
    public void TestEquals1()
    {
        string s1 = "1 +x2";
        string s2 = "1 +x2";

        Formula f1 = new Formula(s1);
        Formula f2 = new Formula(s2);

        Assert.IsTrue(f1.Equals(f2));
    }

    /// <summary>
    /// Test Equals with formulas that have different numbers of tokens
    /// </summary>
    [TestMethod]
    public void TestEquals2()
    {
        string s1 = "x1 +x2";
        string s2 = "1+ 2+ x1";

        Formula f1 = new Formula(s1);
        Formula f2 = new Formula(s2);

        Assert.IsFalse(f1.Equals(f2));
    }

    /// <summary>
    /// Test Equals with null
    /// </summary>
    [TestMethod]
    public void TestEquals3()
    {
        string s = "x1 +x2";
        Formula f = new Formula(s);

        Assert.IsFalse(f.Equals(null));
    }

    /// <summary>
    /// Test Equals without normalizer
    /// </summary>
    [TestMethod]
    public void TestEquals4()
    {
        string s1 = "X1 + X2";
        string s2 = "x1+x2";

        Formula f1 = new Formula(s1);
        Formula f2 = new Formula(s2);

        Assert.IsFalse(f1.Equals(f2));
    }

    /// <summary>
    /// Test Equals with normalizer
    /// </summary>
    [TestMethod]
    public void TestEquals5()
    {
        string s1 = "X1 + X2";
        string s2 = "x1+x2";

        Formula f1 = new Formula(s1, Normalizer, Validator);
        Formula f2 = new Formula(s2, Normalizer, Validator);

        Assert.IsTrue(f1.Equals(f2));
    }


    // **************** == Tests **************** //

    /// <summary>
    /// Test == with values
    /// </summary>
    [TestMethod]
    public void TestEqualEqual1()
    {
        string s1 = "1 +2";
        string s2 = "1   +  2";

        Formula f1 = new Formula(s1, Normalizer, Validator);
        Formula f2 = new Formula(s2, Normalizer, Validator);

        Assert.IsTrue(f1 == f2);
    }

    /// <summary>
    /// Test == with variables
    /// </summary>
    [TestMethod]
    public void TestEqualEqual2()
    {
        string s1 = "X1 + X2";
        string s2 = "x1+x2";

        Formula f1 = new Formula(s1, Normalizer, Validator);
        Formula f2 = new Formula(s2, Normalizer, Validator);

        Assert.IsTrue(f1 == f2);
    }

    /// <summary>
    /// Test == with unequal formulas 
    /// </summary>
    [TestMethod]
    public void TestEqualEqual3()
    {
        string s1 = "_abc  +1";
        string s2 = "_ABC-1";

        Formula f1 = new Formula(s1, Normalizer, Validator);
        Formula f2 = new Formula(s2, Normalizer, Validator);

        Assert.IsFalse(f1 == f2);
    }

    // **************** != Tests **************** //

    /// <summary>
    /// Test != with values
    /// </summary>
    [TestMethod]
    public void TestNonEqual1()
    {
        string s1 = "1 +2";
        string s2 = "1   +  2";

        Formula f1 = new Formula(s1, Normalizer, Validator);
        Formula f2 = new Formula(s2, Normalizer, Validator);

        Assert.IsFalse(f1 != f2);
    }

    /// <summary>
    /// Test != with variables
    /// </summary>
    [TestMethod]
    public void TestNonEqual2()
    {
        string s1 = "X1 + X2";
        string s2 = "x1+x2";

        Formula f1 = new Formula(s1, Normalizer, Validator);
        Formula f2 = new Formula(s2, Normalizer, Validator);

        Assert.IsFalse(f1 != f2);
    }

    /// <summary>
    /// Test != with variables
    /// </summary>
    [TestMethod]
    public void TestNonEqual3()
    {
        string s1 = "_abc  +1";
        string s2 = "_ABC-1";

        Formula f1 = new Formula(s1, Normalizer, Validator);
        Formula f2 = new Formula(s2, Normalizer, Validator);

        Assert.IsTrue(f1 != f2);
    }

    // **************** GetHashCode Tests **************** //

    /// <summary>
    /// Equal formulas should return same hash code
    /// </summary>
    [TestMethod]
    public void TestGetHashCode1()
    {
        string s1 = "1+2";
        string s2 = "1+2";

        Formula f1 = new Formula(s1, Normalizer, Validator);
        Formula f2 = new Formula(s2, Normalizer, Validator);

        Assert.AreEqual(f1.GetHashCode(), f2.GetHashCode());
    }

    /// <summary>
    /// Equal formulas should return same hash code
    /// </summary>
    [TestMethod]
    public void TestGetHashCode2()
    {
        string s1 = "x1+ X2";
        string s2 = "X1 +  x2";

        Formula f1 = new Formula(s1, Normalizer, Validator);
        Formula f2 = new Formula(s2, Normalizer, Validator);

        Assert.AreEqual(f1.GetHashCode(), f2.GetHashCode());
    }

}
