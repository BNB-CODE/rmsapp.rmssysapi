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
        public string CandidateMailId { get; set; }
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
        public List<QuizInfo> SubmittedAnswersInfo
        {
            get
            {
                return JsonConvert.DeserializeObject<List<QuizInfo>>(SubmittedAnswers);
            }
            set
            {
                SubmittedAnswers = JsonConvert.SerializeObject(value);
            }
        }
        [Column(TypeName = "jsonb")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string MasterAnswers { get; set; }
        [NotMapped]
        public List<QuizInfo> MasterAnswersInfo
        {
            get
            {
                return JsonConvert.DeserializeObject<List<QuizInfo>>(MasterAnswers);
            }
            set
            {
                MasterAnswers = JsonConvert.SerializeObject(value);
            }
        }
        public DateTime? UpdatedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
