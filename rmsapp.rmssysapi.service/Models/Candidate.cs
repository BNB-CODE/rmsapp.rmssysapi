using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace rmsapp.rmssysapi.service.Models
{
    public class Candidate: BaseModel
    {
        [Key]
        public string CandidateId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Skills { get; set; }
        public double TotalExperience { get; set; }
        public string SubjectName { get; set; }
    }
}
