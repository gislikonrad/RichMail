using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Threading.Tasks;
using System.Text;

namespace RichMail.Tests
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void TestMethod1()
		{
			SendAsync().Wait();
		}

		private async Task SendAsync()
		{
			using (var client = new SimpleSmtpClient(""))
			{
				await client.EhloAsync();
				await client.MailFromAsync("");
				await client.RcptToAsync("");
				var builder = new StringBuilder();
				builder.AppendLine("From: ");
				builder.AppendLine("To: ");
				builder.AppendLine("Subject: This is a raw email");
				builder.AppendLine("This is the body.");
				await client.DataAsync(builder.ToString());
			}
		}
	}
}
