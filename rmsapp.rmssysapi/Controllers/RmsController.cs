using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public RmsController(IMasterQuizService masterQuizService, IExcelDataConversionService excelDataConversionService,
            ITemplateDownloadService templateDownloadService, IQuizService quizService,
            IQuizSubmissionService quizSubmissionService)
        {
            _masterQuizService = masterQuizService;
            _excelDataConversionService = excelDataConversionService;
            _templateDownloadService = templateDownloadService;
            _quizService = quizService;
            _quizSubmissionService = quizSubmissionService;
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
        [ProducesResponseType(200, Type = typeof(CandidateQuestions))]
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
                        ConfirmationCode= confirmationCode,
                        ConfirmationCodeExpiration = confirmationCodeExpiration,
                        QuizSetList = interviewerQuizRequest
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

        #region Submitt Quiz
        [HttpPost("quiz/interviewer/submitquiz")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(200, Type = typeof(InterviewerQuizResponse))]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> SubmitQuiz(QuizSumissionRequest quizSumissionRequest)
        {
            InterviewerQuizResponse quizResponse = new InterviewerQuizResponse();

            try
            {
                if (quizSumissionRequest!=null)
                {
                    var quizDetails = await _quizService.GetQuizDetails(quizSumissionRequest.QuizId).ConfigureAwait(false);
                    if (quizDetails != null)
                    {
                        List<QuizInfo> subMittedQuizInfo = quizSumissionRequest.Data?.Count() > 0 ? quizSumissionRequest.Data : new List<QuizInfo>();
                        List<MasterQuiz> masterQuizzes = new List<MasterQuiz>();
                        List<QuizInfo> masterQuizInfo = new List<QuizInfo>();
                        foreach (var item in quizDetails.QuizSetList)
                        {
                            List<MasterQuiz> quizzes = new List<MasterQuiz>();
                            quizzes =(List<MasterQuiz>) await _masterQuizService.GetMasterQuestions(item.SetNumber, item.SubjectName).ConfigureAwait(false);
                            if (quizzes?.Count>0)
                            {
                                masterQuizzes.AddRange(masterQuizzes);
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
                            CandidateMailId =quizDetails.Email,
                            IsActive = true,
                            QuizSetList=quizDetails.QuizSetList,
                            SubmittedAnswersInfo= subMittedQuizInfo,
                            MasterAnswersInfo= masterQuizInfo
                            //CreatedBy= Currentuser
                        };
                        var res = await _quizSubmissionService.Add(quiz).ConfigureAwait(false);
                        if (res)
                        {
                            return Ok(quizResponse);
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
