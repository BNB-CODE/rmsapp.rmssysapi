using Microsoft.EntityFrameworkCore;
using rmsapp.rmssysapi.service.DependentInterfaces;
using rmsapp.rmssysapi.service.Models;
using System.Collections.Generic;
using System.Linq;
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
        #region Get All Candidates
        public async Task<IEnumerable<Candidate>> GetTotalCandidateDetails()
        {
            var candidates = await _dbContext.Candidate.Where(x=>x.IsActive).ToListAsync().ConfigureAwait(false);
            return candidates;
        }
        #endregion
    }
}
