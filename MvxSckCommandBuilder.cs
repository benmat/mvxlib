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
	/// This class will help you create command-strings.
	/// </summary>
	public class MvxSckCommandBuilder
	{
		private string _command;

		/// <summary>
		/// Gets the command.
		/// </summary>
		public string Command
		{
			get { return _command; }
		}

		/// <summary>
		/// Creates an instance.
		/// </summary>
		/// <param name="program">The MI-program to execute.</param>
		public MvxSckCommandBuilder(string program)
		{
			const int PROGRAM_LENGTH = 15;

			if (program.Length > PROGRAM_LENGTH)
				throw new Exception("Application-string is longer than allowed.");

			_command = program.PadRight(PROGRAM_LENGTH, ' ');
		}

		/// <summary>
		/// Adds a parameter to the command.
		/// </summary>
		/// <param name="parameter">The value of type <code>string</code>.</param>
		/// <param name="length">The length of <para>parameter</para>.</param>
		public void Add(string parameter, int length)
		{
			if (parameter.Length > length)
				throw new Exception("Parameter-string is longer than allowed. " + parameter + " is " + parameter.Length.ToString() + " characters long, only " + length.ToString() + " is allowed.");

			_command += parameter.PadRight(length, ' ');
		}

		/// <summary>
		/// Adds a parameter to the command.
		/// </summary>
		/// <param name="parameter">The value of type <code>int</code>.</param>
		/// <param name="length">The length of <para>parameter</para>.</param>
		public void Add(int parameter, int length)
		{
			Add(parameter.ToString(), length);
		}

		/// <summary>
		/// Adds a parameter to the command.
		/// </summary>
		/// <param name="parameter">The value of type <code>DateTime</code>.</param>
		public void Add(DateTime parameter)
		{
			Add(parameter.ToString("yyyyMMdd"), 10);
		}

		/// <summary>
		/// Adds a parameter to the command.
		/// </summary>
		/// <param name="parameter">The value of type <code>double</code>.</param>
		/// <param name="length">The length of <para>parameter</para>.</param>
		public void Add(double parameter, int length)
		{
			const int NUMERIC_PRECISION = 6;

			Add(parameter.ToString("0." + string.Empty.PadRight(NUMERIC_PRECISION, '#'), System.Globalization.CultureInfo.InvariantCulture), length);
		}
	}
}
