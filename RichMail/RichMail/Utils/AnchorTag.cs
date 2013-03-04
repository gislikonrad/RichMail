using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RichMail.Utils
{
	internal class AnchorTag : ITag
	{
		internal string Href { get; private set; }
		public int Position { get; private set; }
		public int Length { get; private set; }
		public string Text { get; private set; }

		internal static IEnumerable<AnchorTag> Get(string html)
		{
			var anchorRegex = new Regex("<a.*?href=\"(.*?)\".*?>(.*?)</a>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
			foreach (var tagMatch in anchorRegex.Matches(html).Cast<Match>())
			{
				yield return new AnchorTag
				{
					Href = tagMatch.Groups[1].Value,
					Text = tagMatch.Groups[2].Value.Trim(),
					Position = tagMatch.Index,
					Length = tagMatch.Length
				};
			}
		}
	}
}
