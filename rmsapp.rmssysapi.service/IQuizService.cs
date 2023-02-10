using rmsapp.rmssysapi.service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace rmsapp.rmssysapi.service
{
    public interface IQuizService
    {
        Task<int> GteLatestQuizId();
        Task<bool> Add(Quiz quiz);
        Task<bool> UpdateQuizInfo(Quiz quiz);
        Task<Quiz> GetQuizDetails(int quizId);
    }
}
