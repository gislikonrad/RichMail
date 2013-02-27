using RichMail.Interfaces;
using RichMail.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RichMail.Extensions
{
	public static class SimpleSmtpClientExtensions
	{
		public static async Task<SmtpResponse> SendRichMailAsync(this SimpleSmtpClient client, IRichMailMessage message)
		{	
			//await SetMailFrom(message.From);
			//await SetRcptTo(message.To.First());
			await client._connectionManager.ExecuteCommandAsync("mail from: ");
			await client._connectionManager.ExecuteCommandAsync("rcpt to: ");
			await client._connectionManager.ExecuteCommandAsync("data");
			var builder = new StringBuilder();
			builder.AppendLine("Message-ID: <12345@landsbankinn.is>");

			builder.AppendLine("To: ");
			builder.AppendLine("From: ");
			builder.AppendLine("Subject: Raw email");
			builder.AppendLine("This is a raw email message");
			builder.AppendLine();
			builder.AppendLine(".");
			var data = builder.ToString();
			return await client._connectionManager.ExecuteCommandAsync(data);
		}
		
		//public static async Task<SmtpResponse> SendRichMailAsync(this SimpleSmtpClient client, string to, string from, string subject, string html)
		//{
			
		//}
	}
}
