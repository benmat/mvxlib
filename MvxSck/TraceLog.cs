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
		private System.Collections.ArrayList _entries = new System.Collections.ArrayList();
		private long previous_tick = DateTime.Now.Ticks;

		/// <summary>
		/// Returns an array of all TraceLog entries.
		/// </summary>
		public TraceLogEntry[] Entries
		{
			get { return (TraceLogEntry[]) _entries.ToArray(typeof (TraceLogEntry)); }
		}

		/// <returns>All TraceLogEntries concatenated.</returns>
		public override string ToString()
		{
			string strReturn = string.Empty;

			for (int i = 0; i < Entries.Length; i++)
				strReturn += Entries[i].ToString() + Environment.NewLine;

			return strReturn;
		}

		/// <summary>
		/// Add a TraceLogEntry to the log.
		/// </summary>
		/// <param name="message">The message of the trace.</param>
		public void WriteTrace(string message)
		{
			_entries.Add(new TraceLogEntry(message, DateTime.Now.Ticks - previous_tick));
			previous_tick = DateTime.Now.Ticks;
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
			long		ElapsedTicks;
			string		Message;

			/// <summary>
			/// Enter an entry to the log.
			/// </summary>
			/// <param name="message">Message of the entry.</param>
			/// <param name="elapsed_ticks">Ticks elapsed since the last log was entered.</param>
			public TraceLogEntry(string message, long elapsed_ticks)
			{
				TimeOfEntry		= DateTime.Now;
				ElapsedTicks	= elapsed_ticks;
				Message			= message;
			}

			/// <returns>A string suitable for storing.</returns>
			public override string ToString()
			{
				return TimeOfEntry.ToString() + " " + ElapsedTicks.ToString().PadRight(10) + Message;
			}
		}
	}
}