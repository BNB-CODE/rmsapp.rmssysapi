using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace rmsapp.rmssysapi.service.Models
{
    public class Quiz : BaseModel
    {
        [Key]
        public int QuizId { get; set; }
        public string CandidateId { get; set; }
        [Column(TypeName = "jsonb")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string QuizSets { get; set; }

        [NotMapped]
        public List<InterviewerQuizSet> QuizSetList
        {
            get
            {
                return JsonConvert.DeserializeObject<List<InterviewerQuizSet>>(QuizSets);
            }
            set
            {
                QuizSets = JsonConvert.SerializeObject(value);
            }
        }

        //[Column(TypeName = "jsonb")]
        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        //public ICollection<InterviewerQuizSet> QuizSets { get; set; }

        //[NotMapped]
        //public string[] QuizSetsSerialize { get; set; }
        /// <summary>
        /// Gets or sets the confirmation code for the Candidate.
        /// </summary>
        public string ConfirmationCode { get; set; }

        /// <summary>
        /// Gets or sets the time when the confirmation code will expire.
        /// </summary>
        public DateTime? ConfirmationCodeExpiration { get; set; }
        /// <summary>
        /// Gets or sets the date/time when the candidate Submitted  quiz.
        /// </summary>
        public DateTime? QuizSubmittedAt { get; set; }
        /// <summary>
        /// Gets or sets the amount of attempted failed logins.
        /// This is set to zero on successful login or lockout period is over.
        /// </summary>
        public int LoginAttempts { get; set; }
        /// <summary>
        /// Gets or sets the date/time when the user last logged in.
        /// </summary>
        public DateTime? LastLoggedIn { get; set; }

    }

}
