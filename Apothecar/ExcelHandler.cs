using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;

namespace Apothecary
{
    /// <summary>
    /// Handles reading and writing to Excel workbook
    /// </summary>
    class ExcelHandler
    {
        private static Microsoft.Office.Interop.Excel.Application xlApp;
        private static Workbooks workbooks;
        private static Workbook relationWorkbook;
        private static Workbook descriptorWorkbook;

        public static Workbook RelationWorkbook
        {
            get
            {
                return relationWorkbook;
            }
            private set
            {
                relationWorkbook = value;
            }
        }

        public static Workbook DescriptorWorkbook
        {
            get
            {
                return descriptorWorkbook;
            }
            private set
            {
                descriptorWorkbook = value;
            }
        }
        
        private static const string relationsFileName = "relations.xlsx";
        private static const string descriptorsFileName = "descriptors.xlsx";

        public ExcelHandler()
        {
            xlApp = new Microsoft.Office.Interop.Excel.Application();

            if (xlApp == null)
            {
                MessageBox.Show("Invalid or missing Microsoft Excel program.\n\nExiting Apothecary.", "Error");
                throw new Exception();
            }

            workbooks = xlApp.Workbooks;
            relationWorkbook = GetRelations();
            descriptorWorkbook = GetDescriptors();
        }

        private static Workbook GetRelations()
        {
            //Try to open Relations workbook
            try
            {
                Workbook workbook = workbooks.Open(relationsFileName, Type.Missing, true);

                Console.WriteLine("Found Relations workbook");
                return workbook;
            }
            //Else add a new one    
            catch (Exception ex)
            {
                Console.WriteLine("Adding new Relations workbook");

                return workbooks.Add();
            }
        }

        private static Workbook GetDescriptors()
        {
            //Try to open Descriptor workbook
            try
            {
                Workbook workbook = workbooks.Open(descriptorsFileName, Type.Missing, true);

                Console.WriteLine("Found Descriptor workbook");
                return workbook;
            }
            //Else add a new one    
            catch (Exception ex)
            {
                Console.WriteLine("Adding new Descriptor workbook");

                return workbooks.Add();
            }
        }

        /// <summary>
        /// Used for properly releasing excel resources
        /// </summary>
        public void CleanUp()
        {
            xlApp.Quit();

            Marshal.ReleaseComObject(relationWorkbook.Worksheets[0]);
            Marshal.ReleaseComObject(descriptorWorkbook.Worksheets[0]);

            Marshal.ReleaseComObject(relationWorkbook);
            relationWorkbook = null;
            Marshal.ReleaseComObject(descriptorWorkbook);
            descriptorWorkbook = null;
            
            Marshal.ReleaseComObject(workbooks);
            workbooks = null;
            Marshal.ReleaseComObject(xlApp);
            xlApp = null;
        }
    }
}

