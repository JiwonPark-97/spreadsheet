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

using System.Xml;
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

    // **************** Constructor Tests **************** //

    // zero-argument constructor

    /// <summary>
    /// simple zero-argument constructor
    /// </summary>
    [TestMethod]
    public void ConstructorTest1()
    {
        Spreadsheet sheet = new Spreadsheet();
    }

    // three-argument constructor

    /// <summary>
    /// simple three-argument constructor
    /// </summary>
    [TestMethod]
    public void ConstructorTest2()
    {
        Spreadsheet sheet = new Spreadsheet(s => true, s => s, "default");
    }

    // four-argument constructor

    /// <summary>
    /// simple four-argument constructor
    /// </summary>
    [TestMethod]
    public void ConstructorTest3()
    {
        using (XmlWriter writer = XmlWriter.Create("save.txt"))
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("spreadsheet");
            writer.WriteAttributeString("version", "1.0");

            writer.WriteStartElement("cell");
            writer.WriteElementString("name", "A1");
            writer.WriteElementString("contents", "hello");
            writer.WriteEndElement();

            writer.WriteEndElement();
            writer.WriteEndDocument();
        }

        Spreadsheet sheet = new Spreadsheet("save.txt", s => true, s => s, "1.0");

        Assert.AreEqual(1, sheet.GetNamesOfAllNonemptyCells().Count());
        Assert.AreEqual("hello", sheet.GetCellValue("A1"));
    }

    /// <summary>
    /// four-argument constructor with number, string, formula contents
    /// </summary>
    [TestMethod]
    public void ConstructorTest4()
    {
        using (XmlWriter writer = XmlWriter.Create("save2.txt"))
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("spreadsheet");
            writer.WriteAttributeString("version", "2.0");

            writer.WriteStartElement("cell");
            writer.WriteElementString("name", "a1");
            writer.WriteElementString("contents", "1");
            writer.WriteEndElement();

            writer.WriteStartElement("cell");
            writer.WriteElementString("name", "a2");
            writer.WriteElementString("contents", "two");
            writer.WriteEndElement();

            writer.WriteStartElement("cell");
            writer.WriteElementString("name", "a3");
            writer.WriteElementString("contents", "=a1+2");
            writer.WriteEndElement();

            writer.WriteEndElement();
            writer.WriteEndDocument();
        }

        Spreadsheet sheet = new Spreadsheet("save2.txt", s => true, s => s, "2.0");

        Assert.AreEqual(3, sheet.GetNamesOfAllNonemptyCells().Count());
        Assert.AreEqual(1.0, sheet.GetCellValue("a1"));
        Assert.AreEqual("two", sheet.GetCellValue("a2"));
        Assert.AreEqual(3.0, sheet.GetCellValue("a3"));
    }

    /// <summary>
    /// Should throw SpreadsheetReadWriteException if the version of the saved spreadsheet 
    /// does not match the version parameter provided to the constructor
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(SpreadsheetReadWriteException))]
    public void ConstructorTest5()
    {
        using (XmlWriter writer = XmlWriter.Create("save2.txt"))
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("spreadsheet");
            writer.WriteAttributeString("version", "3.0");

            writer.WriteStartElement("cell");
            writer.WriteElementString("name", "a1");
            writer.WriteElementString("contents", "1");
            writer.WriteEndElement();

            writer.WriteEndElement();
            writer.WriteEndDocument();
        }

        Spreadsheet sheet = new Spreadsheet("save2.txt", s => true, s => s, "2.0");
    }

    /// <summary>
    /// Should throw SpreadsheetReadWriteException if any of the names contained in the saved spreadsheet are invalid
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(SpreadsheetReadWriteException))]
    public void ConstructorTest6()
    {
        using (XmlWriter writer = XmlWriter.Create("save2.txt"))
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("spreadsheet");
            writer.WriteAttributeString("version", "4.0");

            writer.WriteStartElement("cell");
            writer.WriteElementString("name", "1a");
            writer.WriteElementString("contents", "1");
            writer.WriteEndElement();

            writer.WriteEndElement();
            writer.WriteEndDocument();
        }

        Spreadsheet sheet = new Spreadsheet("save2.txt", s => true, s => s, "4.0");
    }

    /// <summary>
    /// Should throw SpreadsheetReadWriteException if any circular dependencies are encountered
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(SpreadsheetReadWriteException))]
    public void ConstructorTest7()
    {
        using (XmlWriter writer = XmlWriter.Create("save2.txt"))
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("spreadsheet");
            writer.WriteAttributeString("version", "4.0");

            writer.WriteStartElement("cell");
            writer.WriteElementString("name", "a1");
            writer.WriteElementString("contents", "=a2 + 1");
            writer.WriteEndElement();

            writer.WriteStartElement("cell");
            writer.WriteElementString("name", "a2");
            writer.WriteElementString("contents", "=a1");
            writer.WriteEndElement();

            writer.WriteEndElement();
            writer.WriteEndDocument();
        }

        Spreadsheet sheet = new Spreadsheet("save2.txt", s => true, s => s, "4.0");
    }

    /// <summary>
    /// Should throw SpreadsheetReadWriteException if any invalid formulas are encountered
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(SpreadsheetReadWriteException))]
    public void ConstructorTest8()
    {
        using (XmlWriter writer = XmlWriter.Create("save2.txt"))
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("spreadsheet");
            writer.WriteAttributeString("version", "4.0");

            writer.WriteStartElement("cell");
            writer.WriteElementString("name", "a1");
            writer.WriteElementString("contents", "=a2 1");
            writer.WriteEndElement();

            writer.WriteEndElement();
            writer.WriteEndDocument();
        }

        Spreadsheet sheet = new Spreadsheet("save2.txt", s => true, s => s, "4.0");
    }

    // **************** Save Tests **************** //

    [TestMethod]
    public void SaveTest1()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.Save("save3.txt");

        Assert.AreEqual(0, sheet.GetNamesOfAllNonemptyCells().Count());
    }

    [TestMethod]
    public void SaveTest2()
    {
        Spreadsheet sheet = new Spreadsheet(s => true, s => s, "3.0");
        sheet.SetContentsOfCell("a1", "1");
        sheet.SetContentsOfCell("a2", "2");
        sheet.SetContentsOfCell("a3", "3");

        sheet.Save("save4.txt");

        Spreadsheet sheet2 = new Spreadsheet("save4.txt", s => true, s => s, "3.0");
        Assert.AreEqual(3, sheet2.GetNamesOfAllNonemptyCells().Count());
        Assert.AreEqual(1.0, sheet2.GetCellValue("a1"));
        Assert.AreEqual(2.0, sheet2.GetCellValue("a2"));
        Assert.AreEqual(3.0, sheet2.GetCellValue("a3"));
    }

    // **************** GetSavedVersion Tests **************** //

    [TestMethod]
    public void GetSavedVersionTest1()
    {
        Spreadsheet sheet = new Spreadsheet();

        sheet.Save("save5.txt");
        Assert.AreEqual("default", new Spreadsheet().GetSavedVersion("save5.txt"));
    }

    [TestMethod]
    public void GetSavedVersionTest2()
    {
        Spreadsheet sheet = new Spreadsheet(s => true, s => s, "4.0");
        sheet.SetContentsOfCell("a1", "1");
        sheet.SetContentsOfCell("a2", "2");
        sheet.SetContentsOfCell("a3", "3");

        sheet.Save("save6.txt");
        Assert.AreEqual(sheet.Version, new Spreadsheet().GetSavedVersion("save6.txt"));
    }

    /// <summary>
    /// Invalid filename should throw SpreadsheetReadWriteException
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(SpreadsheetReadWriteException))]
    public void GetSavedVersionTest3()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.GetSavedVersion("");
    }

    /// <summary>
    /// Null version should throw SpreadsheetReadWriteException
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(SpreadsheetReadWriteException))]
    public void GetSavedVersionTest4()
    {
        using (XmlWriter writer = XmlWriter.Create("save.txt"))
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("spreadsheet");

            writer.WriteStartElement("cell");
            writer.WriteElementString("name", "a1");
            writer.WriteElementString("contents", "=a2 1");
            writer.WriteEndElement();

            writer.WriteEndElement();
            writer.WriteEndDocument();
        }

        Spreadsheet sheet = new Spreadsheet();
        sheet.GetSavedVersion("save.txt");
    }

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

    // **************** GetCellValue Tests **************** //

    /// <summary>
    /// Number content
    /// </summary>
    [TestMethod]
    public void GetCellValueTest1()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetContentsOfCell("a1", "10");
        Assert.AreEqual(10.0, sheet.GetCellValue("a1"));
    }


    /// <summary>
    /// String content
    /// </summary>
    [TestMethod]
    public void GetCellValueTest2()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetContentsOfCell("b1", "abc");
        Assert.AreEqual("abc", sheet.GetCellValue("b1"));
    }

    /// <summary>
    /// Formula content
    /// </summary>
    [TestMethod]
    public void GetCellValueTest3()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetContentsOfCell("c1", "=1+2");
        Assert.AreEqual(3.0, sheet.GetCellValue("c1"));
    }

    /// <summary>
    /// If the cell is empty, the contents and value are an empty strings
    /// </summary>
    [TestMethod]
    public void GetCellValueTest4()
    {
        Spreadsheet sheet = new Spreadsheet();
        Assert.AreEqual("", sheet.GetCellValue("a1"));
    }

    /// <summary>
    /// Should throw InvalidNameException if the name is invalid
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void GetCellValueTest5()
    {
        Spreadsheet sheet = new Spreadsheet();
        Assert.AreEqual("", sheet.GetCellValue("1A"));
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

    // **************** SetContentsOfCell Tests **************** //

    // number contents //

    /// <summary>
    /// Simple SetContentsOfCell
    /// </summary>
    [TestMethod]
    public void SetContentsOfCellTest1()
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
    /// Simple SetContentsOfCell
    /// </summary>
    [TestMethod]
    public void SetContentsOfCellTest2()
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
    public void SetContentsOfCellTest3()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetContentsOfCell("", "0");
    }

    /// <summary>
    /// Invalid name should throw InvalidNameException
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void SetContentsOfCellTest4()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetContentsOfCell("1a", "0");
    }


    // string contents //

    /// <summary>
    /// Simple SetContentsOfCell
    /// </summary>
    [TestMethod]
    public void SetContentsOfCellTest5()
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
    public void SetContentsOfCellTest6()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetContentsOfCell("", "a1");
    }

    /// <summary>
    /// Invalid name should throw InvalidNameException
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void SetContentsOfCellTest7()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetContentsOfCell("1a", "a1");
    }

    /// <summary>
    /// Invalid formula should throw FormulaFormatException
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void SetContentsOfCellTest8()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetContentsOfCell("a1", "=(+)/1");
    }

    /// <summary>
    /// Empty text means empty cell. Should be removed from cells
    /// </summary>
    [TestMethod]
    public void SetContentsOfCellTest9()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetContentsOfCell("a1", "1");
        Assert.AreEqual(1, sheet.GetNamesOfAllNonemptyCells().Count());
        sheet.SetContentsOfCell("a1", "");
        Assert.AreEqual(0, sheet.GetNamesOfAllNonemptyCells().Count());
    }


    // Formula contents //

    /// <summary>
    /// Invalid formulas formula parameter should throw FormulaFormatException
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void SetContentsOfCell10()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetContentsOfCell("a1", "=1--2");
    }

    /// <summary>
    /// Invalid name should throw InvalidNameException
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void SetContentsOfCell11()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetContentsOfCell("123a", "=1+2");
    }

    /// <summary>
    /// Invalid name should throw InvalidNameException
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void SetContentsOfCell12()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetContentsOfCell("1_", "=1+2");
    }

    /// <summary>
    /// Should throw CircularException
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(CircularException))]
    public void SetContentsOfCell13()
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
    public void SetContentsOfCell14()
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
    public void SetContentsOfCell15()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetContentsOfCell("x1", "=x1-1");
    }

    /// <summary>
    /// Should throw CircularException
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(CircularException))]
    public void SetContentsOfCell16()
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
    public void SetContentsOfCell17()
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

    // **************** Changed Tests **************** //

    [TestMethod]
    public void ChangedTest1()
    {
        Spreadsheet sheet = new Spreadsheet();
        sheet.SetContentsOfCell("a1", "1");
        Assert.IsTrue(sheet.Changed);

        sheet.Save("save.txt");
        Assert.IsFalse(sheet.Changed);

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
