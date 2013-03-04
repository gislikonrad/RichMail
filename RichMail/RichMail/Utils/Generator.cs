using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RichMail.Utils
{
	internal static class Generator
	{

		internal static string GenerateContentId()
		{
			return Guid.NewGuid().ToString();

		}
		internal static string GenerateContentId(string host)
		{
			return string.Format("{0}@{1}", GenerateContentId(), host);
		}
		

		internal static string GenerateMessageId(string host)
		{
			return string.Format("<{0}>", GenerateContentId(host));
		}
	}
}
