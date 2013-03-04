using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HtmlMap.Tests
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void TestMethod1()
		{
			var html = @"<html>
	<head>
		<title>Some title</title>
	</head>
	<body>
		<div>
			<p>
				Some content
			</p>
		</div>
	</body>
</html>";

			var map = HDocument.Parse(html);
			Assert.AreEqual("HTML", map.Root.Name);
			Assert.AreEqual(HtmlElementType.Normal, map.Root.Type);
		}
	}
}
