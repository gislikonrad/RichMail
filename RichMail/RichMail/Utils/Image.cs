using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RichMail.Utils
{
	internal class Image : ITag
	{
		internal string ContentId { get; private set; }
		internal string ContentLink { get { return string.Format("cid:{0}", ContentId); } }
		internal int SourcePosition { get; private set; }
		internal int SourceLength { get; private set; }
		internal string Source { get; private set; }
		public int Position { get; private set; }
		public int Length { get; private set; }
		public string Text { get; private set; }
		public string Name { get; set; }

		internal static IEnumerable<Image> Get(string html)
		{
			var imageRegex = new Regex(@"<img.*?/?>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
			var attributeRegex = new Regex("(src|alt)=\"(.*?)\"", RegexOptions.Singleline | RegexOptions.IgnoreCase);
			foreach (var tagMatch in imageRegex.Matches(html).Cast<Match>())
			{
				var tag = new Image
				{
					Length = tagMatch.Length,
					Position = tagMatch.Index
				};
				foreach (var attributeMatch in attributeRegex.Matches(tagMatch.Value).Cast<Match>())
				{
					var key = attributeMatch.Groups[1];
					var value = attributeMatch.Groups[2];
					switch (key.Value.ToLower())
					{
						case "src":
							tag.Source = value.Value;
							tag.SourceLength = value.Length;
							tag.SourcePosition = tagMatch.Index + value.Index;
							tag.Name = value.Value.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries).Last();
							break;
						case "alt":
							tag.Text = value.Value;
							break;
					}
				}
				tag.ContentId = tag.Name;
				yield return tag;
			}
		}
	}
}
