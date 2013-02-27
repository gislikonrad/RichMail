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
    public class SimpleSmtpClient : IDisposable
    {
		private const string MAIL_FROM_FORMAT = "mail from: {0}";
		private const string RCPT_TO_FORMAT = "rcpt to: {0}";

		public const int SMTP_DEFAULT_PORT = 25;
		public const int SSMTP_DEFAULT_PORT = 465;

		private string _host;

		internal IConnectionManager _connectionManager;

		public SimpleSmtpClient(string host)
			: this(host, SMTP_DEFAULT_PORT)
		{

		}

		public SimpleSmtpClient(string host, bool useSsl)
			: this(host, useSsl ? SSMTP_DEFAULT_PORT : SMTP_DEFAULT_PORT, useSsl)
		{
		}

		public SimpleSmtpClient(string host, int port)
			: this(host, port, false)
		{
		}

		public SimpleSmtpClient(string host, int port, bool useSsl)
		{
			this._connectionManager = new TcpClientConnectionManager();
			this._connectionManager.ConnectAsync(host, port, useSsl).Wait();
		}

		//private async Task LoginAsync(string username, string password)
		//{
		//	var builder = new StringBuilder();

		//	var u = Base64.ToBase64(username);
		//	var p = Base64.ToBase64(password);
		//}

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
			return await this._connectionManager.ExecuteCommandAsync(string.Format("{0} {1}", command, entry.HostName));
		}

		public async Task<SmtpResponse> MailFromAsync(string from)
		{
			return await this._connectionManager.ExecuteCommandAsync(string.Format(MAIL_FROM_FORMAT, from));
		}

		public async Task<SmtpResponse> RcptToAsync(string to)
		{
			return await this._connectionManager.ExecuteCommandAsync(string.Format(RCPT_TO_FORMAT, to));
		}

		public async Task<SmtpResponse> DataAsync(string data)
		{
			var end = "\r\n.\r\n";
			await this._connectionManager.ExecuteCommandAsync("DATA");
			return await this._connectionManager.ExecuteCommandAsync(data + end);
		}

		public void Dispose()
		{
			var task = this._connectionManager.ExecuteCommandAsync("QUIT");
			task.Wait();
			this._connectionManager.Dispose();
		}
	}
}
