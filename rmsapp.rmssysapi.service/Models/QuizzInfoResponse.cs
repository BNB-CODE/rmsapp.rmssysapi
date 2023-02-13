using System;
using System.Collections.Generic;
using System.Text;

namespace rmsapp.rmssysapi.service.Models
{
    public class QuizzInfoResponse
    {
        public int QuizId { get; set; }
        public string CandidateId { get; set; }
        public string QuizCodeExpirationAt { get; set; }
        public string QuizSubmittedAt { get; set; }
        public int LoginAttempts { get; set; }
        public string LastLoggedIn { get; set; }
        public string Url { get; set; }
    }
}
