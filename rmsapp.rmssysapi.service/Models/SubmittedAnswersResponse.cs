using System;
using System.Collections.Generic;
using System.Text;

namespace rmsapp.rmssysapi.service.Models
{
    public class SubmittedQuiz
    {
       public List<SubmittedAnswersResponse> Data { get; set; }
    }
    public class SubmittedAnswersResponse
    {
    public int QuestionId { get; set; }
    public int SetNumber { get; set; }
    public string SubjectName { get; set; }
    public string Question { get; set; }
    public string QuestionType { get; set; }
    public string[] QuestionOptions { get; set; }
    public string[] SubmittedAnswers { get; set; }
    public string[] SubmittedAnswersIds { get; set; }
    public string[] MasterQuestionAnswers { get; set; }
    public string[] MasterQuestionAnswersIds { get; set; }
}
}
