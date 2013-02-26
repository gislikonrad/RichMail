using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RichMail.Utilities
{
	internal static class Base64
	{
		internal static string ToBase64(string text)
		{
			return ToBase64(text, Encoding.ASCII);
		}

		internal static string ToBase64(string text, Encoding encoding)
		{
			var bytes = encoding.GetBytes(text);
			return Convert.ToBase64String(bytes, 0, bytes.Length);
		}

		internal static string FromBase64(string base64)
		{
			return FromBase64(base64, Encoding.ASCII);
		}

		internal static string FromBase64(string base64, Encoding encoding)
		{
			var bytes = Convert.FromBase64String(base64);
			return encoding.GetString(bytes);
		}
	}
}
