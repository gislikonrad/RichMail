using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RichMail.Models
{
	public enum ConnectionState
	{
		Disconnected,
		Connected,
		Authenticated,
		Busy
	}

	[Flags]
	public enum SmtpResponseCode
	{
		Unknown										= 0x000000,
		NonStandardSuccess							= 0x000001,
		StatusOrHelpReply							= 0x000002,
		HelpMessage									= 0x000004,
		Ready										= 0x000008,
		ClosingTransmissionChannel					= 0x000010,
		MailActionCompleted							= 0x000020,
		UserNotLocalWillForward						= 0x000040,

		StartMailInput								= 0x000080,

		NotAvailable								= 0x000100,
		MailActionNotTakenMailboxUnavailable		= 0x000200,
		ActionAbortedLocalErrorInProcessing			= 0x000400,
		ActionNotTakenInsufficientSystemStorage		= 0x000800,

		SyntaxErrorInCommand						= 0x001000,
		SyntaxErrorInParametersOrArguments			= 0x002000,
		CommandNotImplemented						= 0x004000,
		BadSequenceOfCommands						= 0x008000,
		ParameterNotImplemented						= 0x010000,
		DomainDoesNotAcceptMail						= 0x020000,
		AccessDenied								= 0x040000,
		ActionNotTakenMailboxUnavailable			= 0x080000,
		UserNotLocalTryForwarding					= 0x100000,
		MailActionAbortedExceededStorageAllocation	= 0x200000,
		ActionNotTakenMailboxNameNotAllowed			= 0x400000,
		TransactionFailed							= 0x800000
	}
}
