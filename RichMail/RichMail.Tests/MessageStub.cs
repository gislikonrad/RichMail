//using RichMail.Interfaces;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace RichMail.Tests
//{
//	public class MessageStub : IRichMailMessage
//	{
//		private readonly string _boundary;
//		private readonly string _id;
//		public MessageStub()
//		{
//			_boundary = Guid.NewGuid().ToString();
//			_id = Guid.NewGuid().ToString();
//		}

//		public string To
//		{
//			get { return "gisli.konrad@gmail.com"; }
//		}

//		public string From
//		{
//			get { return "gisli.k.bjornsson@landsbankinn.is"; }
//		}

//		public string Subject
//		{
//			get { return "Message stub"; }
//		}

//		public string Id
//		{
//			get { return _id; }
//		}

//		public string ContentType
//		{
//			get { return "multipart/related"; }
//		}
//	}
//}
