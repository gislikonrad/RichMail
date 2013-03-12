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
			_tags = Image.Get(html).Union<ITag>(Anchor.Get(html)).OrderByDescending(t => t.Position);
			_html = html;
		}

		public AlternateView CreatePlainTextAlternateView()
		{
			return CreateAlternateView(CreatePlainTextView(), "text/plain", TransferEncoding.QuotedPrintable);
		}

		public async Task<AlternateView> CreateHtmlAlternateView()
		{
			var view = CreateAlternateView(CreateHtmlView(), "text/html", TransferEncoding.QuotedPrintable);
			foreach (var resource in await AttachmentFactory.CreateLinkedResourcesAsync(_tags.OfType<Image>()))
			{
				view.LinkedResources.Add(resource);
			}
			return view;
		}

		private AlternateView CreateAlternateView(string content, string contentType, TransferEncoding encoding = TransferEncoding.Base64)
		{
			var view = AlternateView.CreateAlternateViewFromString(content, null, contentType);
			view.TransferEncoding = encoding;
			return view;
		}

		public Task<IEnumerable<Attachment>> GetInlineImageAttachments()
		{
			return AttachmentFactory.CreateInlineImageAttachmentAsync(_tags.OfType<Image>());
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
				if (typeof(Image).IsAssignableFrom(type))
					copy = ReplaceImageTag(copy, (Image)tag);
				if (typeof(Anchor).IsAssignableFrom(type))
					copy = ReplaceAnchorTag(copy, (Anchor)tag);
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

		private string ReplaceAnchorTag(string html, Anchor anchorTag)
		{
			var text = string.Format("{0} [{1}] ", anchorTag.Text, anchorTag.Href);
			return html
				.Remove(anchorTag.Position, anchorTag.Length)
				.Insert(anchorTag.Position, text);
		}

		private string ReplaceImageTag(string html, Image imageTag)
		{
			var text = !string.IsNullOrWhiteSpace(imageTag.Text) ? imageTag.Text : "Inline image";
			return html
				.Remove(imageTag.Position, imageTag.Length)
				.Insert(imageTag.Position, string.Format("[image: {0}] ", text) + Environment.NewLine);
		}

		private string GetHtmlWithContentIds(string html)
		{
			var copy = (string)html.Clone();
			foreach (var imageTag in _tags.OfType<Image>())
			{
				copy = copy
					.Remove(imageTag.SourcePosition, imageTag.SourceLength)
					.Insert(imageTag.SourcePosition, imageTag.ContentLink);
			}
			return copy;
		}
	}
}
