using RichMail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RichMail.Utilities
{
	internal static class EmailDataGenerator
	{
		private const string START_BOUNDARY_FORMAT = "--{0}";
		private const string END_BOUNDARY_FORMAT = "--{0}--";

		internal static string GenerateData(IRichMailMessage message)
		{
			var globalBoundary = Guid.NewGuid().ToString();
			var builder = new StringBuilder();

			throw new NotImplementedException();
		}
	}
}
