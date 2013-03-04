using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HtmlMap
{
	public class HDocument
	{
		public HtmlElement Root { get; private set; }
		public string RawHtml { get; private set; }

		public static HDocument Parse(string html)
		{
			var document = new HDocument
			{
				RawHtml = html
			};
			var current = null as HtmlElement;
			html = RemoveNewLines(html);
			
			for(var i = 0; i < html.Length; i++)
			{
				var node = null as string;
				if (html[i] == '<')
				{
					node = ReadUntilNext(html, '>', ref i);
					var element = new HtmlElement
					{
						Type = HtmlElementType.Normal,
						Name = node.Replace("<", string.Empty).Replace(">", string.Empty).ToUpper()
					};
					if (document.Root == null)
						document.Root = element;					
				}
				else
				{
					node = ReadUntilNext(html, '<', ref i);
				}
			}

			return document;
		}

		private static string ReadUntilNext(string html, char next, ref int i)
		{
			var r = new List<char>();
			for (; i < html.Length; i++)
			{
				var c = html[i];
				r.Add(c);
				if (c == next) break;
			}
			return string.Join(string.Empty, r);
		}

		private static string RemoveNewLines(string html)
		{
			var regex = new Regex(string.Format(@"({0})+", Environment.NewLine), RegexOptions.Singleline);
			return regex.Replace(html, string.Empty);
		}
	}
}
