using Microsoft.AspNetCore.Http;
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
                            SetNumber = x.SetNumber, SubjectName = x.SubjectName.Trim().ToUpper(),TotalQuestions=x.TotalQuestions }).ToList() : new List<InterviewerQuizSet>()
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

        #region Get Total Created Quiz Details
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
                        List<QuizAnswersDetailedInfo> clubedAnswers = new List<QuizAnswersDetailedInfo>();
                        List<QuizAnswersDetailedInfo> submittedAnswers = new List<QuizAnswersDetailedInfo>();
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
                            clubedAnswers = (from x in masterQuizzes
                                             group x by new
                                             {
                                                 x.SetNumber,
                                                 x.SubjectName
                                             } into g
                                             select new QuizAnswersDetailedInfo
                                             {
                                                 SetNumber = g.Key.SetNumber,
                                                 SubjectName = g.Key.SubjectName,
                                                 QuizAnswersDetails = g.GroupBy(k => new { k.QuestionId, k.QuestionType })
                                                                .Select(M => new QuizAnswerTotalDetails
                                                                {
                                                                    QuestionId = M.Key.QuestionId,
                                                                    QuestionType = M.Key.QuestionType,
                                                                    MasterQuestionAnswerIds = masterQuizzes.Where(x => x.SubjectName == g.Key.SubjectName && x.SetNumber == g.Key.SetNumber && x.QuestionId == M.Key.QuestionId && x.QuestionType == M.Key.QuestionType).SelectMany(x => x.QuestionAnswersIds).ToArray(),
                                                                    MasterQuestionAnswers = masterQuizzes.Where(x => x.SubjectName == g.Key.SubjectName && x.SetNumber == g.Key.SetNumber && x.QuestionId == M.Key.QuestionId && x.QuestionType == M.Key.QuestionType).SelectMany(x => x.QuestionAnswers).ToArray(),
                                                                    SubmittedQuestionAnswerIds= subMittedQuizInfo.Where(x => x.SubjectName == g.Key.SubjectName && x.SetNumber == g.Key.SetNumber).SelectMany(x=>x.quizAnswers).Where(x=>x.QuestionId==M.Key.QuestionId && x.QuestionType==M.Key.QuestionType).SelectMany(x=>x.QuestionAnswerIds).ToArray(),
                                                                    SubmittedQuestionAnswers = subMittedQuizInfo.Where(x => x.SubjectName == g.Key.SubjectName && x.SetNumber == g.Key.SetNumber).SelectMany(x => x.quizAnswers).Where(x => x.QuestionId == M.Key.QuestionId && x.QuestionType == M.Key.QuestionType).SelectMany(x => x.QuestionAnswers).ToArray(),
                                                                    IsCorrect= checkEquality(masterQuizzes.Where(x => x.SubjectName == g.Key.SubjectName && x.SetNumber == g.Key.SetNumber && x.QuestionId == M.Key.QuestionId && x.QuestionType == M.Key.QuestionType).SelectMany(x => x.QuestionAnswersIds).ToArray(),subMittedQuizInfo.Where(x => x.SubjectName == g.Key.SubjectName && x.SetNumber == g.Key.SetNumber).SelectMany(x => x.quizAnswers).Where(x => x.QuestionId == M.Key.QuestionId && x.QuestionType == M.Key.QuestionType).SelectMany(x => x.QuestionAnswerIds).ToArray())
                                                                }).ToArray()
                                             }).ToList();
                            submittedAnswers = (from x in clubedAnswers
                                                group x by new
                                                {
                                                    x.SetNumber,
                                                    x.SubjectName,
                                                    x.QuizAnswersDetails
                                                } into g
                                                select new QuizAnswersDetailedInfo
                                                {
                                                    SetNumber = g.Key.SetNumber,
                                                    SubjectName = g.Key.SubjectName,
                                                    QuizAnswersDetails = g.Key.QuizAnswersDetails,
                                                    TotalQuestions= g.Key.QuizAnswersDetails.Count(),
                                                    TotalAnsweredQuestions = g.Key.QuizAnswersDetails.Where(x=>x?.SubmittedQuestionAnswers.Count() != 0 && x?.SubmittedQuestionAnswerIds.Count() != 0).Count(),
                                                    TotalUnAnsweredQuestions = g.Key.QuizAnswersDetails.Where(x => x?.SubmittedQuestionAnswers.Count()==0 && x?.SubmittedQuestionAnswerIds.Count()==0).Count(),
                                                    TotalCorrectAnswers= g.Key.QuizAnswersDetails.Where(x=>x.IsCorrect).Count(),
                                                    TotalInCorrectAnswers = g.Key.QuizAnswersDetails.Where(x => x.IsCorrect=false).Count(),
                                                }).ToList();
                        }
                        QuizSubmission quiz = new QuizSubmission()
                        {
                            QuizId = quizDetails.QuizId,
                            CandidateId = quizDetails.CandidateId,
                            IsActive = true,
                            QuizSetList = quizDetails.QuizSetList,
                            TotalQuestions= quizSumissionRequest.TotalQuestions,
                            TotalUnAnsweredQuestions= quizSumissionRequest.NotAnsweredQuestions,
                            SubmittedAnswersInfo = submittedAnswers,
                            TotalCorrectAnswers= submittedAnswers.GroupBy(x=>new {x.TotalCorrectAnswers }).Sum(x=>x.Key.TotalCorrectAnswers),
                            TotalInCorrectAnswers = submittedAnswers.GroupBy(x => new { x.TotalInCorrectAnswers }).Sum(x => x.Key.TotalInCorrectAnswers),
                            UpdatedDate = DateTime.Now,
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

        #region Get Submitted Quiz Info
        [HttpGet("quiz/interviewer/submitquiz/{quizId:int}")]
        //[Route("Org/{id:guid}")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(200, Type = typeof(SubmittedAnswersResponse[]))]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> GetSubmittedQuizAnswersInfo(int quizId)
        {
            List<SubmittedAnswersResponse> submittedQuizAnswerResponses = new List<SubmittedAnswersResponse>();
            try
            {
                if (quizId > 0)
                {
                    var submittedQuizDetails = await _quizSubmissionService.GetQuizDetails(quizId).ConfigureAwait(false);
                    List<SubjectExpertQuestions> masterQuizzes = new List<SubjectExpertQuestions>();
                    if (submittedQuizDetails != null)
                    {
                        foreach (var item in submittedQuizDetails.QuizSetList)
                        {
                            List<SubjectExpertQuestions> quizzes = new List<SubjectExpertQuestions>();
                            quizzes = (List<SubjectExpertQuestions>)await _masterQuizService.GetMasterQuestions(item.SetNumber, item.SubjectName).ConfigureAwait(false);
                            if (quizzes?.Count > 0)
                            {
                                masterQuizzes.AddRange(quizzes);
                            }
                        }
                    }
                    if (submittedQuizDetails!=null && masterQuizzes?.Count>0)
                    {
                        submittedQuizAnswerResponses = (from x in masterQuizzes
                                                  group x by new
                                                  {
                                                      x.SetNumber,
                                                      x.SubjectName,
                                                      x.QuestionId,
                                                      x.Question,
                                                      x.QuestionOptions,
                                                      x.QuestionType
                                                  } into g
                                                  select new SubmittedAnswersResponse
                                                  {
                                                      SetNumber=g.Key.SetNumber,
                                                      SubjectName = g.Key.SubjectName,
                                                      QuestionId = g.Key.QuestionId,
                                                      Question = g.Key.Question,
                                                      QuestionOptions = g.Key.QuestionOptions,
                                                      QuestionType = g.Key.QuestionType,
                                                      SubmittedAnswers= submittedQuizDetails.SubmittedAnswersInfo.Where(x=>x.SubjectName== g.Key.SubjectName && x.SetNumber== g.Key.SetNumber).SelectMany(x=>x.QuizAnswersDetails)
                                                                       .Where(x=>x.QuestionId==g.Key.QuestionId && x.QuestionType==g.Key.QuestionType).SelectMany(x=>x.SubmittedQuestionAnswers).ToArray(),
                                                      SubmittedAnswersIds = submittedQuizDetails.SubmittedAnswersInfo.Where(x => x.SubjectName == g.Key.SubjectName && x.SetNumber == g.Key.SetNumber).SelectMany(x => x.QuizAnswersDetails)
                                                                       .Where(x => x.QuestionId == g.Key.QuestionId && x.QuestionType == g.Key.QuestionType).SelectMany(x => x.SubmittedQuestionAnswerIds).ToArray(),
                                                      MasterQuestionAnswers = submittedQuizDetails.SubmittedAnswersInfo.Where(x => x.SubjectName == g.Key.SubjectName && x.SetNumber == g.Key.SetNumber).SelectMany(x => x.QuizAnswersDetails)
                                                                       .Where(x => x.QuestionId == g.Key.QuestionId && x.QuestionType == g.Key.QuestionType).SelectMany(x => x.MasterQuestionAnswers).ToArray(),
                                                      MasterQuestionAnswersIds = submittedQuizDetails.SubmittedAnswersInfo.Where(x => x.SubjectName == g.Key.SubjectName && x.SetNumber == g.Key.SetNumber).SelectMany(x => x.QuizAnswersDetails)
                                                                       .Where(x => x.QuestionId == g.Key.QuestionId && x.QuestionType == g.Key.QuestionType).SelectMany(x => x.MasterQuestionAnswerIds).ToArray()


                                                  }).ToList();
                        if (submittedQuizAnswerResponses.Count > 0)
                        {
                            return Ok(submittedQuizAnswerResponses);
                        }
                    }
                    return NoContent();
                }
                return BadRequest("Please provide Valid Quiz Id");
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }
        #endregion

        #region Get Submitted Quiz Info
        [HttpGet("quiz/interviewer/submitquiz")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(200,Type =typeof(SubmittedQuizResponse[]))]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> GetSubmittedQuiz()
        {
            List<SubmittedQuizResponse> submittedQuizResponses = new List<SubmittedQuizResponse>();
            try
            {
                    var totalSubmittedQuizDetails = await _quizSubmissionService.GetTotalQuizDetails().ConfigureAwait(false);
                    var totalCandidates = await _candidateService.GetTotalCandidateDetails().ConfigureAwait(false);
                    var totalQuizDetails = await _quizService.GetTotalQuizDetails().ConfigureAwait(false);
                    if (totalQuizDetails?.Count()>0 && totalCandidates?.Count()>0)
                    {
                      submittedQuizResponses = (from x in totalSubmittedQuizDetails
                                              group x by new
                                              {
                                                  x.QuizId,
                                                  x.CandidateId,
                                                  x.QuizSetList,
                                                  x.SubmittedAnswersInfo
                                              } into g
                                              select new SubmittedQuizResponse
                                              {
                                                  QuizId = g.Key.QuizId,
                                                  CandidateId = g.Key.CandidateId,
                                                  QuizSetList = g.Key.QuizSetList,
                                                  CreatedDate = (totalQuizDetails.Where(x => x.QuizId == g.Key.QuizId).Select(x => x.CreatedDate).SingleOrDefault())?.ToString("dd MMMM yyyy hh:mm tt"),
                                                  InterviewLevel = totalCandidates.Where(x => x.CandidateId == g.Key.CandidateId).Select(x => x.InterviewLevel).SingleOrDefault(),
                                                  //TotalQuestions= g.Key.SubmittedAnswersInfo.SelectMany(x=>x.)
                                                  //CreatedBy= totalQuizDetails.Where(x => x.QuizId == g.Key.QuizId).Select(x => x.CreatedBy).SingleOrDefault(),
                                              }).ToList();
                        if (submittedQuizResponses.Count>0)
                        {
                           return Ok(submittedQuizResponses);
                        }
                    }
                    return NoContent();
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

        private static bool checkEquality<T>(T[] first, T[] second)
        {
            if (ReferenceEquals(first, second))
            {
                return true;
            }

            if (first == null || second == null || (first.Length != second.Length))
            {
                return false;
            }

            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < first.Length; i++)
            {
                if (!comparer.Equals(first[i], second[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
