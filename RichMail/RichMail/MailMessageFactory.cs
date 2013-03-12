using RichMail.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RichMail
{
	public static class MailMessageFactory
	{
		public static Task<MailMessage> CreateMessage(string to, string from, string subject, Uri url)
		{
			return CreateMessage(new[] { to }, from, subject, url);
		}
		public static async Task<MailMessage> CreateMessage(string[] to, string from, string subject, Uri url)
		{
			var html = await WebContent.GetHtmlAsync(url);
			return await CreateMessage(to, from, subject, html);
		}
		public static Task<MailMessage> CreateMessage(string to, string from, string subject, string html)
		{
			return CreateMessage(new[] { to }, from, subject, html);
		}

		public static Task<MailMessage> CreateMessage(string to, string from, Uri url)
		{
			return CreateMessage(new[] { to }, from, url);
		}

		public static async Task<MailMessage> CreateMessage(string[] to, string from, Uri url)
		{
			var html = await WebContent.GetHtmlAsync(url);
			return await CreateMessage(to, from, html);
		}

		public static Task<MailMessage> CreateMessage(string[] to, string from, string html)
		{
			var subject = GetTitleFromHtml(html);
			return CreateMessage(to, from, subject, html);
		}

		public static Task<MailMessage> CreateMessage(string[] to, string from, string subject, string html)
		{
			return CreateMessage(to, new MailAddress(from), subject, html);
		}

		public static Task<MailMessage> CreateMessage(string to, MailAddress from, string subject, Uri url)
		{
			return CreateMessage(new[] { to }, from, subject, url);
		}
		public static async Task<MailMessage> CreateMessage(string[] to, MailAddress from, string subject, Uri url)
		{
			var html = await WebContent.GetHtmlAsync(url);
			return await CreateMessage(to, from, subject, html);
		}
		public static Task<MailMessage> CreateMessage(string to, MailAddress from, string subject, string html)
		{
			return CreateMessage(new[] { to }, from, subject, html);
		}

		public static async Task<MailMessage> CreateMessage(string to, MailAddress from, Uri url)
		{
			var recipients = new[] { to };
			return await CreateMessage(recipients, from, url);
		}

		public static async Task<MailMessage> CreateMessage(string[] to, MailAddress from, Uri url)
		{
			var html = await WebContent.GetHtmlAsync(url);
			return await CreateMessage(to, from, html);
		}

		public static Task<MailMessage> CreateMessage(string[] to, MailAddress from, string html)
		{
			var subject = GetTitleFromHtml(html);
			return CreateMessage(to, from, subject, html);
		}

		public static async Task<MailMessage> CreateMessage(string[] to, MailAddress from, string subject, string html)
		{
			var message = new MailMessage();

			message.To.Add(string.Join(",", to));
			message.From = from;
			message.Subject = subject;
			message.Headers.Add("Message-ID", Generator.GenerateMessageId(from.Address.Split(new[] { '@' }, StringSplitOptions.RemoveEmptyEntries).Last()));

			var viewFactory = new ViewFactory(html);
			message.AlternateViews.Add(viewFactory.CreatePlainTextAlternateView());
			message.AlternateViews.Add(await viewFactory.CreateHtmlAlternateView());

			return message;
		}

		private static string GetTitleFromHtml(string html)
		{
			var regex = new Regex(@"<title>(.*?)</title>", RegexOptions.Singleline);
			var match = regex.Match(html);
			if (!match.Success) return string.Empty;
			return match.Groups[1].Value.Trim();
		}
	}
}
