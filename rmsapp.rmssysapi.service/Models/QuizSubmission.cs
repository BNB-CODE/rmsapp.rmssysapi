using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace rmsapp.rmssysapi.service.Models
{
    public class QuizSubmission
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
        [Column(TypeName = "jsonb")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SubmittedAnswers { get; set; }
        [NotMapped]
        public List<QuizAnswersDetailedInfo> SubmittedAnswersInfo
        {
            get
            {
                return JsonConvert.DeserializeObject<List<QuizAnswersDetailedInfo>>(SubmittedAnswers);
            }
            set
            {
                SubmittedAnswers = JsonConvert.SerializeObject(value);
            }
        }
        public int TotalQuestions { get; set; }
        public int TotalAnsweredQuestions { get; set; }
        public int TotalUnAnsweredQuestions { get; set; }
        public int TotalCorrectAnswers { get; set; }
        public int TotalInCorrectAnswers { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
