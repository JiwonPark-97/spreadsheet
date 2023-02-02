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

    // GetTokens Tests //


    /// <summary>
    /// A helper method in Formula class
    /// </summary>
    /// <param name="formula"> a formula </param>
    /// <returns> IEnumerable of composed tokens </returns>
    private static IEnumerable<string> GetTokens(String formula)
    {
        // Patterns for individual tokens
        String lpPattern = @"\(";
        String rpPattern = @"\)";
        String opPattern = @"[\+\-*/]";
        String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
        String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
        String spacePattern = @"\s+";

        // Overall pattern
        String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                        lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

        // Enumerate matching tokens that don't consist solely of white space.
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


    // Constructor Tests //

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

    // Evaluate Tests //

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
        string s = "(10) /((1 +2)-3)";
        Formula f = new Formula(s);
        //Assert.AreEqual(new FormulaError(""), f.Evaluate(null));
        Assert.IsInstanceOfType(f.Evaluate(null), typeof(FormulaError));
    }

}
