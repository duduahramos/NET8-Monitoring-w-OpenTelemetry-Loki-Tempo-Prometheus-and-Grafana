using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Core.Helpers
{
    public static class StringHelper
    {
        public static bool AreEquals(this string a, string b)
        {
            return a.Equals(b, StringComparison.OrdinalIgnoreCase);
        }
        
        public static bool AreEquals(string a, string b, StringComparison stringComparisonOption = StringComparison.OrdinalIgnoreCase)
        {
            return string.Equals(a, b, stringComparisonOption);
        }
    }
}
