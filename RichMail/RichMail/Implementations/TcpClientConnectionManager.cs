using RichMail.Interfaces;
using RichMail.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RichMail.Implementations
{
	public class TcpClientConnectionManager : IConnectionManager
	{
		private TcpClient _client;
		private Stream _stream;

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

		public async Task<SmtpResponse> ConnectAsync(string host, int port, bool useSsl)
		{
			await _client.ConnectAsync(host, port);
			State = ConnectionState.Connected;
			if (useSsl)
			{
				var sslStream = new SslStream(_client.GetStream());
				await sslStream.AuthenticateAsClientAsync(host);
				_stream = sslStream;
				State = ConnectionState.Authenticated;
			}
			else
			{
				_stream = _client.GetStream();
			}
			var response = await ReadResponseAsync();
			response.Command = "Connect";
			return response;
		}

		public async Task<SmtpResponse> ExecuteCommandAsync(string command)
		{
			DebugOutput("Client> {0}", command);
			await WriteRequestAsync(command);
			var response = await ReadResponseAsync();
			response.Command = command;
			return response;
		}

		private async Task WriteRequestAsync(string command)
		{
			var temp = State;
			State = ConnectionState.Busy;
			var writer = new StreamWriter(_stream, Encoding.ASCII);
			await writer.WriteLineAsync(command);
			await writer.FlushAsync();
			State = temp;
		}

		private async Task<SmtpResponse> ReadResponseAsync()
		{
			var temp = State;
			State = ConnectionState.Busy;
			var reader = new StreamReader(_stream, Encoding.ASCII);
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
			_stream.Close();
			_client.Close();
			State = ConnectionState.Disconnected;
		}
	}
}
