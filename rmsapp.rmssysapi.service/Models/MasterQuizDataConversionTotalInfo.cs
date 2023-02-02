using System;
using System.Collections.Generic;
using System.Text;

namespace rmsapp.rmssysapi.service.Models
{
    public class MasterQuizDataConversionTotalInfo
    {
        public List<QuizDetails> Data { get; set; }
        public List<ExcelErrors> Error { get; set; }
    }
    public class QuizDetails
    {
        public string Question { get; set; }
        public string QuestionType { get; set; }
        public string QuestionOptions { get; set; }
        public string QuestionAnswers { get; set; }
        public string QuestionAnswerIds { get; set; }
    }
    public class ExcelErrors
    {
        public string Msg { get; set; }
        public int RowNumber { get; set; }
        public string Coulumn { get; set; }
    }
}
