/// <summary>
/// Author:    Jiwon Park
/// Partner:   None
/// Date:      10-Feb-2023
/// Course:    CS 3500, University of Utah, School of Computing
/// Copyright: CS 3500 and Jiwon Park - This work may not 
///            be copied for use in Academic Coursework.
///
/// I, Jiwon Park, certify that I wrote this code from scratch and
/// did not copy it in part or whole from another source.  All 
/// references used in the completion of the assignments are cited 
/// in my README file.
///
/// File content
/// </summary>

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using SS;

namespace SpreadsheetTests;

[TestClass]
public class SpreadsheetTests
{

    // **************** GetCellContents Tests **************** //

    /// <summary>
    /// Named cell is empty
    /// </summary>
    [TestMethod]
    public void GetCellContentsTest1()
    {
        Spreadsheet sheet = new Spreadsheet();
        Assert.AreEqual("", sheet.GetCellContents("a1"));
    }

    /// <summary>
    /// Invalid name should throw InvalidNameException
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void GetCellContentsTest2()
    {
        Spreadsheet sheet = new Spreadsheet();
        Assert.AreEqual("", sheet.GetCellContents(""));
    }

    /// <summary>
    /// Invalid name should throw InvalidNameException
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void GetCellContentsTest3()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.GetCellContents(null);
    }

    /// <summary>
    /// Number cell
    /// </summary>
    [TestMethod]
    public void GetCellContentsTest4()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetCellContents("a1", 10);
        Assert.AreEqual(10.0, sheet.GetCellContents("a1"));
    }

    /// <summary>
    /// String cell
    /// </summary>
    [TestMethod]
    public void GetCellContentsTest5()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetCellContents("a1", "abc");
        Assert.AreEqual("abc", sheet.GetCellContents("a1"));
    }

    /// <summary>
    /// Formula cell
    /// </summary>
    [TestMethod]
    public void GetCellContentsTest6()
    {
        Spreadsheet sheet = new Spreadsheet();
        Formula f = new Formula("1+2");
        sheet.SetCellContents("a1", f);
        Assert.AreEqual(new Formula("1+2"), sheet.GetCellContents("a1"));
    }

    // **************** GetNamesOfAllNonemptyCells Tests **************** //

    [TestMethod]
    public void GetNamesOfAllNonemptyCellsTest1()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetCellContents("a1", 1);

        List<string> names = sheet.GetNamesOfAllNonemptyCells().ToList();
        names.Contains("a1");
    }

    [TestMethod]
    public void GetNamesOfAllNonemptyCellsTest2()
    {
        Spreadsheet sheet = new Spreadsheet();
        Formula f = new Formula("1+2");
        sheet.SetCellContents("a1", 1);
        sheet.SetCellContents("b1", "a");
        sheet.SetCellContents("c1", f);

        List<string> names = sheet.GetNamesOfAllNonemptyCells().ToList();
        names.Contains("a1");
        names.Contains("b1");
        names.Contains("c1");
    }

    [TestMethod]
    public void GetNamesOfAllNonemptyCellsTest3()
    {
        Spreadsheet sheet = new Spreadsheet();
        Formula f = new Formula("1+2");
        sheet.SetCellContents("a1", 1);
        sheet.SetCellContents("_", "a");
        sheet.SetCellContents("A", f);

        List<string> names = sheet.GetNamesOfAllNonemptyCells().ToList();
        names.Contains("a1");
        names.Contains("_");
        names.Contains("A");
    }

    // **************** SetCellContents Tests **************** //

    // number contents //

    [TestMethod]
    public void SetCellContentsTest1()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetCellContents("a", 1);
        sheet.SetCellContents("b_", 3.5);
        sheet.SetCellContents("_c", 10);

        Assert.AreEqual(1.0, sheet.GetCellContents("a"));
        Assert.AreEqual(3.5, sheet.GetCellContents("b_"));
        Assert.AreEqual(10.0, sheet.GetCellContents("_c"));
    }

    [TestMethod]
    public void SetCellContentsTest2()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetCellContents("A1", 10);
        sheet.SetCellContents("A2", 10e-1);
        sheet.SetCellContents("A3", 5.1234);

        Assert.AreEqual(10.0, sheet.GetCellContents("A1"));
        Assert.AreEqual(10e-1, sheet.GetCellContents("A2"));
        Assert.AreEqual(5.1234, sheet.GetCellContents("A3"));
    }

    /// <summary>
    /// Null name should throw InvalidNameException
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void SetCellContentsTest4()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetCellContents(null, 0);
    }

    /// <summary>
    /// Invalid name should throw InvalidNameException
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void SetCellContentsTest5()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetCellContents("1a", 0);
    }


    // string contents //

    [TestMethod]
    public void SetCellContentsTest6()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetCellContents("A1", "a1");
        sheet.SetCellContents("A2", "a2");
        sheet.SetCellContents("A3", "a3");

        Assert.AreEqual("a1", sheet.GetCellContents("A1"));
        Assert.AreEqual("a2", sheet.GetCellContents("A2"));
        Assert.AreEqual("a3", sheet.GetCellContents("A3"));
    }

    /// <summary>
    /// Null name should throw InvalidNameException
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void SetCellContentsTest7()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetCellContents(null, "a1");
    }

    /// <summary>
    /// Invalid name should throw InvalidNameException
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void SetCellContentsTest8()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetCellContents("1a", "a1");
    }

    /// <summary>
    /// Null text should throw ArgumentNullException
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void SetCellContentsTest9()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetCellContents("a1", (string)null);
    }

    /// <summary>
    /// Empty text means empty cell. Should remove from cells
    /// </summary>
    [TestMethod]
    public void SetCellContentsTest10()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetCellContents("_1", 1);
        Assert.AreEqual(1, sheet.GetNamesOfAllNonemptyCells().Count());
        sheet.SetCellContents("_1", "");
        Assert.AreEqual(0, sheet.GetNamesOfAllNonemptyCells().Count());
    }


    // Formula contents //

    /// <summary>
    /// Null formula parameter should throw ArgumentNullException
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void SetCellContentsTest11()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetCellContents("a1", (Formula)null);
    }

    /// <summary>
    /// Null name should throw InvalidNameException
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void SetCellContentsTest12()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetCellContents(null, new Formula("1+2"));
    }

    /// <summary>
    /// Invalid name should throw InvalidNameException
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void SetCellContentsTest13()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetCellContents("1_", new Formula("1+2"));
    }

    /// <summary>
    /// Should throw CircularException
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(CircularException))]
    public void SetCellContentsTest14()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetCellContents("a1", new Formula("a3+3"));
        sheet.SetCellContents("a2", new Formula("a1+1"));
        sheet.SetCellContents("a3", new Formula("a2+2"));
    }

    /// <summary>
    /// Should throw CircularException
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(CircularException))]
    public void SetCellContentsTest15()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetCellContents("_1", new Formula("_a+3"));
        sheet.SetCellContents("_a", new Formula("_1+1"));
        sheet.SetCellContents("_A", new Formula("_1*_a"));
    }

    /// <summary>
    /// Should throw CircularException
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(CircularException))]
    public void SetCellContentsTest16()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetCellContents("x1", new Formula("x1-1"));
    }

    /// <summary>
    /// Should throw CircularException
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(CircularException))]
    public void SetCellContentsTest17()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetCellContents("x1", new Formula("1"));
        sheet.SetCellContents("x2", new Formula("x5+x1"));
        sheet.SetCellContents("x3", new Formula("x1+3"));
        sheet.SetCellContents("x4", new Formula("x2+2+x3"));
        sheet.SetCellContents("x5", new Formula("x1+x2"));
    }

    /// <summary>
    /// Shoud return a set consisting of name and names that depend on named cell directly or indirectly.
    /// </summary>
    [TestMethod]
    public void SetCellContentsTest18()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetCellContents("x1", new Formula("1"));
        sheet.SetCellContents("x2", new Formula("2* x3"));
        sheet.SetCellContents("x3", new Formula("x1/ 3"));
        sheet.SetCellContents("x4", new Formula("x2 -2"));

        List<string> names = sheet.SetCellContents("x1", 3).ToList();

        Assert.AreEqual(4, names.Count());
        Assert.AreEqual("x1", names[0]);
        Assert.AreEqual("x3", names[1]);
        Assert.AreEqual("x2", names[2]);
        Assert.AreEqual("x4", names[3]);
    }

}
