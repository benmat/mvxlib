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
	public class MvxSckReturnBuilder
	{
		private string		_return;
		private ArrayList	_return_values;
		private int			_cur_pos;

		public string[] ReturnValues
		{
			get { return (string[]) _return_values.ToArray(typeof(string)); }
		}

		public string CommandStringLeft
		{
			get { return _return.Substring(_cur_pos); }
		}

		public MvxSckReturnBuilder(string returned_string)
		{
			_return = returned_string;
			_return_values = new ArrayList();
			_cur_pos = 0;
		}

		public string GetString(int length)
		{
			string ret = _return.Substring(_cur_pos, length).Trim();
			_cur_pos += length;
			_return_values.Add(ret);
			return ret;
		}

		public double GetDouble(int length)
		{
			string ret = GetString(length);

			if (ret == "")
				return 0;

			return Convert.ToDouble(ret, System.Globalization.CultureInfo.InvariantCulture);
		}

		public bool GetBool()
		{
			if (GetString(1) == "1")
				return true;
			else
				return false;
		}

		public int GetInt(int length)
		{
			string ret = GetString(length);

			if (ret == "")
				return 0;

			return Convert.ToInt32(ret);
		}
	}
}
