using RichMail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RichMail.Models
{
	public class ViewCollection : IEnumerable<IRichMailMessageView>, IHaveABoundary, IHaveAContentType
	{
		private IEnumerable<IRichMailMessageView> _views;
		public ViewCollection(IEnumerable<IRichMailMessageView> views)
		{
			_views = views;
		}

		public ContentType ContentType
		{
			get { throw new NotImplementedException(); }
		}

		public IEnumerator<IRichMailMessageView> GetEnumerator()
		{
			throw new NotImplementedException();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}
}
