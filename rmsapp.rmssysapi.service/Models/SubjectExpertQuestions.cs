using System;
using System.Collections.Generic;
using System.Text;

namespace rmsapp.rmssysapi.service.Models
{
    public class SubjectExpertQuestions
    {
        public int QuestionId { get; set; }
        public int SetNumber { get; set; }
        public string SubjectName { get; set; }
        public string Question { get; set; }
        public string QuestionType { get; set; }
        public string[] QuestionOptions { get; set; }
        public string[] QuestionAnswers { get; set; }
        public string[] QuestionAnswersIds { get; set; }
    }
}
