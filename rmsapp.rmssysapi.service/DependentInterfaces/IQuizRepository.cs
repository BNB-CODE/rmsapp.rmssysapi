using rmsapp.rmssysapi.service.Models;
using System.Threading.Tasks;

namespace rmsapp.rmssysapi.service.DependentInterfaces
{
    public interface IQuizRepository
    {
        Task<int> GteLatestQuizId();
        Task<bool> Add(Quiz quiz);
        Task<bool> UpdateUserInfo(Quiz quiz);
        Task<bool> UpdateQuizInfo(Quiz quiz);
        Task<Quiz> GetQuizDetails(int quizId);
    }
}
