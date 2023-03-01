using System;
using System.Collections.Generic;
using System.Text;

namespace rmsapp.rmssysapi.service.Models
{
    public class SubmittedQuizResponse
    {
        public int QuizId { get; set; }
        public string CandidateId { get; set; }
        public int TotalQuestions { get; set; }
        public int AnsweredQuestions { get; set; }
        public int NotAnsweredQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public int InCorrectAnswers { get; set; }
        public int InterviewLevel { get; set; }
        public string CreatedBy { get; set; } = "Test@User";
        public string CreatedDate { get; set; }
    }
    public class SubmittedQuizDetailedInfo
    {
        public int QuizId { get; set; }
        public string SubjectName { get; set; }
        public string Version { get; set; }
        public int TotalQuestions { get; set; }
        public int AnsweredQuestions { get; set; }
        public int NotAnsweredQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public int InCorrectAnswers { get; set; }
    }
}
