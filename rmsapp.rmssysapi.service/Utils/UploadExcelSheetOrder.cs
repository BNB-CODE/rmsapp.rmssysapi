

using System;
using System.Collections.Generic;
using System.Text;

namespace rmsapp.rmssysapi.service.Utils
{
    public static class UploadExcelSheetOrder
    {
        public static readonly string Quiz = "Quiz";

        public static List<string> AlphabetName(List<int> Indexes)
        {
            List<string> alphabetsList = new List<string>();
            foreach (int number in Indexes)
            {
                int CapitalAlphabetsStartingLetter = 65 + number;
                string strAlpha = Encoding.ASCII.GetString(new byte[] { Convert.ToByte(CapitalAlphabetsStartingLetter) });
                alphabetsList.Add(strAlpha);
            }
            return alphabetsList;
        }
    }
}
