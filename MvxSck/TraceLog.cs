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

namespace MvxLib
{
	/// <summary>
	/// A simple logging-class.
	/// </summary>
	public class TraceLog
	{
		private System.Collections.ArrayList _objTraceLogEntries = new System.Collections.ArrayList();
		private long _lngLastTraceTicks = DateTime.Now.Ticks;

		/// <summary>
		/// Returns an array of all TraceLogEntries.
		/// </summary>
		public TraceLogEntry[] TraceLogEntries
		{
			get { return (TraceLogEntry[]) _objTraceLogEntries.ToArray(typeof (TraceLogEntry)); }
		}

		/// <returns>All TraceLogEntries concatenated.</returns>
		public override string ToString()
		{
			string strReturn = string.Empty;

			for (int i = 0; i < TraceLogEntries.Length; i++)
				strReturn += TraceLogEntries[i].ToString() + Environment.NewLine;

			return strReturn;
		}

		/// <summary>
		/// Add a TraceLogEntry to the log.
		/// </summary>
		/// <param name="strMessage">The message of the trace.</param>
		public void WriteTrace(string strMessage)
		{
			_objTraceLogEntries.Add(new TraceLogEntry(strMessage, DateTime.Now.Ticks - _lngLastTraceTicks));
			_lngLastTraceTicks = DateTime.Now.Ticks;
		}

		/// <summary>
		/// Add a TraceLogEntry to the log.
		/// </summary>
		/// <param name="ex">The exception to log to the trace.</param>
		public void WriteTrace(Exception ex)
		{
			WriteTrace("Exception: " + ex.ToString());
		}

		/// <summary>
		/// Struct containing the actual tracelog-messages.
		/// </summary>
		public struct TraceLogEntry
		{
			DateTime	TimeOfEntry;
			long		TicksSinceLastEntry;
			string		Message;

			/// <summary>
			/// Enter an entry to the log.
			/// </summary>
			/// <param name="strMessage">Message of the entry.</param>
			/// <param name="lngTicksSinceLastEntry">Ticks elapsed since the last log was entered.</param>
			public TraceLogEntry(string strMessage, long lngTicksSinceLastEntry)
			{
				TimeOfEntry			= DateTime.Now;
				TicksSinceLastEntry	= lngTicksSinceLastEntry;
				Message				= strMessage;
			}

			/// <returns>A string suitable for storing.</returns>
			public override string ToString()
			{
				return TimeOfEntry.ToString() + " " + TicksSinceLastEntry.ToString().PadRight(10) + Message;
			}
		}
	}
}