using System;
using System.Collections.Generic;
using System.Text;

namespace rmsapp.rmssysapi.service.Models
{
    public class QuizSumissionRequest
    {
        public int QuizId { set; get; }
        public List<QuizInfo> Data { set; get; }

    }
    public class QuizInfo
    {
        public string SubjectName { set; get; }
        public int SetNumber { set; get; }
        public QuizAnswer[] quizAnswers { set; get; }
    }
    public class QuizAnswer
    {
        public int QuestionId { set; get; }
        public string QuestionType { set; get; }
        public string[] QuestionAnswers { set; get; }
        public string[] QuestionAnswerIds { set; get; }
    }
}
