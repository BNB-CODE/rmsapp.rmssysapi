
using System.Collections.Generic;

namespace rmsapp.rmssysapi.service.Models
{
    public class QuizSumissionRequest
    {
        public int QuizId { set; get; }
        public int TotalQuestions { get; set; }
        public int AnsweredQuestions { get; set; }
        public int NotAnsweredQuestions { get; set; }
        public List<QuizInfo> Data { set; get; }

    }
    public class QuizInfo
    {
        public string SubjectName { set; get; }
        public string Version { get; set; }
        public QuizAnswer[] quizAnswers { set; get; }
    }
    public class QuizAnswer
    {
        public int QuestionId { set; get; }
        public string QuestionType { set; get; }
        public string[] QuestionAnswers { set; get; }
        public string[] QuestionAnswerIds { set; get; }
    }

    public class QuizAnswersDetailedInfo
    {
        public string SubjectName { set; get; }
        public string Version { set; get; }
        public int TotalQuestions { get; set; }
        public int TotalAnsweredQuestions { get; set; }
        public int TotalUnAnsweredQuestions { get; set; }
        public int TotalCorrectAnswers { get; set; }
        public int TotalInCorrectAnswers { get; set; }
        public QuizAnswerTotalDetails[] QuizAnswersDetails { set; get; }
    }
    public class QuizAnswerTotalDetails
    {
        public int QuestionId { set; get; }
        public string QuestionType { set; get; }
        public string[] SubmittedQuestionAnswers { set; get; }
        public string[] SubmittedQuestionAnswerIds { set; get; }
        public string[] MasterQuestionAnswers { set; get; }
        public string[] MasterQuestionAnswerIds { set; get; }
        public bool  IsCorrect { set; get; }
    }
}
