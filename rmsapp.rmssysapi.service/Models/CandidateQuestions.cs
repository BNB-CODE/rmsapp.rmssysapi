using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace rmsapp.rmssysapi.service.Models
{
    public class CandidateQuestions
    {
        public int QuestionId { get; set; }
        public string Version { get; set; }
        public string SubjectName { get; set; }
        public string Question { get; set; }
        public string QuestionType { get; set; }
        public string[] QuestionOptions { get; set; }
    }
}


