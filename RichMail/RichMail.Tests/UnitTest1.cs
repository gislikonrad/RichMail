using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

namespace RichMail.Tests
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void TestMethod1()
		{
			using (var client = new SmtpClient(""))
			{
				var task = client.HeloAsync();
				task.Wait();
				var response = task.Result;
				var t = client.SendAsync(null);
				t.Wait();
			}
		}
	}
}
