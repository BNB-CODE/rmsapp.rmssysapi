using System.Diagnostics.CodeAnalysis;

namespace workforceapp.workforcesysapi.Service.Utils
{
    [ExcludeFromCodeCoverage]
    public class UpperCaseFormat
    {
        string RawName;
        public UpperCaseFormat(string value)
        {
            this.value = value;
        }
        public string value
        {
            get
            {
                if (!string.IsNullOrEmpty(RawName))
                {
                    return RawName.Trim();
                }
                return RawName;
            }
            set
            {
                RawName = value;
            }
        }
        public static implicit operator string(UpperCaseFormat value) => value.value;
    }

    public partial class U
    {
        static public UpperCaseFormat Convert(string value)
        {
            return new UpperCaseFormat(value);
        }
    }
}
