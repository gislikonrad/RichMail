using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RichMail.Utils
{
	internal interface ITag
	{
		int Position { get; } 
		int Length { get; }
		string Text { get; }
	}
}
