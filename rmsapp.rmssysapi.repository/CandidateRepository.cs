using rmsapp.rmssysapi.service.DependentInterfaces;
using rmsapp.rmssysapi.service.Models;
using System.Threading.Tasks;

namespace rmsapp.rmssysapi.repository
{
    public class CandidateRepository: ICandidateRepository
    {
        private readonly PostgreSqlContext _dbContext;

        public CandidateRepository(PostgreSqlContext context)
        {
            _dbContext = context;

        }
        #region Update User info
        public async Task<bool> AddUserInfo(Candidate candidate)
        {
            bool result = false;
            if (candidate != null)
            {
              await _dbContext.Candidate.AddAsync(candidate);
              await _dbContext.SaveChangesAsync();
              result = true;
            }
            return result;
        }
        #endregion
    }
}
