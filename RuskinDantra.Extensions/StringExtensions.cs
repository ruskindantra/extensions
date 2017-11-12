using System;
using System.Security;

namespace RuskinDantra.Extensions
{
	public static class StringExtensions
	{
		public static string Flatten(this string str)
		{
			return str.Replace(Environment.NewLine, "").Replace('\t', '\0');
		}

	    public static SecureString ToSecureString(this string str)
	    {
	        SecureString secureString = new SecureString();
	        foreach (var c in str)
	        {
	            secureString.AppendChar(c);
	        }
	        return secureString;
	    }
	}
}