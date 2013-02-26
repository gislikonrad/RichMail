using RichMail.Interfaces;
using RichMail.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RichMail.Implementations
{
	public class TcpClientConnectionManager : IConnectionManager
	{
		private TcpClient _client;

		public TcpClientConnectionManager()
		{
			_client = new TcpClient();
			State = ConnectionState.Disconnected;
		}

		public ConnectionState State
		{
			get;
			private set;
		}

		public async Task<SmtpResponse> Connect(string host, int port)
		{
			await _client.ConnectAsync(host, port);
			State = ConnectionState.Connected;
			return await ReadResponse();
		}

		public async Task<SmtpResponse> ExecuteCommand(string command)
		{
			DebugOutput("Client> {0}", command);
			await WriteRequest(command);
			return await ReadResponse();
		}

		private async Task WriteRequest(string command)
		{
			var temp = State;
			State = ConnectionState.Busy;
			var writer = new StreamWriter(_client.GetStream(), Encoding.ASCII);
			await writer.WriteLineAsync(command);
			await writer.FlushAsync();
			State = temp;
		}

		private async Task<SmtpResponse> ReadResponse()
		{
			var temp = State;
			State = ConnectionState.Busy;
			var reader = new StreamReader(_client.GetStream(), Encoding.ASCII);
			var response = await reader.ReadLineAsync();
			DebugOutput("Server> {0}", response);
			State = temp;
			return new SmtpResponse(response);
		}

		private void DebugOutput(string message, params object[] args)
		{
			Debug.WriteLine(message, args);
		}


		public void Dispose()
		{
			_client.Close();
			State = ConnectionState.Disconnected;
		}
	}
}
