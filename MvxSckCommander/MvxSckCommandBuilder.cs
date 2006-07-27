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
	public class MvxSckCommandBuilder
	{
		private string _strCommand;

		public string Command
		{
			get { return _strCommand; }
		}

		public MvxSckCommandBuilder(string strApplication)
		{
			const int intApplicationLength = 15;

			if (strApplication.Length > intApplicationLength)
				throw new Exception("Application-string is longer than allowed.");

			_strCommand = strApplication.PadRight(intApplicationLength, ' ');
		}

		public void Add(string strParameter, int intLength)
		{
			if (strParameter.Length > intLength)
				throw new Exception("Parameter-string is longer than allowed. " + strParameter + " is " + strParameter.Length.ToString() + " characters long, only " + intLength.ToString() + " is allowed.");

			_strCommand += strParameter.PadRight(intLength, ' ');
		}

		public void Add(int intParameter, int intLength)
		{
			Add(intParameter.ToString(), intLength);
		}

		public void Add(DateTime dtDate)
		{
			Add(dtDate.ToString("yyyyMMdd"), 10);
		}

		public void Add(double dblParameter, int intLength)
		{
			const int intPrecision = 6;

			Add(dblParameter.ToString("0." + string.Empty.PadRight(intPrecision, '#'), System.Globalization.CultureInfo.InvariantCulture), intLength);
		}
	}
}
