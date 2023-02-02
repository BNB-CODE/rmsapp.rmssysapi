using System;
using System.Collections.Generic;
using System.Text;

namespace rmsapp.rmssysapi.service.Utils
{
    public class Header
    {
        public Dictionary<string, int> NameToColumnNumberDict { get; set; }
        public int Question
        {
            get
            {
                int index = -1;
                NameToColumnNumberDict.TryGetValue("question", out index);
                return index;
            }
        }
        public int QuestionType
        {
            get
            {
                int index = -1;
                NameToColumnNumberDict.TryGetValue("questiontype", out index);
                return index;
            }
        }
        public int QuestionOptions
        {
            get
            {
                int index = -1;
                NameToColumnNumberDict.TryGetValue("questionoptions", out index);
                return index;
            }
        }
        public int QuestionAnswers
        {
            get
            {
                int index = -1;
                NameToColumnNumberDict.TryGetValue("questionanswers", out index);
                return index;
            }
        }
        //public int QuestionAnswerIds
        //{

        //    get
        //    {
        //        int index = -1;
        //        NameToColumnNumberDict.TryGetValue("questionanswerids", out index);
        //        return index;
        //    }
        //}
    }
}
