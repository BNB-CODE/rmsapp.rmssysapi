using rmsapp.rmssysapi.service.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace rmsapp.rmssysapi.service
{
    public interface ICandidateService
    {
        Task<bool> AddUserInfo(Candidate candidate);
        Task<IEnumerable<Candidate>> GetTotalCandidateDetails();

    }
}
