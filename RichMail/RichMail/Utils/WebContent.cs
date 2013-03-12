using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RichMail.Utils
{
	internal class WebContent
	{
		internal Stream ResponseStream { get; private set; }
		internal string ContentType { get; private set; }

		internal static async Task<string> GetHtmlAsync(Uri url)
		{
			using (var reader = new StreamReader((await GetWebContentAsync(url)).ResponseStream))
				return await reader.ReadToEndAsync();
		}

		internal static async Task<WebContent> GetWebContentAsync(Uri url)
		{
			var request = WebRequest.Create(url);
			using (var response = await request.GetResponseAsync())
			using (var stream = response.GetResponseStream())
			{
				var copy = new MemoryStream();
				await stream.CopyToAsync(copy);
				copy.Position = 0;
				return new WebContent
				{
					ResponseStream = copy,
					ContentType = response.ContentType
				};
			}
		}
	}
}
