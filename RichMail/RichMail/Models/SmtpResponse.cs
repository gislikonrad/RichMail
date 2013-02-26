using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RichMail.Models
{
	public class SmtpResponse
	{
		private string _raw;
		internal SmtpResponse(string raw)
		{
			_raw = raw;
			ParseResponse();
		}

		private void ParseResponse()
		{
			var regex = new Regex(@"^(\d{3})(.*)$", RegexOptions.Singleline);
			var match = regex.Match(_raw);
			if (match == null || !match.Success) return;

			var codeNumber = 0;
			if (int.TryParse(match.Groups[1].Value, out codeNumber))
			{
				CodeNumber = codeNumber;
				Code = GetResponseCodeEnumeration(codeNumber);
			}
			Message = match.Groups[2].Value;
		}

		private SmtpResponseCode GetResponseCodeEnumeration(int codeNumber)
		{
			switch (codeNumber)
			{
				case 200: return SmtpResponseCode.NonStandardSuccess;
				case 211: return SmtpResponseCode.StatusOrHelpReply;
				case 214: return SmtpResponseCode.HelpMessage;
				case 220: return SmtpResponseCode.Ready;
				case 221: return SmtpResponseCode.ClosingTransmissionChannel;
				case 250: return SmtpResponseCode.MailActionCompleted;
				case 251: return SmtpResponseCode.UserNotLocalWillForward;

				case 354: return SmtpResponseCode.StartMailInput;
					
				case 421: return SmtpResponseCode.NotAvailable;
				case 450: return SmtpResponseCode.MailActionNotTakenMailboxUnavailable;
				case 451: return SmtpResponseCode.ActionAbortedLocalErrorInProcessing;
				case 452: return SmtpResponseCode.ActionNotTakenInsufficientSystemStorage;

				case 500: return SmtpResponseCode.SyntaxErrorInCommand;
				case 501: return SmtpResponseCode.SyntaxErrorInParametersOrArguments;
				case 502: return SmtpResponseCode.CommandNotImplemented;
				case 503: return SmtpResponseCode.BadSequenceOfCommands;
				case 504: return SmtpResponseCode.ParameterNotImplemented; 
				case 521: return SmtpResponseCode.DomainDoesNotAcceptMail;
				case 530: return SmtpResponseCode.AccessDenied;
				case 550: return SmtpResponseCode.ActionNotTakenMailboxUnavailable;
				case 551: return SmtpResponseCode.UserNotLocalTryForwarding;
				case 552: return SmtpResponseCode.MailActionAbortedExceededStorageAllocation;
				case 553: return SmtpResponseCode.ActionNotTakenMailboxNameNotAllowed;
				case 554: return SmtpResponseCode.TransactionFailed;

				default: return SmtpResponseCode.Unknown;
			}
		}

		public SmtpResponseCode Code { get; private set; }
		public int CodeNumber { get; private set; }
		public string Message { get; private set; }
		public override string ToString()
		{
			return _raw;
		}
	}
}
