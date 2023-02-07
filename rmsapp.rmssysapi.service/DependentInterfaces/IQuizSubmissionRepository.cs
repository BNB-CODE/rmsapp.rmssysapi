using rmsapp.rmssysapi.service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace rmsapp.rmssysapi.service.DependentInterfaces
{
    public interface IQuizSubmissionRepository
    {
        Task<bool> Add(QuizSubmission quiz);
        Task<QuizSubmission> GetQuizDetails(int quizId);
    }
}
