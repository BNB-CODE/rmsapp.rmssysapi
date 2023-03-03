using rmsapp.rmssysapi.service.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace rmsapp.rmssysapi.service.DependentInterfaces
{
    public interface IQuizRepository
    {
        Task<int> GteLatestQuizId();
        Task<bool> Add(Quiz quiz);
        Task<bool> UpdateQuizInfoByInterviewer(Quiz quiz);
        Task<bool> UpdateQuizInfo(Quiz quiz);
        Task<Quiz> GetQuizDetails(int quizId);
        Task<IEnumerable<Quiz>> GetTotalQuizDetails();
    }
}
