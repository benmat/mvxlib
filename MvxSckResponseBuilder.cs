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
using System.Collections;

namespace MvxLib
{
	/// <summary>
	/// This class will help you format response-strings.
	/// </summary>
	public class MvxSckResponseBuilder
	{
		private string		_response;
		private ArrayList	_response_values;
		private int			_cur_pos;

		/// <summary>
		/// Gets the response-values as a string-array.
		/// </summary>
		public string[] ResponseValues
		{
			get { return (string[]) _response_values.ToArray(typeof(string)); }
		}

		/// <summary>
		/// Gets the remaining commandstring that is not parsed.
		/// </summary>
		public string RemainingResponse
		{
			get { return _response.Substring(_cur_pos); }
		}

		/// <summary>
		/// Creates an instance.
		/// </summary>
		/// <param name="response">The response-string Movex returned.</param>
		public MvxSckResponseBuilder(string response)
		{
			_response = response;
			_response_values = new ArrayList();
			_cur_pos = 0;
		}

		/// <summary>
		/// Parses the next value of the response.
		/// </summary>
		/// <param name="length">The expected length of the value.</param>
		/// <returns>The parsed value.</returns>
		public string GetString(int length)
		{
			string ret = _response.Substring(_cur_pos, length).Trim();
			_cur_pos += length;
			_response_values.Add(ret);
			return ret;
		}

		/// <summary>
		/// Parses the next value of the response.
		/// </summary>
		/// <param name="length">The expected length of the value.</param>
		/// <returns>The parsed value.</returns>
		public double GetDouble(int length)
		{
			string ret = GetString(length);

			if (ret == "")
				return 0;

			return Convert.ToDouble(ret, System.Globalization.CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// Parses the next value of the response.
		/// </summary>
		/// <returns>The parsed value.</returns>
		public bool GetBool()
		{
			if (GetString(1) == "1")
				return true;
			else
				return false;
		}

		/// <summary>
		/// Parses the next value of the response.
		/// </summary>
		/// <param name="length">The expected length of the value.</param>
		/// <returns>The parsed value.</returns>
		public int GetInt(int length)
		{
			string ret = GetString(length);

			if (ret == "")
				return 0;

			return Convert.ToInt32(ret);
		}
	}
}
