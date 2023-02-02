using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using rmsapp.rmssysapi.service.Models;
using rmsapp.rmssysapi.service.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace rmsapp.rmssysapi.service.Impl
{
    public class ExcelDataConversionService : IExcelDataConversionService
    {
        public async Task<IEnumerable<QuizDetails>> GetMasterQuizData(IFormFile formFile, CancellationToken cancellationToken)
        {
            var list = new List<QuizDetails>();
            Header header = new Header();
            using (var stream = new MemoryStream())
            {
                await formFile.CopyToAsync(stream, cancellationToken);

                using (var package = new ExcelPackage(stream))
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[UploadExcelSheetOrder.Quiz];
                    var rowCount = worksheet.Dimension.Rows;
                    int headerRowNumber = 1;
                    ExcelHeader Headerdata = new ExcelHeader();
                    var headerInfo = Headerdata.Compileindex(worksheet, headerRowNumber);
                    header = new Header() { NameToColumnNumberDict = headerInfo };
                    int EmptyRowCount = 0;
                    for (int row = 2; row <= rowCount; row++)
                    {
                        if (EmptyRowCount >= 10)
                        {
                            //if continuesly rows are empty we need to stop reading
                            break;
                        }
                        for (int col = 1; col <= 1; col++)
                        {
                            bool isRowColumnNonEmptyValues = ExcelRowValidation(row, worksheet, header);
                            if (isRowColumnNonEmptyValues)
                            {
                                QuizDetails data = new QuizDetails();
                                data.Question = worksheet.Cells[row, header.Question].Value != null ? worksheet.Cells[row, header.Question].Value.ToString().Trim() : null;
                                data.QuestionType = worksheet.Cells[row, header.QuestionType].Value != null ? worksheet.Cells[row, header.QuestionType].Value.ToString().Trim() : null;
                                data.QuestionOptions = worksheet.Cells[row, header.QuestionOptions].Value != null ? worksheet.Cells[row, header.QuestionOptions].Value.ToString().Trim() : null;
                                data.QuestionAnswers = worksheet.Cells[row, header.QuestionAnswers].Value != null ? worksheet.Cells[row, header.QuestionAnswers].Value.ToString().Trim() : null;
                                //data.QuestionAnswerIds = worksheet.Cells[row, header.QuestionAnswerIds].Value != null ? worksheet.Cells[row, header.QuestionAnswerIds].Value.ToString().Trim() : null;
                                list.Add(data);
                            }
                            else
                            {
                                EmptyRowCount++;
                            }
                        }
                    }
                }
            }
            return list;
        }

        private bool ExcelRowValidation(int row, ExcelWorksheet worksheet, Header header)
        {
            bool isRowColumnNonEmptyValues = worksheet.Cells[row, header.Question].Value != null ||
                                 worksheet.Cells[row, header.QuestionType].Value != null ||
                                 worksheet.Cells[row, header.QuestionOptions].Value != null || worksheet.Cells[row, header.QuestionAnswers].Value != null; 
                                 //|| worksheet.Cells[row, header.QuestionAnswerIds].Value != null;
            return isRowColumnNonEmptyValues;
        }
    }
}
