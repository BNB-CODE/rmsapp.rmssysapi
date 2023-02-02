using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace rmsapp.rmssysapi.service.Models
{
    public class MasterQuiz: BaseModel
    {
        [Key]
        [Column(Order = 0)]
        public int QuestionId { get; set; }
        [Key]
        [Column(Order = 1)]
        public int  SetNumber { get; set; }
        [Key]
        [Column(Order = 2)]
        public string SubjectName { get; set; }
        public string Question { get; set; }
        public string QuestionType { get; set; }
        public string[] QuestionOptions { get; set; }
        public string[] QuestionAnswers { get; set; }
        public string[] QuestionAnswersIds { get; set; }
    }
}
