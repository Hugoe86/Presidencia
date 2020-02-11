﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.IO;

namespace ExportToExcel
{
    public class CreateExcelFile
    {
        public static bool CreateExcelDocument<T>(List<T> list, string xlsxFilePath)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(ListToDataTable(list));

            return CreateExcelDocument(ds, xlsxFilePath);
        }

        //  This function is taken from: http://www.codeguru.com/forum/showthread.php?t=450171
        public static DataTable ListToDataTable<T>(List<T> list)
        {
            DataTable dt = new DataTable();

            foreach (PropertyInfo info in typeof(T).GetProperties())
            {
                dt.Columns.Add(new DataColumn(info.Name, info.PropertyType));
            }
            foreach (T t in list)
            {
                DataRow row = dt.NewRow();
                foreach (PropertyInfo info in typeof(T).GetProperties())
                {
                    row[info.Name] = info.GetValue(t, null);
                }
                dt.Rows.Add(row);
            }
            return dt;
        }

        public static bool CreateExcelDocument(DataTable dt, string xlsxFilePath)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);

            return CreateExcelDocument(ds, xlsxFilePath);
        }

        public static bool CreateExcelDocument(DataSet ds, string xlsxFilePath)
        {
            //  Note: for this Export to Excel class to work, you need to have References to two .dll files:
            //      DocumentFormat.OpenXml.dll
            //      WindowBase.dll
            // 
            //  The "DocumentFormat.OpenXml.dll" file can be downloaded from Microsoft, just Google "Open XML SDK" to find the latest download location.  
            //  It typically installs the relevant file to: "C:\Program Files\Open XML SDK\V2.0\lib"
            //
            //  The WindowBase.dll file is installed with Microsoft Framework, typically into the directory:
            //  "C:\Program Files\Reference Assemblies\Microsoft\Framework\v3.0\WindowsBase.dll"
            //
            //  References:
            //      "How to create an Excel 2007 file, from scratch:"
            //      http://blogs.msdn.com/b/chrisquon/archive/2009/07/22/creating-an-excel-spreadsheet-from-scratch-using-openxml.aspx
            //      "Writing data into excel document using openxml"
            //      http://www.prowareness.com/blog/?p=476
            //
            using (SpreadsheetDocument spreadSheet = SpreadsheetDocument.Create(xlsxFilePath, SpreadsheetDocumentType.Workbook, true))
            {
                // Create the Workbook
                spreadSheet.AddWorkbookPart();
                spreadSheet.WorkbookPart.Workbook = new Workbook();

                // A Workbook must only have exactly one <Sheets> section
                spreadSheet.WorkbookPart.Workbook.AppendChild(new Sheets());

                //  Loop through each of the DataTables in our DataSet, and create a new Excel Worksheet for each.
                uint worksheetNumber = 1;
                foreach (DataTable dt in ds.Tables)
                {
                    WorksheetPart newWorksheetPart = spreadSheet.WorkbookPart.AddNewPart<WorksheetPart>();
                    newWorksheetPart.Worksheet = new Worksheet();

                    //  Create a new Excel worksheet 
                    SheetData sheetData = newWorksheetPart.Worksheet.AppendChild(new SheetData());

                    //  Create a Header Row in our Excel file, containing one header for each Column of data in our DataTable.
                    //
                    //  We'll also create an array, showing which type each column of data is (Text or Numeric), so when we come to write the actual
                    //  cells of data, we'll know if to write Text values or Numeric cell values.
                    int numberOfColumns = dt.Columns.Count;
                    bool[] IsNumericColumn = new bool[numberOfColumns];

                    Row newRow = sheetData.AppendChild(new Row());
                    for (int colInx = 0; colInx < numberOfColumns; colInx++)
                    {
                        DataColumn col = dt.Columns[colInx];
                        Cell newHeaderCell = CreateTextCell(col.ColumnName);
                        IsNumericColumn[colInx] = (col.DataType.FullName == "System.Decimal");     //  eg "System.String" or "System.Decimal"
                        newRow.AppendChild(newHeaderCell);
                    }

                    //  Now, step through each row of data in our DataTable...
                    Cell newCell;
                    Console.Write ("\n");
                    int i = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        // ...create a new row, and append a set of this row's data to it.
                        newRow = sheetData.AppendChild(new Row());
                        for (int colInx = 0; colInx < numberOfColumns; colInx++)
                        {
                            string cellValue = dr.ItemArray[colInx].ToString();

                            // Create cell with data
                            if (IsNumericColumn[colInx] && !string.IsNullOrEmpty(cellValue))
                                newCell = CreateNumericCell(cellValue);
                            else
                                newCell = CreateTextCell(cellValue);

                            newRow.AppendChild(newCell);
                        }
                        
						if (++i % 10000 == 0)
							Console.Write (".");
                    }
                    newWorksheetPart.Worksheet.Save();

                    //  Link this worksheet to our workbook
                    spreadSheet.WorkbookPart.Workbook.GetFirstChild<Sheets>().AppendChild(new Sheet()
                    {
                        Id = spreadSheet.WorkbookPart.GetIdOfPart(newWorksheetPart),
                        SheetId = worksheetNumber++,
                        Name = dt.TableName
                    });
                }   // foreach (DataTable..)

                spreadSheet.WorkbookPart.Workbook.Save();

            }   // using (SpreadsheetDocument spreadSheet..)

            return true;
        }

        private static Cell CreateTextCell(string cellValue)
        {
            //  Create an Excel cell, containing a Text value
            Cell c = new Cell();
            c.DataType = CellValues.InlineString;

            //Add text to the text cell.
            InlineString inlineString = new InlineString();
            Text t = new Text();
            t.Text = cellValue.ToString();
            inlineString.AppendChild(t);
            c.AppendChild(inlineString);

            return c;
        }

        private static Cell CreateNumericCell(string cellValue)
        {
            //  Create an Excel cell, containing a numeric value
            Cell c = new Cell();

            if (string.IsNullOrEmpty(cellValue))
                return c;

            decimal decimalValue = 0;
            if (!decimal.TryParse(cellValue, out decimalValue))
                return c;

            //  If we don't specifically set a number of decimal places, sometimes it'll create invalid Excel 
            //  files ("Do you wish to repair..")
            CellValue v = new CellValue();
            v.Text = decimalValue.ToString("0.000000");
            c.AppendChild(v);

            return c;
        }
    }
}
