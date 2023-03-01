
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;

namespace rmsapp.rmssysapi.service.Models
{
  
    public class InterviewerQuizSet
    {
        public int[] QuestionIds { get; set; }
        public string Version { get; set; }
        public string SubjectName { get; set; }
    }
    public class CreateQuizRequest
    {
        public string QuizTopic { get; set; }
        public int TotalQuestions { get; set; }
        public int QuizTimeInMinutes { get; set; }
        public int QuizLinkExpireInHours { get; set; }
        public InterviewerQuizSet[] QuizSetWiseInfo { get; set; }
    }
}
