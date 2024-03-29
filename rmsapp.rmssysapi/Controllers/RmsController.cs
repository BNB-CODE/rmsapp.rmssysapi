﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using rmsapp.rmssysapi.service;
using rmsapp.rmssysapi.service.Models;
using rmsapp.rmssysapi.service.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace rmsapp.rmssysapi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/{v:apiVersion}")]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [ApiVersion("3.0")]
    public class RmsController : ControllerBase
    {
        private readonly IMasterQuizService _masterQuizService;
        private readonly IExcelDataConversionService _excelDataConversionService;
        private readonly ITemplateDownloadService _templateDownloadService;
        private readonly IQuizService _quizService;
        private readonly IQuizSubmissionService _quizSubmissionService;
        private readonly ICandidateService _candidateService;
        private readonly IConfiguration _configuration;
        public RmsController(IMasterQuizService masterQuizService, IExcelDataConversionService excelDataConversionService,
            ITemplateDownloadService templateDownloadService, IQuizService quizService,
            IQuizSubmissionService quizSubmissionService, ICandidateService candidateService,
            IConfiguration configuration)
        {
            _masterQuizService = masterQuizService;
            _excelDataConversionService = excelDataConversionService;
            _templateDownloadService = templateDownloadService;
            _quizService = quizService;
            _quizSubmissionService = quizSubmissionService;
            _candidateService = candidateService;
            _configuration = configuration;
        }
        #region Upload/Save Quiz Excel

        [HttpPost("quiz/import")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(200, Type = typeof(QuizDetails))]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Import(int setNumber, string SubjectName, IFormFile formFile, CancellationToken cancellationToken)
        {
            try
            {

                if (formFile == null || formFile.Length <= 0)
                {
                    return BadRequest("Excel file is empty");
                }

                if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    return BadRequest("File extension Not supported");
                }
                var list = await _excelDataConversionService.GetMasterQuizData(formFile, cancellationToken).ConfigureAwait(false);
                if (list.Count() > 0)
                {
                    var res = await _masterQuizService.Add(setNumber, SubjectName, list).ConfigureAwait(false);
                    if (res)
                    {
                        return Ok();
                    }
                }
                else
                {
                    return BadRequest("Excel file is empty");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }
        #endregion

        #region  Download Quiz Excel Template
        [HttpGet("quiz/exportTemplate")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(200, Type = typeof(QuizDetails))]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Export()
        {
            MemoryStream resultStream = new MemoryStream();
            try
            {
                //string excelName = $"UnapprovedRepo.xlsx";
                //var stream = _templateDownloadService.DownloadUnapprovedRepoTemplate();
                //return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
                var stream = _templateDownloadService.DownloadQuizTemplate();
                string excelName = $"RMS Export Template - Quiz.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }
        #endregion

        #region Get Invidiual Subject and Set Wise total Info

        [HttpGet("quiz/SubjectExpert/questions")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(200, Type = typeof(SubjectExpertQuestions))]
        // [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> GetSubjectExpertQuestions(int set, string subject)
        {
            try
            {
                if (!string.IsNullOrEmpty(subject))
                {
                    subject = subject.ToUpper();
                }

                var res = await _masterQuizService.GetMasterQuestions(set, subject);

                if (res.Any())
                {
                    return Ok(res);
                }
                return NoContent();

            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }

        }
        #endregion

        #region Get Subjectwise All Sets and Qestions Count with grid data

        [HttpGet("quiz/SubjectExpert/allquestions")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(200, Type = typeof(SubjectDetails[]))]
        // [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> GetSubjectExpertQuestions(string subject)
        {
            try
            {
                if (!string.IsNullOrEmpty(subject))
                {
                    subject = subject.ToUpper();
                }

                var res = await _masterQuizService.GetQuizDetails(subject).ConfigureAwait(false);

                if (res.Any())
                {
                    return Ok(res);
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }

        }
        #endregion

        #region Get Candidate Questions
        [HttpGet("quiz/candidate/questions")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(200, Type = typeof(CandidateQuestions[]))]
        // [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> GetCandidateQuestions(int set, string subject)
        {
            try
            {
                if (!string.IsNullOrEmpty(subject))
                {
                    subject = subject.ToUpper();
                }

                var res = await _masterQuizService.GetCandidateAssignment(set, subject);

                if (res.Any())
                {
                    return Ok(res);
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }

        }
        #endregion

        #region Create Quiz
        [HttpPost("quiz/interviewer/createquiz")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(200, Type = typeof(InterviewerQuizResponse))]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> createquiz(List<InterviewerQuizSet> interviewerQuizRequest)
        {
            InterviewerQuizResponse quizResponse = new InterviewerQuizResponse();

            try
            {
                if (interviewerQuizRequest.Count>0)
                {
                    int maxQuestionId = await _quizService.GteLatestQuizId().ConfigureAwait(false);
                    var (confirmationCode, confirmationCodeExpiration) =GetInvitationCode();
                    Quiz quiz = new Quiz()
                    {
                        QuizId = maxQuestionId + 1,
                        CreatedDate = DateTime.Now,
                        IsActive = true,
                        ConfirmationCode = confirmationCode,
                        ConfirmationCodeExpiration = confirmationCodeExpiration,
                        QuizSetList = interviewerQuizRequest.Count > 0 ? interviewerQuizRequest.Select(x => new InterviewerQuizSet {
                            SetNumber = x.SetNumber, SubjectName = x.SubjectName.Trim().ToUpper() }).ToList() : new List<InterviewerQuizSet>()
                        //CreatedBy= Currentuser
                    };
                    var res = await _quizService.Add(quiz).ConfigureAwait(false);
                    if (res)
                    {
                        var quizDetails = await _quizService.GetQuizDetails(quiz.QuizId).ConfigureAwait(false);
                        if (quizResponse!=null)
                        {
                            quizResponse.QuizId = quizDetails.QuizId;
                            quizResponse.QuizLink = quizDetails.ConfirmationCode;
                            quizResponse.QuizLinkExpiresAt = quizDetails.ConfirmationCodeExpiration;
                        }
                        
                        return Ok(quizResponse);
                    }
                    else
                    {
                        return NoContent();
                    }
                }
                else
                {
                    return BadRequest("Please Add Quiz Details");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }
        #endregion

        #region check Quiz Code
        [HttpGet("quiz/interviewer/quizdetails")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(200, Type = typeof(QuizzInfoResponse[]))]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> quizdetails()
        {
            List<QuizzInfoResponse> quizzInfoResponses = new List<QuizzInfoResponse>();
            var quizUrl = _configuration.GetValue("QuizLink", "http://localhost:3000/rms-aug/test/");
            try
            {
                List<Quiz> quizDetails = (List<Quiz>)await _quizService.GetTotalQuizDetails().ConfigureAwait(false);
                if (quizDetails?.Count > 0)
                {
                    quizzInfoResponses = quizDetails.Select(x=>new QuizzInfoResponse {
                                         QuizId=x.QuizId,
                                         CandidateId=x.CandidateId,
                                         QuizCodeExpirationAt=x.ConfirmationCodeExpiration!=null? (x.ConfirmationCodeExpiration)?.ToString("dd MMMM yyyy hh:mm tt") : null,//.ToString("dddd,dd MMMM yyyy hh:mm tt") 
                        QuizSubmittedAt =x.QuizSubmittedAt != null ? (x.QuizSubmittedAt)?.ToString("dd MMMM yyyy hh:mm tt") : null,
                                         LoginAttempts =x.LoginAttempts,
                                         LastLoggedIn=x.LastLoggedIn != null ? (x.LastLoggedIn)?.ToString("dd MMMM yyyy hh:mm tt") : null,
                                         Url = quizUrl+x.QuizId+"/"+x.ConfirmationCode
                        //DateTime.Now.ToString("yyyyMMddHHmmss");
                    }).ToList();

                   return Ok(quizzInfoResponses);
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }
        #endregion

        #region check Quiz Code
        [HttpGet("quiz/candidate/checkquizid")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> checkquizid(int quizId, string confirmationCode)
        {;

            try
            {
                    if (quizId > 0 && !string.IsNullOrEmpty(confirmationCode))
                    {
                        var quizDetails = await _quizService.GetQuizDetails(quizId).ConfigureAwait(false);
                        if (quizDetails == null)
                        {
                            return BadRequest("interview link not found");
                        }
                        else if (quizDetails.ConfirmationCodeExpiration.Value <= DateTime.UtcNow)
                        {
                            return BadRequest("interview link expired");
                        }
                        else if (quizDetails.QuizSubmittedAt != null)
                        {
                            return BadRequest("interview already  submitted");
                        }
                        else if (quizDetails.ConfirmationCode != confirmationCode)
                        {
                            return BadRequest("invalid interview link");
                        }
                        else
                        {
                        return Ok();
                        }
                    }
                    else
                    {
                    return BadRequest("Please provide valid quizId & confirmation code");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }
        #endregion

        #region Save User With Quiz
        [HttpPost("quiz/candidate/adduser")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(200, Type = typeof(CandidateQuestions[]))]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> AddUser(int quizId,string confirmationCode,AddUserRequest interviewerQuizRequest)
        {
            List<CandidateQuestions> quizResponse = new List<CandidateQuestions>();

            try
            {
                if (quizId>0 && interviewerQuizRequest != null)
                {
                    var quizDetails = await _quizService.GetQuizDetails(quizId).ConfigureAwait(false);
                    if (quizDetails == null)
                    {
                        return BadRequest("interview link not found");
                    }
                    else if (quizDetails.ConfirmationCodeExpiration.Value <= DateTime.UtcNow)
                    {
                        return BadRequest("interview link expired");
                    }
                    else if (quizDetails.QuizSubmittedAt!=null)
                    {
                        return BadRequest("interview already  submitted");
                    }
                    else if (quizDetails.ConfirmationCode != confirmationCode)
                    {
                        return BadRequest("invalid interview link");
                    }

                    if (quizDetails != null)
                    {

                        foreach (var item in quizDetails.QuizSetList)
                        {
                            List<CandidateQuestions> quizzes = new List<CandidateQuestions>();
                            quizzes = (List<CandidateQuestions>)await _masterQuizService.GetCandidateAssignment(item.SetNumber, ((item.SubjectName).Trim()).ToUpper()).ConfigureAwait(false);
                            if (quizzes?.Count > 0)
                            {
                                quizResponse.AddRange(quizzes);
                            }
                        }
                        string candidateId= string.IsNullOrEmpty(interviewerQuizRequest.Email) ? string.Empty : ((interviewerQuizRequest.Email).Trim()).ToUpper();
                        Candidate candidate = new Candidate {
                            FirstName = interviewerQuizRequest.FirstName,
                            MiddleName = interviewerQuizRequest.MiddleName,
                            LastName = interviewerQuizRequest.LastName,
                            Email = interviewerQuizRequest.Email,
                            CandidateId = candidateId,
                            Phone = interviewerQuizRequest.Phone,
                            IsActive = true,
                            CreatedDate = DateTime.Now
                        };
                        quizDetails.CandidateId = candidateId;
                        var res = await _candidateService.AddUserInfo(candidate).ConfigureAwait(false);
                        await _quizService.UpdateQuizInfo(quizDetails).ConfigureAwait(false);
                        if (res)
                        {
                            return Ok(quizResponse);
                        }
                    }

                   return BadRequest("please provide valid info");
                }
                else
                {
                    return BadRequest("Please Add Quiz Details");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }
        #endregion

        #region Submitt Quiz
        [HttpPost("quiz/interviewer/submitquiz")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> SubmitQuiz(QuizSumissionRequest quizSumissionRequest)
        {
            try
            {
                if (quizSumissionRequest!=null)
                {
                    var quizDetails = await _quizService.GetQuizDetails(quizSumissionRequest.QuizId).ConfigureAwait(false);
                    if (quizDetails != null)
                    {
                        List<QuizInfo> subMittedQuizInfo = quizSumissionRequest.Data?.Count() > 0 ? quizSumissionRequest.Data : new List<QuizInfo>();
                        List<SubjectExpertQuestions> masterQuizzes = new List<SubjectExpertQuestions>();
                        List<QuizInfo> masterQuizInfo = new List<QuizInfo>();
                        foreach (var item in quizDetails.QuizSetList)
                        {
                            List<SubjectExpertQuestions> quizzes = new List<SubjectExpertQuestions>();
                            quizzes =(List<SubjectExpertQuestions>)await _masterQuizService.GetMasterQuestions(item.SetNumber, item.SubjectName).ConfigureAwait(false);
                            if (quizzes?.Count>0)
                            {
                                masterQuizzes.AddRange(quizzes);
                            }
                        }
                        if (masterQuizzes?.Count > 0)
                        {
                            masterQuizInfo = (from x in masterQuizzes
                                              group x by new
                                              {
                                                  x.SetNumber,
                                                  x.SubjectName,
                                              } into g
                                              select new QuizInfo
                                              {
                                                  SetNumber = g.Key.SetNumber,
                                                  SubjectName = g.Key.SubjectName,
                                                  quizAnswers= masterQuizzes.Where(x=>x.SetNumber== g.Key.SetNumber
                                                               && x.SubjectName== g.Key.SubjectName).Select(k=>new QuizAnswer {
                                                                   QuestionId=k.QuestionId,
                                                                   QuestionType=k.QuestionType,
                                                                   QuestionAnswers=k.QuestionAnswers,
                                                                   QuestionAnswerIds=k.QuestionAnswersIds
                                                               }).ToArray()
                                              }).ToList();
                        }
                        QuizSubmission quiz = new QuizSubmission()
                        {
                            QuizId = quizDetails.QuizId,
                            CandidateMailId = quizDetails.CandidateId,
                            IsActive = true,
                            QuizSetList = quizDetails.QuizSetList,
                            SubmittedAnswersInfo = subMittedQuizInfo,
                            MasterAnswersInfo = masterQuizInfo,
                            UpdatedDate = DateTime.Now
                            //CreatedBy= Currentuser
                        };
                        quizDetails.QuizSubmittedAt = DateTime.Now;
                        quizDetails.LastLoggedIn = DateTime.Now;
                        quizDetails.LoginAttempts = quizDetails.LoginAttempts+ 1;
                        var quizsubmission =await _quizSubmissionService.Add(quiz).ConfigureAwait(false);
                        await _quizService.UpdateQuizInfo(quizDetails).ConfigureAwait(false);
                        if (quizsubmission)
                        {
                            return Ok();
                        }
                    }
                    return BadRequest("Please provide quid id Details");
                }
                else
                {
                    return BadRequest("Please Add Quiz Details");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }
        #endregion
        private static (string, DateTime) GetInvitationCode() =>
        GetConfirmationCode(AccountOptions.InvitationCodeDuration);
        private static (string, DateTime) GetConfirmationCode(TimeSpan duration)
        {
            var (confirmationCode, confirmationCodeExpiration, _) =
                    Secrets.Create(
                        DateTime.Now.Add(duration));
            confirmationCode = confirmationCode
                .Replace('+', '_')
                .Replace('/', '-')
                .Replace('=', '!');
            return (confirmationCode, confirmationCodeExpiration);
        }
    }
}
