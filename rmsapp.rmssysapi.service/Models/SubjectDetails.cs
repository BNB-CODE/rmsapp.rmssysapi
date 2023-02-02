using System;


namespace rmsapp.rmssysapi.service.Models
{
    public class SubjectDetails
    {
        public string SubjectName { get; set; }
        public int SetNumber { get; set; }
        public int TotalQuestionsCount { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
