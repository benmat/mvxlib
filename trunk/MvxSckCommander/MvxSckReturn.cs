/*
 *
 * MvxLib, an open source C# library for communication with Intentia Movex.
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
	public class MvxSckReturn
	{
		public struct PriceItem
		{
			public double	Price;
			public int		StaggerQuantity;
			public bool		EmptyPrice;

			public PriceItem(
				double	dblPrice,
				int		intStaggerQuantity,
				bool	blnEmptyPrice
				)
			{
				this.Price				= dblPrice;
				this.StaggerQuantity	= intStaggerQuantity;
				this.EmptyPrice			= blnEmptyPrice;
			}
		}

		public static PriceItem GetPriceItem(string strCommandReturn)
		{
			string strPrice				= strCommandReturn.Substring(69, 10).Trim();
			string strStaggerQuantity	= strCommandReturn.Substring(87, 3).Trim();

			return new PriceItem(ParseDouble(strPrice), ParseInt(strStaggerQuantity, 1), (strPrice == ""));
		}

		public struct PriceLineItem
		{
			public double	Price;
			public bool		EmptyPrice;

			public PriceLineItem(
				double	dblPrice,
				bool	blnEmptyPrice
				)
			{
				this.Price		= dblPrice;
				this.EmptyPrice	= blnEmptyPrice;
			}
		}

		public static PriceLineItem GetPriceLineItem(string strCommandReturn)
		{
			string strPrice = strCommandReturn.Substring(18, 15).Trim();
			return new PriceLineItem(ParseDouble(strPrice), (strPrice == ""));
		}

		public struct PriceMLineItem
		{
			public string	ItemNumber;
			public double	Salesprice;
			public bool		EmptyPrice;
			public double	LineAmount;
			public double	OrderQuantity;
			public string	Pricelist;
			public bool		ScaledPricelist;
			public bool		Error;

			public PriceMLineItem(
				string	strItemNumber,
				double	dblSalesprice,
				bool	blnEmptyPrice,
				double	dblLineAmount,
				double	dblOrderQuantity,
				string	strPricelist,
				bool	blnScaledPricelist,
				bool	blnError
				)
			{
				this.ItemNumber			= strItemNumber;
				this.Salesprice			= dblSalesprice;
				this.EmptyPrice			= blnEmptyPrice;
				this.LineAmount			= dblLineAmount;
				this.OrderQuantity		= dblOrderQuantity;
				this.Pricelist			= strPricelist;
				this.ScaledPricelist	= blnScaledPricelist;
				this.Error				= blnError;
			}
		}

		public static PriceMLineItem[] GetPriceMLineItems(string strCommandReturn)
		{
			ArrayList arrReturnItems = new ArrayList();
			MvxSckReturnBuilder objRb = new MvxSckReturnBuilder(strCommandReturn.Trim());
			objRb.Add(15); // Return code

			while (objRb.CommandStringLeft != "FINITO" && objRb.CommandStringLeft.Length > 0)
			{
				string strItemNumber		= objRb.Add(15);
				bool blnEmptyPrice			= (objRb.CommandStringLeft.Substring(0, 15).Trim() == "");
				double dblPrice				= ParseDouble(objRb.Add(15));
				double dblLineAmount		= ParseDouble(objRb.Add(15));
				double dblOrderQuantity		= ParseDouble(objRb.Add(15));
				string strPricelist			= objRb.Add(2);
				bool blnScaledPricelist		= ParseBool(objRb.Add(1));
				bool blnError				= ParseBool(objRb.Add(1));

				arrReturnItems.Add(new PriceMLineItem(
					strItemNumber,
					dblPrice,
					blnEmptyPrice,
					dblLineAmount,
					dblOrderQuantity,
					strPricelist,
					blnScaledPricelist,
					blnError
					));
			}

			return (PriceMLineItem[]) arrReturnItems.ToArray(typeof(PriceMLineItem));
		}

		private static double ParseDouble(string strInstring)
		{
			if (strInstring == "")
				return (double) 0;

			return Convert.ToDouble(ReplaceDecimalSeperator(strInstring));
		}

		private static bool ParseBool(string strInstring)
		{
			if (strInstring == "1")
				return true;
			else
				return false;
		}

		private static int ParseInt(string strInstring, int intDefault)
		{
			if (strInstring.Trim() == "")
				return intDefault;

			return Convert.ToInt32(strInstring);
		}

		private static string ReplaceDecimalSeperator(string strInstring)
		{
			string strCurrentDecimal = System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;

			return strInstring.Replace(",", strCurrentDecimal).Replace(".", strCurrentDecimal);
		}
	}
}
