using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlMap
{
	public class HtmlElement
	{
		public string Id { get; private set; }
		public IDictionary<string, string> Attributes { get; private set; }
		public HtmlElement Parent { get; internal set; }
		public IEnumerable<HtmlElement> Children { get; internal set; }
		public string Name { get; set; }
		public HtmlElementType Type { get; set; }
	}
	public enum HtmlElementType
	{
		Normal,
		Text
	}
}
