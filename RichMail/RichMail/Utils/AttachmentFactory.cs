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
		internal static async Task<IEnumerable<Attachment>> CreateInlineImageAttachmentAsync(IEnumerable<Image> images)
		{
			var list = new List<Attachment>();
			foreach (var imageTag in images.OrderBy(t => t.Position))
			{
				var content = await WebContent.GetWebContentAsync(new Uri(imageTag.Source));
				var attachment = new Attachment(content.ResponseStream, imageTag.Name);
				attachment.ContentId = imageTag.ContentId;
				attachment.ContentType.MediaType = content.ContentType;
				list.Add(attachment);
			}
			return list;
		}

		internal static async Task<IEnumerable<LinkedResource>> CreateLinkedResourcesAsync(IEnumerable<Image> images)
		{
			var list = new List<LinkedResource>();
			foreach (var imageTag in images.OrderBy(t => t.Position))
			{
				var content = await WebContent.GetWebContentAsync(new Uri(imageTag.Source));
				var resource = new LinkedResource(content.ResponseStream, content.ContentType);
				resource.ContentType.Name = imageTag.Name;
				resource.ContentId = imageTag.ContentId;
				resource.ContentLink = new Uri(imageTag.ContentLink);
				list.Add(resource);
			}
			return list;
		}
	}
}
