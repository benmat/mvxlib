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

		private static System.Text.Encoding _objEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");
		private Socket		_objSocket		= null;
		private bool		_blnLoggedIn	= false;
		private IPEndPoint	_objIPEndPoint	= null;
		private string		_strServer		= null;
		private int			_intPort		= 0;
		
		private TraceLog	_objTraceLog	= new TraceLog();
		private bool		_blnWriteTrace	= false;

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
			get { return _blnLoggedIn; }
		}

		/// <summary>
		/// Gets the TraceLog. Only written to if WriteTraceLog is set to true.
		/// </summary>
		public TraceLog TraceLog
		{
			get { return _objTraceLog; }
		}

		/// <summary>
		/// Gets or sets if messages should be written to the TraceLog.
		/// </summary>
		public bool WriteTraceLog
		{
			get { return _blnWriteTrace; }
			set { _blnWriteTrace = value; }
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
		/// <param name="strServer">Movex' server/host/computer-name.</param>
		/// <param name="intPort">The corresponding port the "RPG"-service/daemon listens to.</param>
		public MvxSck(string strServer, int intPort)
		{
			WriteTrace(String.Format("Constructing MvxSck {0}. Parameters: strServer = {1}, intPort = {2}.", this.Version, strServer, intPort));

			_strServer	= strServer;
			_intPort	= intPort;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="objIPEndPoint">Movex IP-number and port.</param>
		public MvxSck(IPEndPoint objIPEndPoint)
		{
			WriteTrace(String.Format("Constructing MvxSck {0}. Parameters: objIPEndPoint = {1}.", this.Version, objIPEndPoint));

			_objIPEndPoint = objIPEndPoint;
		}

		/// <summary>
		/// Connects a socket to the server specified when constructing the MvxSck-object.
		/// </summary>
		public void Connect()
		{
			if (_blnWriteTrace) WriteTrace("Entering Connect().");

			if (_objSocket != null)
				if (_objSocket.Connected)
				{
					MvxSckException ex = new MvxSckException("Socket is already connected.");
					if (_blnWriteTrace) WriteTrace(ex);
					throw ex;
				}

			CreateSocketObject();

			if (_objIPEndPoint != null)
			{
				if (_blnWriteTrace) WriteTrace("Attempting to connect socket.");

				_objSocket.Connect(_objIPEndPoint);

				if (!_objSocket.Connected)
				{
					MvxSckException ex = new MvxSckException("Unable to connect socket.");
					if (_blnWriteTrace) WriteTrace(ex);
					throw ex;
				}
				
				if (_blnWriteTrace) WriteTrace("Socket connected successfully.");
			}
			else if (_strServer != null && _intPort != 0)
			{
				if (_blnWriteTrace) WriteTrace("Resolving IP-addresses for " + _strServer + ".");

#if MONO
				IPHostEntry objHostEntry = Dns.GetHostByName(_strServer);
				IPAddress[] objIPaddresses = objHostEntry.AddressList;
#elif NET_2_0
				IPAddress[] objIPaddresses = Dns.GetHostAddresses(_strServer);
#else
				IPAddress[] objIPaddresses = Dns.Resolve(_strServer).AddressList;
#endif

				if (_blnWriteTrace) WriteTrace(objIPaddresses.Length + " IP-addresse(s) found.");

				for (int i = 0; i < objIPaddresses.Length; i++)
				{
					if (_blnWriteTrace) WriteTrace("Trying to connect to " + objIPaddresses[i].ToString() + ".");

					_objSocket.Connect(new IPEndPoint(objIPaddresses[i], _intPort));

					if (_objSocket.Connected)
					{
						if (_blnWriteTrace) WriteTrace("Socket connected successfully.");
						return;
					}
				}

				MvxSckException ex = new MvxSckException("Did not find any connectable servers.");
				if (_blnWriteTrace) WriteTrace(ex);
				throw ex;
			}
			else
			{
				MvxSckException ex = new MvxSckException("IPEndPoint, Server or Port not configured.");
				if (_blnWriteTrace) WriteTrace(ex);
				throw ex;
			}
		}

		/// <summary>
		/// Destructor.
		/// </summary>
		~MvxSck()
		{
			if (_objSocket == null)
				return;

			if (_objSocket.Connected)
				Close();

			_objSocket = null;
		}

		private void CreateSocketObject()
		{
			if (_blnWriteTrace) WriteTrace("Constructing Socket-object.");

			_objSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		}

		/// <summary>
		/// Logging of and closing the connection and socket.
		/// </summary>
		public void Close()
		{
			if (_blnWriteTrace) WriteTrace("Entering Close().");

			_blnLoggedIn = false;

			if (_objSocket == null)
				return;

			if (!_objSocket.Connected)
				return;

			if (_blnLoggedIn)
				Execute("CLOSE", true);

			if (_blnWriteTrace) WriteTrace("Closing Socket.");
			_objSocket.Close();
		}

		/// <summary>
		/// Executing a command at the current MI-program.
		/// </summary>
		/// <param name="strCommand">The commandstring.</param>
		/// <returns>The string returned from Movex.</returns>
		public string Execute(string strCommand)
		{
			return Execute(strCommand, true);
		}

		private string Execute(string strCommand, bool blnCheckLogin)
		{
			if (_blnWriteTrace) WriteTrace("Entering Execute(). Parameters: strCommand = <" + strCommand + ">, blnCheckLogin = " + blnCheckLogin.ToString() + ".");

			if (_objSocket == null)
			{
				MvxSckException ex = new MvxSckException("Socket is not constructed.");
				if (_blnWriteTrace) WriteTrace(ex);
				throw ex;
			}

			if (!_objSocket.Connected)
			{
				MvxSckException ex = new MvxSckException("Socket is not connected.");
				if (_blnWriteTrace) WriteTrace(ex);
				throw ex;
			}

			if (blnCheckLogin && !_blnLoggedIn)
			{
				MvxSckException ex = new MvxSckException("Cannot run command because not logged in.");
				if (_blnWriteTrace) WriteTrace(ex);
				throw ex;
			}

			if (strCommand != null)
			{
				byte[] baSend = AddCommandLengthPrefix(strCommand);
			
				if (_blnWriteTrace) WriteTrace("Sending command through socket.");

				_objSocket.Send(baSend, baSend.Length, SocketFlags.None);
			}

			int intBytesRecieved = 0, intTotalBytesReceived = 0;
			int intBytesToExpect = 0, i = 0;
			byte[] baBuffer = new byte[4];
			string strReceived = string.Empty;

			if (_blnWriteTrace) WriteTrace("Beginning to receive data.");

			do 
			{
				intBytesRecieved = _objSocket.Receive(baBuffer, baBuffer.Length, 0);
				strReceived += _objEncoding.GetString(baBuffer, 0, intBytesRecieved);
				
				intTotalBytesReceived += intBytesRecieved;

				if (i == 0 && intBytesRecieved >= 4)
					intBytesToExpect = baBuffer[2] * 256 + baBuffer[3] + 4;
				
				if (intBytesToExpect == 0)
					return null;

				i++;
				
				if (_blnWriteTrace) WriteTrace(String.Format("Iteration {0}. Recieved {1} bytes (total {2} bytes so far), expecting {3} bytes total.", i, intBytesRecieved, intTotalBytesReceived, intBytesToExpect));
				
				baBuffer = new byte[intBytesToExpect - 4];
			}
			while (intBytesToExpect > intTotalBytesReceived);

			return strReceived.Substring(4);
		}

		/// <summary>
		/// Adds CheckLength-bytes in the beginning of the command before sending it to Movex.
		/// </summary>
		/// <param name="strCommand">The command without CheckLength-bytes.</param>
		/// <returns>The command with CheckLength-bytes.</returns>
		private byte[] AddCommandLengthPrefix(string strCommand)
		{
			if (_blnWriteTrace) WriteTrace("Entering AddCommandLengthPrefix().");

			byte[] bcCommand = _objEncoding.GetBytes("\x00\x00\x01\x01" + strCommand);

			bcCommand[2] = Convert.ToByte(strCommand.Length / 256);
			bcCommand[3] = Convert.ToByte(strCommand.Length % 256);

			return bcCommand;
		}

		/// <summary>
		/// Log in to Movex 10. This must be done before sending commands. Throws an MvxSckException if unsuccessful.
		/// </summary>
		/// <param name="strUsername">The username/login.</param>
		/// <param name="strPassword">The password.</param>
		/// <param name="strLibrary">The library in Movex where the Program resides.</param>
		/// <param name="strProgram">The MI-program to use.</param>
		public void Login(string strUsername, string strPassword, string strLibrary, string strProgram)
		{
			if (_blnWriteTrace) WriteTrace(String.Format("Entering Login(). Parameters: strUsername = {0}, strPassword = {1}, strLibrary = {2}, strProgram = {3}", strUsername, strPassword, strLibrary, strProgram));

			string strCommand = "LOGON" + 
				Environment.MachineName.PadRight(32) +
				strUsername.PadRight(16) +
				strPassword.PadRight(16) +
				(strLibrary + "/" + strProgram).PadRight(32) +
				strProgram.PadRight(32);

			string strReceived = Execute(strCommand.ToUpper(), false);

			if (strReceived != "Connection accepted")
				throw new MvxSckException("Login unsuccessful.", new Exception(strReceived));

			_blnLoggedIn = true;
		}

		/// <summary>
		/// Alpha! (untested)
		/// Log in to Movex 12. This must be done before sending commands. Throws an MvxSckException if unsuccessful.
		/// </summary>
		/// <param name="strUsername">The username/login.</param>
		/// <param name="strPassword">The password.</param>
		/// <param name="strProgram">The MI-program to use.</param>
		public void LoginMovex12(string strUsername, string strPassword, string strProgram)
		{
			if (_blnWriteTrace) WriteTrace(String.Format("Entering LoginMovex12(). Parameters: strUsername = {0}, strPassword = {1}, strProgram = {2}", strUsername, strPassword, strProgram));

			string strCommand = "PWLOG" +
				Environment.MachineName.PadRight(32) +
				strUsername.PadRight(16) +
				Movex12PasswordCipher(strPassword).PadRight(13) +
				strProgram.PadRight(32);

			string strReceived = Execute(strCommand.ToUpper(), false);

			if (strReceived != "Connection accepted")
				throw new MvxSckException("Login unsuccessful.", new Exception(strReceived));

			_blnLoggedIn = true;
		}

		/// <summary>
		/// Alpha! (untested)
		/// Generates a Movex 12 compatible password.
		/// </summary>
		/// <param name="strPassword">The password in clear text.</param>
		/// <returns>The ciphered password.</returns>
		public static string Movex12PasswordCipher(string strPassword)
		{
			/*
			 *
			 * Movex 12 ciphers the password using a XOR swap algorithm.
			 * The key starts at \x38 and increases one every char.
			 *
			 */

			byte bytCiphKey = 38;
			byte[] bytCiphPass = _objEncoding.GetBytes(strPassword);

			for (int i = 0; i < strPassword.Length; i++)
			{
				bytCiphPass[i] ^= bytCiphKey;
				bytCiphKey++;
			}

			return _objEncoding.GetString(bytCiphPass);
		}

		/// <summary>
		/// Fires OnTraceWrite after writing to the TraceLog.
		/// </summary>
		/// <param name="strMessage">The trace-message.</param>
		private void WriteTrace(string strMessage)
		{
			_objTraceLog.WriteTrace(strMessage);
			TraceWrite(new EventArgs());
		}

		/// <summary>
		/// Fires OnTraceWrite after writing to the TraceLog.
		/// </summary>
		/// <param name="ex">An exception to write.</param>
		private void WriteTrace(Exception ex)
		{
			_objTraceLog.WriteTrace(ex);
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
		/// <param name="innerException">Inner exception, the exception that caused the outer exception.</param>
		public MvxSckException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}