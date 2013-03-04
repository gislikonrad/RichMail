using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace RichMail.Utils
{
	internal static class AttachmentFactory
	{
		internal static async Task<IEnumerable<Attachment>> CreateInlineImageAttachmentAsync(IEnumerable<ImageTag> imageTags)
		{
			var list = new List<Attachment>();
			foreach (var imageTag in imageTags.OrderBy(t => t.Position))
			{
				var content = await WebContent.GetWebContentAsync(new Uri(imageTag.Source));
				var attachment = new Attachment(content.ResponseStream, content.Name);
				attachment.ContentId = imageTag.ContentId;
				list.Add(attachment);
			}
			return list;
		}
	}
}
