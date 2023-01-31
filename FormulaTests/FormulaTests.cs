using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;

namespace FormulaTests;

[TestClass]
public class UnitTest1
{

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
    public void TestSimpleGetTokens()
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
    /// Test if an empty string can be passed in GetTokens
    /// </summary>
    [TestMethod]
    public void TestGetTokensEmpty()
    {
        string s = "";
        List<string> tokens = GetTokens(s).ToList();
        Assert.AreEqual(0, tokens.Count());
    }


    private bool IsValidSyntax(string s)
    {
        String lpPattern = @"\(";
        String rpPattern = @"\)";
        String opPattern = @"[\+\-*/]";
        String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
        String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
        String spacePattern = @"\s+";

        String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                        lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);
        if (Regex.IsMatch(s, pattern))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    [TestMethod]
    public void TestSimpleValidSyntax()
    {
        string s = "1 +2/3 - 1.5";
        Assert.IsTrue(IsValidSyntax(s));
    }

    [TestMethod]
    public void TestComplexVaildSyntax()
    {
        string s = "x1 + 234/(298a) * 6.25+(linked_list)";
        Assert.IsTrue(IsValidSyntax(s));
    }

    [TestMethod]
    public void TestSimpleInvaildSyntax()
    {
        string s = "x 23 + 1";
        Assert.IsFalse(IsValidSyntax(s));
    }

    [TestMethod]
    public void TestComplexInvaildSyntax()
    {
        string s = "x123 linked_list.5 * 3.0 -(linked_list)";
        Assert.IsFalse(IsValidSyntax(s));
    }

    [TestMethod]
    public void TestComplexInvaildSyntax2()
    {
        string s = "x123 linked_list + 1";
        Assert.IsFalse(IsValidSyntax(s));
    }



}
