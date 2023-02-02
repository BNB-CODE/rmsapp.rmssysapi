using OfficeOpenXml;
using System.Collections.Generic;

namespace rmsapp.rmssysapi.service.Utils
{
    public class ExcelHeader
    {
        public Dictionary<string, int> Compileindex(ExcelWorksheet worksheet, int headerRowNumber)
        {
            Dictionary<string, int> objDict = new Dictionary<string, int>();
            int columncount = worksheet.Dimension.Columns;
            for (int i = 1; i <= columncount; i++)
            {
                var columnName = worksheet.Cells[headerRowNumber, i].Value;
                if (columnName != null)
                {
                    switch (columnName)
                    {
                        case "Question":
                            objDict.Add("question", i);
                            break;
                        case "Question Type":
                            objDict.Add("questiontype", i);
                            break;
                        case "Options":
                            objDict.Add("questionoptions", i);
                            break;
                        case "Text_Answer(s)":
                            objDict.Add("questionanswers", i);
                            break;
                        //case "Answer_Id(s)":
                        //    objDict.Add("questionanswerids", i);
                        //    break;
                        default:
                            break;
                    }
                }
            }
            return objDict;
        }

    }
}
