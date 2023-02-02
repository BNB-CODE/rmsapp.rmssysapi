using rmsapp.rmssysapi.service.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace rmsapp.rmssysapi.service
{
    public interface IExcelDataConversionService
    {
        Task<IEnumerable<QuizDetails>> GetMasterQuizData(IFormFile formFile, CancellationToken cancellationToken);
    }
}
