using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Mail;
using System.Threading.Tasks;

namespace RichMail.Tests
{
	/// <summary>
	/// Summary description for UnitTest1
	/// </summary>
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void TestMethod1()
		{
		}
	}

	public static class TaskExtensions
	{
		public static TResult WaitForResult<TResult>(this Task<TResult> task)
		{
			task.Wait();
			return task.Result;
		}
	}
}
