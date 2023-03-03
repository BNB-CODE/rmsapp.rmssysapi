using System;
using System.Collections.Generic;
using System.Text;

namespace rmsapp.rmssysapi.service.Models
{
    public class BaseModel
    {
        public string CreatedBy { get; set; } = "TestUser@gmail.com";
        public string UpdatedBy { get; set; } = "DemoUser@gmail.com";
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
