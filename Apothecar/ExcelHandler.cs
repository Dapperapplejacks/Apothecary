using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using System.IO;
using System.Runtime.InteropServices;

namespace Apothecary
{
    /// <summary>
    /// Handles reading and writing to Excel workbook
    /// </summary>
    class ExcelHandler
    {
        private Microsoft.Office.Interop.Excel.Application xlApp;
        private Workbooks workbooks;
        

        public ExcelHandler()
        {
            xlApp = new Microsoft.Office.Interop.Excel.Application();
            workbooks = xlApp.Workbooks;

            if (xlApp == null)
            {
                
                throw new Exception();
            }
        }

        /// <summary>
        /// Reads manifest excel file
        /// </summary>
        /// <param name="path">Path of manifest</param>
        /// <returns>Manifest object</returns>
        private Manifest ReadManifest(string path)
        {
            try
            {
                Workbook workbook = workbooks.Open(path, Type.Missing, true);
                manifest = new Manifest(workbook);

                Marshal.ReleaseComObject(workbook);
                workbook = null;

                return manifest;
            }
            catch (Exception ex)
            {
                throw new Exception("Error locating/reading UPC manifest");
            }
        }

        /// <summary>
        /// Creates metadata excel file and saves it in destination folder
        /// </summary>
        /// <param name="album">Album to make metadata file for</param>
        /// <param name="startEndDates">Array of start and end dates taken from GUI</param>
        public void CreateMetaData(Album album, string[] startEndDates)
        {
            Metadata metadata = new Metadata(workbooks.Add());
            metadata.PopulateSheet(album, ReadManifest(manifestPath), startEndDates);
            metadata.SaveFile(album.Path);

            CleanUp();

            xlApp = new Microsoft.Office.Interop.Excel.Application();
            workbooks = xlApp.Workbooks;
        }

        /// <summary>
        /// Used for properly releasing excel resources
        /// </summary>
        public void CleanUp()
        {
            xlApp.Quit();

            if (manifest != null)
            {
                manifest.CleanUp();
                manifest = null;
            }
            Marshal.ReleaseComObject(workbooks);
            workbooks = null;
            Marshal.ReleaseComObject(xlApp);
            xlApp = null;
        }
    }
}

