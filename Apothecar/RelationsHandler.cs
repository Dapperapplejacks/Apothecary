using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;


namespace Apothecary
{
    class RelationsHandler
    {

        private Dictionary<string, List<EssentialOil>> relations;

        public RelationsHandler()
        {
            relations = RetrieveRelations();
        }

        public void AddRelation(EssentialOil firstOil, EssentialOil secondOil)
        {
            List<EssentialOil> list = relations[firstOil.Name];
            list.Add(secondOil);
        }

        public void RemoveRelation(EssentialOil firstOil, EssentialOil secondOil)
        {
            List<EssentialOil> list = relations[firstOil.Name];
            list.Remove(secondOil);
        }

        private Dictionary<string, List<EssentialOil>> RetrieveRelations()
        {
            Dictionary<string, List<EssentialOil>> ret = new Dictionary<string,List<EssentialOil>>();

            //Read excel workbook
            Workbook wb = ExcelHandler.RelationWorkbook;
            Worksheet ws = wb.Worksheets[0];
            
            //Retrieve all relations
            for (int i = 0; i < ws.Columns.Count; i++)
            {
                Range column = ws.Columns[i];
                string key = column[0].Value;

                List<EssentialOil> list = new List<EssentialOil>();
                for (int j = 1; j < column.Count; j++)
                {
                    list.Add(new EssentialOil(column[i]));
                }

                ret[key] = list;
            }

            return ret;
        }
    }
}
