using System.Security;

namespace RuskinDantra.Extensions
{
    public static class SecureStringExtensions
    {
        /// <summary>
        /// Mostly done for unit-tests, shouldn't be converting secure strings to non secure strings
        /// </summary>
        /// <param name="secureStr"></param>
        /// <returns></returns>
        public static string ToNonSecureString(this SecureString secureStr)
        {
            return new System.Net.NetworkCredential(string.Empty, secureStr).Password;
        }
    }
}