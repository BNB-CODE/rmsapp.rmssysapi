﻿using Npgsql;
using rmsapp.rmssysapi.service.DependentInterfaces;
using rmsapp.rmssysapi.service.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace rmsapp.rmssysapi.service.Impl
{
    public class CandidateService:ICandidateService
    {
        private readonly ICandidateRepository _candidateRepository;

        public CandidateService(ICandidateRepository candidateRepository)
        {
            _candidateRepository = candidateRepository;

        }
        #region Add User info
        public async Task<bool> AddUserInfo(Candidate candidate)
        {
            try
            {
                var res = await _candidateRepository.AddUserInfo(candidate).ConfigureAwait(false);
                return res;
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"CandidateService::AddUserInfo:: Save candidate failed {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"CandidateService::AddUserInfo:: Save candidate failed {ex.Message}");
            }
        }
        #endregion
        #region get Total Candidate Details
        public async Task<IEnumerable<Candidate>> GetTotalCandidateDetails()
        {
            try
            {
                var candidates = await _candidateRepository.GetTotalCandidateDetails().ConfigureAwait(false);
                return candidates;
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"CandidateService::GetTotalCandidateDetails:: Fetch candidate failed {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"CandidateService::GetTotalCandidateDetails:: Fetch candidate failed {ex.Message}");
            }

        }
        #endregion
    }
}
