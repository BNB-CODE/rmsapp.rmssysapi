
using System.IO;

namespace rmsapp.rmssysapi.service
{
    public interface ITemplateDownloadService
    {
        Stream DownloadQuizTemplate();
    }
}
