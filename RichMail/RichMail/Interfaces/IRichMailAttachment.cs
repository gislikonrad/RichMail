using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RichMail.Interfaces
{
	public interface IRichMailAttachment : IHaveAContentType, IHaveABoundary
	{
		bool IsInline { get; }
	}
}
