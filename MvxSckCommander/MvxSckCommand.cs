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
	public class MvxSckCommand
	{
		public static string GetPrice(
			int			intCompany,
			string		strDivision,
			string		strWharehouse,
			string		strItemNumber,
			int			intQuantity,
			string		strCustomerNumber,
			string		strOrderType,
			string		strCurrency,
			string		strPricelist,
			string		strDiscountModel,
			DateTime	dtDate,
			string		strUnitOfMeasure
			)
		{
			MvxSckCommandBuilder objCb = new MvxSckCommandBuilder("GetPrice");
			objCb.Add(intCompany, 3);
			objCb.Add(strDivision, 3);
			objCb.Add(strWharehouse, 3);
			objCb.Add(strItemNumber, 15);
			objCb.Add(intQuantity, 16);
			objCb.Add(strCustomerNumber, 10);
			objCb.Add(strOrderType, 3);
			objCb.Add(strCurrency, 3);
			objCb.Add(strPricelist, 2);
			objCb.Add(strDiscountModel, 10);
			objCb.Add(dtDate);
			objCb.Add(strUnitOfMeasure, 3);

			return objCb.Command;
		}

		public static string GetPrice(
			int			intCompany,
			string		strWharehouse,
			string		strItemNumber,
			int			intQuantity,
			string		strCustomerNumber,
			string		strOrderType,
			string		strCurrency,
			string		strPricelist,
			DateTime	dtDate,
			string		strUnitOfMeasure
			)
		{
			return GetPrice(
				intCompany,
				string.Empty,
				strWharehouse,
				strItemNumber,
				intQuantity,
				strCustomerNumber,
				strOrderType,
				strCurrency,
				strPricelist,
				string.Empty,
				dtDate,
				strUnitOfMeasure);
		}

		public static string GetPriceLine(
			int			intCompany,
			string		strFacility,
			string		strCustomerNumber,
			string		strItemNumber,
			string		strWharehouse,
			DateTime	dtDate,
			int			intQuantity,
			string		strUnitOfMeasure,
			string		strCurrency,
			string		strOrderType,
			string		strPricelist
			)
		{
			MvxSckCommandBuilder objCb = new MvxSckCommandBuilder("GetPriceLine");
			objCb.Add(intCompany, 3);
			objCb.Add(strFacility, 3);
			objCb.Add(strCustomerNumber, 10);
			objCb.Add(strItemNumber, 15);
			objCb.Add(strWharehouse, 3);
			objCb.Add(dtDate);
			objCb.Add(intQuantity, 16);
			objCb.Add(strUnitOfMeasure, 3);
			objCb.Add(strCurrency, 3);
			objCb.Add(strOrderType, 3);
			objCb.Add(strPricelist, 2);

			return objCb.Command;
		}

		public struct GetPriceMLineItem
		{
			public string	ItemNumber;
			public string	Wharehouse;
			public int		Quantity;
			public string	UnitOfMeasure;

			public GetPriceMLineItem(string strItemNumber, string strWharehouse, int intQuantity, string strUnitOfMeasure)
			{
				this.ItemNumber		= strItemNumber;
				this.Wharehouse		= strWharehouse;
				this.Quantity		= intQuantity;
				this.UnitOfMeasure	= strUnitOfMeasure;
			}
		}

		public static string GetPriceMLine(
			int			intCompany,
			string		strFacility,
			string		strCustomerNumber,
			DateTime	dtOrderDate,
			string		strCurrency,
			string		strOrderType,
			string		strPricelist,
			string		strDiscountModel,
			DateTime	dtDate,
			GetPriceMLineItem[] objItems
			)
		{
			if (objItems.Length == 0)
				throw new ApplicationException("No items specified.");

			if (objItems.Length > 39)
				throw new ApplicationException("Too many items specified. Only 39 is allowed.");

			MvxSckCommandBuilder objCb = new MvxSckCommandBuilder("GetPriceMLine");
			objCb.Add(intCompany, 3);
			objCb.Add(strFacility, 3);
			objCb.Add(strCustomerNumber, 10);
			objCb.Add(dtOrderDate);
			objCb.Add(strCurrency, 3);
			objCb.Add(strOrderType, 3);
			objCb.Add(strPricelist, 2);
			objCb.Add(strDiscountModel, 10);
			objCb.Add(dtDate);

			for (int i = 0; i < objItems.Length && i < 39; i++)
			{
				objCb.Add(objItems[i].ItemNumber.ToUpper(), 15);
				objCb.Add(objItems[i].Wharehouse, 3);
				objCb.Add(objItems[i].Quantity, 16);
				objCb.Add(objItems[i].UnitOfMeasure, 3);
			}

			return objCb.Command;
		}

		public static string GetLastOrderErrorMessage(
			int			intCompany,
			string		strTemporaryOrderNumber
			)
		{
			MvxSckCommandBuilder objCb = new MvxSckCommandBuilder("LstErrMsgOrder");
			objCb.Add(intCompany, 3);
			objCb.Add(strTemporaryOrderNumber, 7);

			return objCb.Command;
		}
	}
}
