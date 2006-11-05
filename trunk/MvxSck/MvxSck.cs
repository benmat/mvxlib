/*
 *
 * MvxLib, an open source C# library used for communication with Intentia Movex.
 * http://mvxlib.sourceforge.net
 *
 * Copyright (C) 2005 - 2006  Mattias Bengtsson
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
 *
 */
using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace MvxLib
{
	/// <summary>
	/// A free C# implementation of MvxSockX (from MVXSOCKX_SVRLib.dll).
	/// </summary>
	public class MvxSck
	{
		/// <summary>
		/// Event delegate used with OnTraceWrite.
		/// </summary>
		public delegate void MvxSckTraceWriteHandler(object sender, EventArgs e);

		/// <summary>
		/// This event fires after something has been written to the TraceLog.
		/// </summary>
		public event MvxSckTraceWriteHandler OnTraceWrite;

		private static System.Text.Encoding _encoding = System.Text.Encoding.GetEncoding("ISO-8859-1");
		private Socket		_socket			= null;
		private bool		_logged_in		= false;
		private IPEndPoint	_host			= null;
		private string		_server			= null;
		private int			_port			= 0;
		private const int	RECEIVE_TIMEOUT	= 30000;
		private const int	SEND_TIMEOUT	= 2000;
		private TraceLog	_tracelog		= new TraceLog();
		private bool		_write_tracelog	= false;

		/// <summary>
		/// The method used when the OnTraceWrite is called.
		/// </summary>
		protected virtual void TraceWrite(EventArgs e)
		{
			if (OnTraceWrite != null)
				OnTraceWrite(this, e);
		}

		/// <summary>
		/// Gets whether or not the object is logged in Movex.
		/// </summary>
		public bool LoggedIn
		{
			get { return _logged_in; }
		}

		/// <summary>
		/// Gets or sets the timeout for the amount of milliseconds to wait
		/// for Movex to return a value.
		/// </summary>
		public int Timeout
		{
			get
			{
				if (_socket != null)
					return _socket.ReceiveTimeout;
				else
					return RECEIVE_TIMEOUT;
			}
			set
			{
				if (_socket != null)
					_socket.ReceiveTimeout = value;
			}
		}

		/// <summary>
		/// Gets the TraceLog. Only written to if WriteTraceLog is set to true.
		/// </summary>
		public TraceLog TraceLog
		{
			get { return _tracelog; }
		}

		/// <summary>
		/// Gets or sets if messages should be written to the TraceLog.
		/// </summary>
		public bool WriteTraceLog
		{
			get { return _write_tracelog; }
			set { _write_tracelog = value; }
		}

		/// <summary>
		/// Gets MvxSck's build-information.
		/// </summary>
		public string Version
		{
			get
			{
				AssemblyName an = Assembly.GetExecutingAssembly().GetName();
				return String.Format("{0}.{1}.{2}.{3}", an.Version.Major, an.Version.Minor, an.Version.Build, an.Version.Revision);
			}
		}

		/// <summary>
		/// Constructor. Use the constructor with IPEndPoint as parameter if possible since this will retrive IP-addresses from DNS when Connect()ing.
		/// </summary>
		/// <param name="server">Movex' server/host/computer-name.</param>
		/// <param name="port">The corresponding port the "RPG"-service/daemon listens to.</param>
		public MvxSck(string server, int port)
		{
			WriteTrace(String.Format("Constructing MvxSck {0}. Parameters: strServer = {1}, intPort = {2}.", this.Version, server, port), true);

			_server = server;
			_port = port;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="host">Movex IP-number and port.</param>
		public MvxSck(IPEndPoint host)
		{
			WriteTrace(String.Format("Constructing MvxSck {0}. Parameters: objIPEndPoint = {1}.", this.Version, host), true);

			_host = host;
		}

		/// <summary>
		/// Connects a socket to the server specified when constructing the MvxSck-object.
		/// </summary>
		public void Connect()
		{
			WriteTrace("Entering Connect().");

			if (_socket != null)
				if (_socket.Connected)
				{
					MvxSckException ex = new MvxSckException("Socket is already connected.");
					WriteTrace(ex);
					throw ex;
				}

			WriteTrace("Constructing Socket-object.");

			_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			_socket.SendTimeout = SEND_TIMEOUT;
			_socket.ReceiveTimeout = RECEIVE_TIMEOUT;

			if (_host != null)
			{
				WriteTrace("Attempting to connect socket.");

				_socket.Connect(_host);

				if (!_socket.Connected)
				{
					MvxSckException ex = new MvxSckException("Unable to connect socket.");
					WriteTrace(ex);
					throw ex;
				}
				
				WriteTrace("Socket connected successfully.");
			}
			else if (_server != null && _port != 0)
			{
				WriteTrace("Resolving IP-addresses for " + _server + ".");

#if MONO
				IPAddress[] ip_addresses = Dns.GetHostByName(_server).AddressList;
#elif NET_2_0
				IPAddress[] ip_addresses = Dns.GetHostAddresses(_server);
#else
				IPAddress[] ip_addresses = Dns.Resolve(_server).AddressList;
#endif

				WriteTrace(ip_addresses.Length + " IP-addresse(s) found.");

				for (int i = 0; i < ip_addresses.Length; i++)
				{
					WriteTrace("Trying to connect to " + ip_addresses[i].ToString() + ".");

					_socket.Connect(new IPEndPoint(ip_addresses[i], _port));

					if (_socket.Connected)
					{
						WriteTrace("Socket connected successfully.");
						return;
					}
				}

				MvxSckException ex = new MvxSckException("Did not find any connectable servers.");
				WriteTrace(ex);
				throw ex;
			}
			else
			{
				MvxSckException ex = new MvxSckException("host, server or port not configured.");
				WriteTrace(ex);
				throw ex;
			}
		}

		/// <summary>
		/// Destructor.
		/// </summary>
		~MvxSck()
		{
			Close();
			_socket = null;
		}

		/// <summary>
		/// Logging of and closing the connection and socket.
		/// </summary>
		public void Close()
		{
			WriteTrace("Entering Close().");

			if (_socket == null)
				return;

			if (!_socket.Connected)
				return;

			if (_logged_in)
				Execute("CLOSE", true);

			_logged_in = false;

			WriteTrace("Closing Socket.");

			_socket.Close();
		}

		/// <summary>
		/// Executing a command at the current MI-program.
		/// </summary>
		/// <param name="command">The commandstring.</param>
		/// <returns>The string returned from Movex.</returns>
		public string Execute(string command)
		{
			return Execute(command, true);
		}

		private string Execute(string command, bool check_login)
		{
			WriteTrace("Entering Execute(). Parameters: strCommand = <" + command + ">, blnCheckLogin = " + check_login.ToString() + ".");

			if (_socket == null)
			{
				MvxSckException ex = new MvxSckException("Socket is not constructed.");
				WriteTrace(ex);
				throw ex;
			}

			if (!_socket.Connected)
			{
				MvxSckException ex = new MvxSckException("Socket is not connected.");
				WriteTrace(ex);
				throw ex;
			}

			if (check_login && !_logged_in)
			{
				MvxSckException ex = new MvxSckException("Cannot run command because not logged in.");
				WriteTrace(ex);
				throw ex;
			}

			if (command != null)
			{
				byte[] send = AddCommandLengthPrefix(command);
			
				WriteTrace("Sending command through socket.");

				_socket.Send(send, send.Length, SocketFlags.None);
			}

			int count_received = 0, count_total_received = 0;
			int count_expected = 0, i = 0;
			byte[] buffer = new byte[4];
			string received = string.Empty;

			WriteTrace("Beginning to receive data.");

			do 
			{
				count_received = _socket.Receive(buffer, buffer.Length, 0);
				received += _encoding.GetString(buffer, 0, count_received);
				
				count_total_received += count_received;

				if (i == 0 && count_received >= 4)
					count_expected = buffer[2] * 256 + buffer[3] + 4;
				
				if (count_expected == 0)
					return null;

				i++;
				
				WriteTrace(String.Format("Iteration {0}. Recieved {1} bytes (total {2} bytes so far), expecting {3} bytes total.", i, count_received, count_total_received, count_expected));
				
				buffer = new byte[count_expected - 4];
			}
			while (count_expected > count_total_received);

			return received.Substring(4);
		}

		/// <summary>
		/// Adds CheckLength-bytes in the beginning of the command before sending it to Movex.
		/// </summary>
		/// <param name="command">The command without CheckLength-bytes.</param>
		/// <returns>The command with CheckLength-bytes.</returns>
		private byte[] AddCommandLengthPrefix(string command)
		{
			WriteTrace("Entering AddCommandLengthPrefix().");

			byte[] ret = _encoding.GetBytes("\x00\x00\x01\x01" + command);

			ret[2] = Convert.ToByte(command.Length / 256);
			ret[3] = Convert.ToByte(command.Length % 256);

			return ret;
		}

		/// <summary>
		/// Log in to Movex 10. This must be done before sending commands. Throws an MvxSckException if unsuccessful.
		/// </summary>
		/// <param name="username">The username/login.</param>
		/// <param name="password">The password.</param>
		/// <param name="library">The library in Movex where the Program resides.</param>
		/// <param name="program">The MI-program to use.</param>
		public void Login(string username, string password, string library, string program)
		{
			WriteTrace(String.Format("Entering Login(). Parameters: username = {0}, password = {1}, library = {2}, program = {3}", username, password, library, program));

			string command = "LOGON" + 
				Environment.MachineName.PadRight(32) +
				username.PadRight(16) +
				password.PadRight(16) +
				(library + "/" + program).PadRight(32) +
				program.PadRight(32);

			string received = Execute(command.ToUpper(), false);

			if (received != "Connection accepted")
				throw new MvxSckException("Login unsuccessful.", new Exception(received));

			_logged_in = true;
		}

		/// <summary>
		/// Alpha! (untested)
		/// Log in to Movex 12. This must be done before sending commands. Throws an MvxSckException if unsuccessful.
		/// </summary>
		/// <param name="username">The username/login.</param>
		/// <param name="password">The password.</param>
		/// <param name="program">The MI-program to use.</param>
		public void LoginMovex12(string username, string password, string program)
		{
			WriteTrace(String.Format("Entering LoginMovex12(). Parameters: username = {0}, password = {1}, program = {2}", username, password, program));

			string command = "PWLOG" +
				Environment.MachineName.PadRight(32) +
				username.PadRight(16) +
				Movex12PasswordCipher(password).PadRight(13) +
				program.PadRight(32);

			string received = Execute(command.ToUpper(), false);

			if (received != "Connection accepted")
				throw new MvxSckException("Login unsuccessful.", new Exception(received));

			_logged_in = true;
		}

		/// <summary>
		/// Alpha! (untested)
		/// Generates a Movex 12 compatible password.
		/// </summary>
		/// <param name="password">The password in clear text.</param>
		/// <returns>The ciphered password.</returns>
		public static string Movex12PasswordCipher(string password)
		{
			/*
			 *
			 * Movex 12 ciphers the password using a XOR swap algorithm.
			 * The key starts at \x38 and increases one every char.
			 *
			 */

			byte key = 38;
			byte[] pass = _encoding.GetBytes(password);

			for (int i = 0; i < password.Length; i++)
			{
				pass[i] ^= key;
				key++;
			}

			return _encoding.GetString(pass);
		}

		/// <summary>
		/// Fires OnTraceWrite after writing to the TraceLog.
		/// </summary>
		/// <param name="message">The trace-message.</param>
		private void WriteTrace(string message)
		{
			WriteTrace(message, false);
		}

		/// <summary>
		/// Fires OnTraceWrite after writing to the TraceLog.
		/// </summary>
		/// <param name="message">The trace-message.</param>
		/// <param name="force_write">True will force the message to be written.</param>
		private void WriteTrace(string message, bool force_write)
		{
			if (_write_tracelog || force_write)
			{
				_tracelog.WriteTrace(message);
				TraceWrite(new EventArgs());
			}
		}

		/// <summary>
		/// Fires OnTraceWrite after writing to the TraceLog.
		/// </summary>
		/// <param name="ex">An exception to write.</param>
		private void WriteTrace(Exception ex)
		{
			_tracelog.WriteTrace(ex);
			TraceWrite(new EventArgs());
		}
	}

	/// <summary>
	/// Exception-types used in MvxSck.
	/// </summary>
	public class MvxSckException : Exception
	{
		/// <summary>
		/// Exception thrown from MvxSck.
		/// </summary>
		/// <param name="message">Errormessage.</param>
		public MvxSckException(string message) : base(message)
		{
		}
		/// <summary>
		/// Exception thrown from MvxSck.
		/// </summary>
		/// <param name="message">Errormessage.</param>
		/// <param name="inner_exception">Inner exception, the exception that caused the outer exception.</param>
		public MvxSckException(string message, Exception inner_exception)
			: base(message, inner_exception)
		{
		}
	}
}