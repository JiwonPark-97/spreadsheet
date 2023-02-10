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
        AbstractSpreadsheet sheet = new Spreadsheet();
        Assert.AreEqual("", sheet.GetCellContents("a1"));
    }

    /// <summary>
    /// Invalid name should throw InvalidNameException
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void GetCellContentsTest2()
    {
        AbstractSpreadsheet sheet = new Spreadsheet();
        Assert.AreEqual("", sheet.GetCellContents(""));
    }

    /// <summary>
    /// Invalid name should throw InvalidNameException
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void GetCellContentsTest3()
    {
        AbstractSpreadsheet sheet = new Spreadsheet();
        Assert.AreEqual("", sheet.GetCellContents(null));
    }

    /// <summary>
    /// Number cell
    /// </summary>
    [TestMethod]
    public void GetCellContentsTest4()
    {
        AbstractSpreadsheet sheet = new Spreadsheet();
        sheet.SetCellContents("a1", 10);
        Assert.AreEqual(10.0, sheet.GetCellContents("a1"));
    }

    /// <summary>
    /// String cell
    /// </summary>
    [TestMethod]
    public void GetCellContentsTest5()
    {
        AbstractSpreadsheet sheet = new Spreadsheet();
        sheet.SetCellContents("a1", "abc");
        Assert.AreEqual("abc", sheet.GetCellContents("a1"));
    }

    /// <summary>
    /// Formula cell
    /// </summary>
    [TestMethod]
    public void GetCellContentsTest6()
    {
        AbstractSpreadsheet sheet = new Spreadsheet();
        Formula f = new Formula("1+2");
        sheet.SetCellContents("a1", f);
        Assert.AreEqual(new Formula("1+2"), sheet.GetCellContents("a1"));
    }

    // **************** GetNamesOfAllNonemptyCells Tests **************** //

    public void GetNamesOfAllNonemptyCellsTest1()
    {
        AbstractSpreadsheet sheet = new Spreadsheet();
        sheet.SetCellContents("a1", 1);

        List<string> names = sheet.GetNamesOfAllNonemptyCells().ToList();
        names.Contains("a1");
    }

    public void GetNamesOfAllNonemptyCellsTest2()
    {
        AbstractSpreadsheet sheet = new Spreadsheet();
        Formula f = new Formula("1+2");
        sheet.SetCellContents("a1", 1);
        sheet.SetCellContents("b1", "a");
        sheet.SetCellContents("c1", f);

        List<string> names = sheet.GetNamesOfAllNonemptyCells().ToList();
        names.Contains("a1");
        names.Contains("b1");
        names.Contains("c1");
    }

    public void GetNamesOfAllNonemptyCellsTest3()
    {
        AbstractSpreadsheet sheet = new Spreadsheet();
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

    // string contents //

    // Formula contents //


}
