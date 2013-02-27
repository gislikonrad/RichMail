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

		Task<SmtpResponse> ConnectAsync(string host, int port, bool useSsl);
		Task<SmtpResponse> ExecuteCommandAsync(string command);
	}
}
