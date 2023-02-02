using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using rmsapp.rmssysapi.service;
using rmsapp.rmssysapi.service.Models;
using System;
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
        public RmsController(IMasterQuizService masterQuizService, IExcelDataConversionService excelDataConversionService,
            ITemplateDownloadService templateDownloadService)
        {
            _masterQuizService = masterQuizService;
            _excelDataConversionService = excelDataConversionService;
            _templateDownloadService = templateDownloadService;
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
    }
}
