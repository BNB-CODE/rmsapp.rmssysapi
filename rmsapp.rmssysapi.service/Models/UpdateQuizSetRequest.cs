using System;
using System.Collections.Generic;
using System.Text;

namespace rmsapp.rmssysapi.service.Models
{
    public class UpdateQuizSetRequest
    {
       public string Version { get; set; }
       public string SubjectName { get; set; }
       public string Tag { get; set; }
       public UpdateQuizDetails[] updateQuizDetails { get; set; }
    }
}
