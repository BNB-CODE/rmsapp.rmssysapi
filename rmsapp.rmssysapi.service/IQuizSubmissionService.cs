using rmsapp.rmssysapi.service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace rmsapp.rmssysapi.service
{
    public interface IQuizSubmissionService
    {
        Task<bool> Add(QuizSubmission quiz);
        Task<QuizSubmission> GetQuizDetails(int quizId);
    }
}
