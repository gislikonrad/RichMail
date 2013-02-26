using RichMail.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RichMail.Models
{
	public class RichMailMessage : IRichMailMessage
	{
		public string To
		{
			get { throw new NotImplementedException(); }
		}

		public string From
		{
			get { throw new NotImplementedException(); }
		}

		public string Subject
		{
			get { throw new NotImplementedException(); }
		}

		public string Id
		{
			get { throw new NotImplementedException(); }
		}
	}
}
