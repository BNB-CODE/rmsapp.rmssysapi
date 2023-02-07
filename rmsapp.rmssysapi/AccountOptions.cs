using System;
using System.Diagnostics.CodeAnalysis;

namespace rmsapp.rmssysapi
{
    [ExcludeFromCodeCoverage]
    public static class AccountOptions
    {
        public static TimeSpan InvitationCodeDuration { get; set; }
    }
}
