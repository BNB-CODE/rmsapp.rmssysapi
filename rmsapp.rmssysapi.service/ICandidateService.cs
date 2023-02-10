using rmsapp.rmssysapi.service.Models;
using System.Threading.Tasks;

namespace rmsapp.rmssysapi.service
{
    public interface ICandidateService
    {
        Task<bool> AddUserInfo(Candidate candidate);

    }
}
