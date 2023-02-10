using rmsapp.rmssysapi.service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace rmsapp.rmssysapi.service.DependentInterfaces
{
    public interface ICandidateRepository
    {
        Task<bool> AddUserInfo(Candidate quiz);
    }
}
