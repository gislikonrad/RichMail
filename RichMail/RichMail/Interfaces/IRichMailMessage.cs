using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RichMail.Interfaces
{
	public interface IRichMailMessage
	{
		string To { get; }
		string From { get; }
		string Subject { get; }
		string Boundary { get; }
		string ContentType { get; }
		IDictionary<string, string> Headers { get; }

		IEnumerable<IRichMailMessageView> Views { get; }
		IEnumerable<IRichMailAttachment> Attachments { get; }
	}
}
