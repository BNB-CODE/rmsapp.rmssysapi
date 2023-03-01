
using Microsoft.EntityFrameworkCore;

namespace rmsapp.rmssysapi.service.Models
{
    public class InterviewerQuizSet
    {
        public int QuestionId { get; set; }
        public string Version { get; set; }
        public string SubjectName { get; set; }
    }
}
