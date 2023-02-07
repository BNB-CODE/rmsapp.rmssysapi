using System;
using System.Collections.Generic;
using System.Text;

namespace rmsapp.rmssysapi.service.Models
{
    public class InterviewerQuizResponse
    {
        public int QuizId { get; set; }
        public string QuizLink { get; set; }
        public DateTime? QuizLinkExpiresAt { get; set; }
    }
}
