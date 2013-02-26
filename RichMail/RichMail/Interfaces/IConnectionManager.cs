using RichMail.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RichMail.Interfaces
{
	internal interface IConnectionManager : IDisposable
	{
		ConnectionState State { get; }

		Task<SmtpResponse> Connect(string host, int port);
		Task<SmtpResponse> ExecuteCommand(string command);
	}
}
