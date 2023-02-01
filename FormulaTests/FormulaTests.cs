using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;

namespace FormulaTests;

[TestClass]
public class UnitTest1
{
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


}
