using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RichMail.Utils
{
	internal class ViewFactory
	{
		private string _html;
		private IEnumerable<ITag> _tags;

		internal ViewFactory(string html)
		{
			_tags = ImageTag.Get(html).Union<ITag>(AnchorTag.Get(html)).OrderByDescending(t => t.Position);
			_html = html;
		}

		public AlternateView CreatePlainTextAlternateView()
		{
			return AlternateView.CreateAlternateViewFromString(CreatePlainTextView(), null, "text/plain");
		}

		public AlternateView CreateHtmlAlternateView()
		{
			return AlternateView.CreateAlternateViewFromString(CreateHtmlView(), null, "text/html");
		}

		public Task<IEnumerable<Attachment>> GetInlineImageAttachments()
		{
			return AttachmentFactory.CreateInlineImageAttachmentAsync(_tags.OfType<ImageTag>());
		}

		public string CreateHtmlView()
		{
			return GetHtmlWithContentIds(_html);
		}

		public string CreatePlainTextView()
		{
			return ConvertToPlainText(_html);
		}

		private string ConvertToPlainText(string html)
		{
			var copy = (string)html.Clone();
			foreach (var tag in _tags)
			{
				var type = tag.GetType();
				if (typeof(ImageTag).IsAssignableFrom(type))
					copy = ReplaceImageTag(copy, (ImageTag)tag);
				if (typeof(AnchorTag).IsAssignableFrom(type))
					copy = ReplaceAnchorTag(copy, (AnchorTag)tag);
			}

			copy = ConvertToSingleLine(copy);
			copy = RemoveTagAndContents(copy, "script", "style", "title");

			copy = ReplaceTags(copy, Environment.NewLine, "div", "table", "img", "tr", "li", "th", "p");
			copy = NormalizeLineBreaks(copy);
			copy = ReplaceTags(copy, Environment.NewLine + Environment.NewLine, "br");
			copy = RemoveTags(copy);
			copy = Sanitize(copy);

			return copy;
		}

		private string Sanitize(string copy)
		{
			var regex = new Regex(@"^[\s-[\n\r]]+", RegexOptions.Multiline);
			return regex.Replace(copy, string.Empty);
		}

		private string NormalizeLineBreaks(string copy)
		{
			var regex = new Regex(string.Format(@"({0})+", Environment.NewLine));
			return regex.Replace(copy, Environment.NewLine);
		}

		private string ConvertToSingleLine(string copy)
		{
			var regex = new Regex(@"[\t\n\r]+", RegexOptions.Singleline);
			return regex.Replace(copy, string.Empty);
		}

		private string ReplaceTags(string html, string replaceEndTagWith, params string[] tagnames)
		{
			var startTags = new Regex(string.Join("|", tagnames.Select(s => string.Format(@"<{0}[^/]*?>", s))));
			var closingTags = new Regex(string.Join("|", tagnames.Select(s => string.Format(@"<{0}.*?/>|</{0}.*?>", s))));
			return startTags.Replace(closingTags.Replace(html, replaceEndTagWith), string.Empty);
		}

		private string RemoveTags(string html)
		{
			var regex = new Regex(@"</?.*?/?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
			return regex.Replace(html, string.Empty);	
		}

		private string RemoveTagAndContents(string html, params string[] tagnames)
		{
			var pattern = string.Join("|", tagnames.Select(s => string.Format(@"<{0}.*?>.*?</{0}>", s)));
			var regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
			return regex.Replace(html, string.Empty);
		}

		private string ReplaceAnchorTag(string html, AnchorTag anchorTag)
		{
			var text = string.Format("{0} [{1}] ", anchorTag.Text, anchorTag.Href);
			return html
				.Remove(anchorTag.Position, anchorTag.Length)
				.Insert(anchorTag.Position, text);
		}

		private string ReplaceImageTag(string html, ImageTag imageTag)
		{
			var text = !string.IsNullOrWhiteSpace(imageTag.Text) ? imageTag.Text : "Inline image";
			return html
				.Remove(imageTag.Position, imageTag.Length)
				.Insert(imageTag.Position, string.Format("[image: {0}] ", text) + Environment.NewLine);
		}

		private string GetHtmlWithContentIds(string html)
		{
			var copy = (string)html.Clone();
			foreach (var imageTag in _tags.OfType<ImageTag>())
			{
				copy = copy
					.Remove(imageTag.SourcePosition, imageTag.SourceLength)
					.Insert(imageTag.SourcePosition, string.Format("cid:{0}", imageTag.ContentId));
			}
			return copy;
		}
	}
}
