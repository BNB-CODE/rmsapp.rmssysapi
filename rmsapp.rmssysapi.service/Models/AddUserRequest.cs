using System;
using System.Collections.Generic;
using System.Text;

namespace rmsapp.rmssysapi.service.Models
{
    public class AddUserRequest
    {
        public string CandidateName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
