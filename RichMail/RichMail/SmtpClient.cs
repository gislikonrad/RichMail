using RichMail.Implementations;
using RichMail.Interfaces;
using RichMail.Models;
using RichMail.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RichMail
{
    public class SmtpClient : IDisposable
    {
		private const string MAIL_FROM_FORMAT = "mail from: {0}";
		private const string RCPT_TO_FORMAT = "rcpt to: {0}";

		public const int SMTP_DEFAULT_PORT = 25;
		public const int SSMTP_DEFAULT_PORT = 465;

		private string _host;

		internal IConnectionManager _connectionManager;

		public SmtpClient(string host)
			: this(host, SMTP_DEFAULT_PORT)
		{

		}

		//public SmtpClient(string host, bool useSsl)
		//	: this(host, useSsl ? SSMTP_DEFAULT_PORT : SMTP_DEFAULT_PORT)
		//{
		//}

		public SmtpClient(string host, int port)
			: this(new TcpClientConnectionManager(), host, port)
		{
			this._host = host;
		}

		internal SmtpClient(IConnectionManager connectionManager, string host, int port)
		{
			this._connectionManager = connectionManager;
			this._connectionManager.Connect(host, port).Wait();
		}

		private async Task LoginAsync(string username, string password)
		{
			var builder = new StringBuilder();

			var u = Base64.ToBase64(username);
			var p = Base64.ToBase64(password);
		}

		public async Task<SmtpResponse> HeloAsync()
		{
			return await DnsCommandAsync("HELO");
		}

		public async Task<SmtpResponse> EhloAsync()
		{
			return await DnsCommandAsync("EHLO");
		}

		private async Task<SmtpResponse> DnsCommandAsync(string command)
		{
			var entry = Dns.GetHostEntry(Dns.GetHostName());
			return await this._connectionManager.ExecuteCommand(string.Format("{0} {1}", command, entry.HostName));
		}

		public async Task<SmtpResponse> SendAsync(IRichMailMessage message)
		{
			//await SetMailFrom(message.From);
			//await SetRcptTo(message.To.First());
			await this._connectionManager.ExecuteCommand("mail from: gisli.k.bjornsson@landsbankinn.is");
			await this._connectionManager.ExecuteCommand("rcpt to: gisli.konrad@gmail.com");
			await this._connectionManager.ExecuteCommand("data");
			var builder = new StringBuilder();
			builder.AppendLine("Message-ID: <12345@landsbankinn.is>");

			builder.AppendLine("To: gisli.konrad@landsbankinn.is");
			builder.AppendLine("From: no-reply@landsbankinn.is");
			builder.AppendLine("Subject: Raw email");
			builder.AppendLine("This is a raw email message");
			builder.AppendLine();
			builder.AppendLine(".");
			var data = builder.ToString();
			return await this._connectionManager.ExecuteCommand(data);
		}

		private async Task<SmtpResponse> SetMailFrom(string from)
		{
			return await this._connectionManager.ExecuteCommand(string.Format(MAIL_FROM_FORMAT, from));
		}

		private async Task<SmtpResponse> SetRcptTo(string to)
		{
			return await this._connectionManager.ExecuteCommand(string.Format(RCPT_TO_FORMAT, to));
		}


		public void Dispose()
		{
		}
	}
}
