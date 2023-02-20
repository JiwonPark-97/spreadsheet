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
/// This file contains two test classes SpreadsheetTests and ProtectedMethodTests. 
/// SpreadsheetTests contains unit tests for Spreadsheet class and 
/// ProtectedMethodTests class inherits from Spreadsheet class to test protected methods. 
/// </summary>

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using SS;

namespace SpreadsheetTests;

/// <summary>
/// This class provides unit tests for Spreadsheet
/// </summary>
[TestClass]
public class SpreadsheetTests
{
    // **************** GetCellContents Tests **************** //

    /// <summary>
    /// Empty cell should return an empty string
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
        sheet.GetCellContents("");
    }

    /// <summary>
    /// Invalid name should throw InvalidNameException
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void GetCellContentsTest3()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.GetCellContents("_");
    }

    /// <summary>
    /// Number cell
    /// </summary>
    [TestMethod]
    public void GetCellContentsTest4()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetContentsOfCell("a1", "10");
        Assert.AreEqual(10.0, sheet.GetCellContents("a1"));
    }

    /// <summary>
    /// String cell
    /// </summary>
    [TestMethod]
    public void GetCellContentsTest5()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetContentsOfCell("a1", "abc");
        Assert.AreEqual("abc", sheet.GetCellContents("a1"));
    }

    /// <summary>
    /// Formula cell
    /// </summary>
    [TestMethod]
    public void GetCellContentsTest6()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetContentsOfCell("a1", "=1+2");
        Assert.AreEqual(new Formula("1+2"), sheet.GetCellContents("a1"));
    }


    // **************** GetNamesOfAllNonemptyCells Tests **************** //

    /// <summary>
    /// Simple GetNamesOfAllNonemptyCells
    /// </summary>
    [TestMethod]
    public void GetNamesOfAllNonemptyCellsTest1()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetContentsOfCell("a1", "1");

        List<string> names = sheet.GetNamesOfAllNonemptyCells().ToList();
        Assert.IsTrue(names.Contains("a1"));
        Assert.AreEqual(1, names.Count());
    }

    /// <summary>
    /// Simple GetNamesOfAllNonemptyCells
    /// </summary>
    [TestMethod]
    public void GetNamesOfAllNonemptyCellsTest2()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetContentsOfCell("a1", "1");
        sheet.SetContentsOfCell("b1", "a");
        sheet.SetContentsOfCell("c1", "=1+2");

        List<string> names = sheet.GetNamesOfAllNonemptyCells().ToList();
        Assert.IsTrue(names.Contains("a1"));
        Assert.IsTrue(names.Contains("b1"));
        Assert.IsTrue(names.Contains("c1"));
    }

    /// <summary>
    /// Simple GetNamesOfAllNonemptyCells
    /// </summary>
    [TestMethod]
    public void GetNamesOfAllNonemptyCellsTest3()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetContentsOfCell("a1", "1");
        sheet.SetContentsOfCell("b1", "a");
        sheet.SetContentsOfCell("A1", "=1+2");

        List<string> names = sheet.GetNamesOfAllNonemptyCells().ToList();
        Assert.IsTrue(names.Contains("a1"));
        Assert.IsTrue(names.Contains("b1"));
        Assert.IsTrue(names.Contains("A1"));
    }

    // **************** SetCellContents Tests **************** //

    // number contents //

    /// <summary>
    /// Simple SetCellContents
    /// </summary>
    [TestMethod]
    public void SetCellContentsTest1()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetContentsOfCell("a1", "1");
        sheet.SetContentsOfCell("b1", "3.5");
        sheet.SetContentsOfCell("c1", "10");

        Assert.AreEqual(1.0, sheet.GetCellContents("a1"));
        Assert.AreEqual(3.5, sheet.GetCellContents("b1"));
        Assert.AreEqual(10.0, sheet.GetCellContents("c1"));
    }
    /// <summary>
    /// Simple SetCellContents
    /// </summary>
    [TestMethod]
    public void SetCellContentsTest2()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetContentsOfCell("A1", "10");
        sheet.SetContentsOfCell("A2", "10e-1");
        sheet.SetContentsOfCell("A3", "5.1234");

        Assert.AreEqual(10.0, sheet.GetCellContents("A1"));
        Assert.AreEqual(10e-1, sheet.GetCellContents("A2"));
        Assert.AreEqual(5.1234, sheet.GetCellContents("A3"));
    }

    /// <summary>
    /// Invalid name should throw InvalidNameException
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void SetCellContentsTest4()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetContentsOfCell("", "0");
    }

    /// <summary>
    /// Invalid name should throw InvalidNameException
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void SetCellContentsTest5()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetContentsOfCell("1a", "0");
    }


    // string contents //

    /// <summary>
    /// Simple SetCellContents
    /// </summary>
    [TestMethod]
    public void SetCellContentsTest6()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetContentsOfCell("A1", "a1");
        sheet.SetContentsOfCell("A2", "a2");
        sheet.SetContentsOfCell("A3", "a3");

        Assert.AreEqual("a1", sheet.GetCellContents("A1"));
        Assert.AreEqual("a2", sheet.GetCellContents("A2"));
        Assert.AreEqual("a3", sheet.GetCellContents("A3"));
    }

    /// <summary>
    /// Invalid name should throw InvalidNameException
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void SetCellContentsTest7()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetContentsOfCell("", "a1");
    }

    /// <summary>
    /// Invalid name should throw InvalidNameException
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void SetCellContentsTest8()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetContentsOfCell("1a", "a1");
    }

    /// <summary>
    /// Invalid formula should throw FormulaFormatException
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void SetCellContentsTest9()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetContentsOfCell("a1", "=(+)/1");
    }

    /// <summary>
    /// Empty text means empty cell. Should be removed from cells
    /// </summary>
    [TestMethod]
    public void SetCellContentsTest10()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetContentsOfCell("a1", "1");
        Assert.AreEqual(1, sheet.GetNamesOfAllNonemptyCells().Count());
        sheet.SetContentsOfCell("a1", "");
        Assert.AreEqual(0, sheet.GetNamesOfAllNonemptyCells().Count());
    }


    // Formula contents //

    ///// <summary>
    ///// Null formula parameter should throw ArgumentNullException
    ///// </summary>
    //[TestMethod]
    //[ExpectedException(typeof(ArgumentNullException))]
    //public void SetCellContentsTest11()
    //{
    //    Spreadsheet sheet = new Spreadsheet();
    //    sheet.SetContentsOfCell("a1", (Formula)null);
    //}

    /// <summary>
    /// Invalid name should throw InvalidNameException
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void SetCellContentsTest12()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetContentsOfCell("123a", "=1+2");
    }

    /// <summary>
    /// Invalid name should throw InvalidNameException
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void SetCellContentsTest13()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetContentsOfCell("1_", "=1+2");
    }

    /// <summary>
    /// Should throw CircularException
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(CircularException))]
    public void SetCellContentsTest14()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetContentsOfCell("a1", "=a3+3");
        sheet.SetContentsOfCell("a2", "=a1+1");
        sheet.SetContentsOfCell("a3", "=a2+2");
    }

    /// <summary>
    /// Should throw CircularException
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(CircularException))]
    public void SetCellContentsTest15()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetContentsOfCell("a1", "=b2+3");
        sheet.SetContentsOfCell("b2", "=a1+1");
        sheet.SetContentsOfCell("c3", "=a1*b2");
    }

    /// <summary>
    /// Should throw CircularException
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(CircularException))]
    public void SetCellContentsTest16()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetContentsOfCell("x1", "=x1-1");
    }

    /// <summary>
    /// Should throw CircularException
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(CircularException))]
    public void SetCellContentsTest17()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetContentsOfCell("x1", "=1");
        sheet.SetContentsOfCell("x2", "=x5+x1");
        sheet.SetContentsOfCell("x3", "=x1+3");
        sheet.SetContentsOfCell("x4", "=x2+2+x3");
        sheet.SetContentsOfCell("x5", "=x1+x2");
    }

    /// <summary>
    /// Shoud return a set consisting of name and names that depend on named cell directly or indirectly.
    /// </summary>
    [TestMethod]
    public void SetCellContentsTest18()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetContentsOfCell("x1", "=1");
        sheet.SetContentsOfCell("x2", "=2* x3");
        sheet.SetContentsOfCell("x3", "=x1/ 3");
        sheet.SetContentsOfCell("x4", "=x2 -2");

        List<string> names = sheet.SetContentsOfCell("x1", "3").ToList();

        Assert.AreEqual(4, names.Count());
        Assert.AreEqual("x1", names[0]);
        Assert.AreEqual("x3", names[1]);
        Assert.AreEqual("x2", names[2]);
        Assert.AreEqual("x4", names[3]);
    }

}

/// <summary>
/// This class contains unit tests for protected methods
/// </summary>
[TestClass]
public class ProtectedMethodTests : Spreadsheet
{
    // **************** GetCellsToRecalculate Tests **************** //

    /// <summary>
    /// Shoud throw InvalidNameException
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void GetCellsToRecalculateTest1()
    {
        Spreadsheet sheet = new Spreadsheet();
        GetCellsToRecalculate("");
    }

    /// <summary>
    /// Shoud throw InvalidNameException
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void GetCellsToRecalculateTest2()
    {
        GetCellsToRecalculate("1a");
    }
}
