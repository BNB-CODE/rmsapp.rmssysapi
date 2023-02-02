using System.IO;
using System.Reflection;

namespace rmsapp.rmssysapi.service.Impl
{
    public class TemplateDownloadService: ITemplateDownloadService
    {
        public Stream DownloadQuizTemplate()
        {
            var resourceName = @"rmsapp.rmssysapi.service.Resources.UploadTemplates.RMSUploadTemplate-Quiz.xlsx";
            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream(resourceName);
            return stream;
        }
    }
}
