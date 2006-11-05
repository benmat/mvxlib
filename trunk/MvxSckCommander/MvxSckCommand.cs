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
			int			company,
			string		division,
			string		wharehouse,
			string		item,
			int			quantity,
			string		customer,
			string		ordertype,
			string		currency,
			string		pricelist,
			string		discount_model,
			DateTime	date,
			string		unit_of_measure
			)
		{
			MvxSckCommandBuilder cb = new MvxSckCommandBuilder("GetPrice");
			cb.Add(company, 3);
			cb.Add(division, 3);
			cb.Add(wharehouse, 3);
			cb.Add(item, 15);
			cb.Add(quantity, 16);
			cb.Add(customer, 10);
			cb.Add(ordertype, 3);
			cb.Add(currency, 3);
			cb.Add(pricelist, 2);
			cb.Add(discount_model, 10);
			cb.Add(date);
			cb.Add(unit_of_measure, 3);

			return cb.Command;
		}

		public static string GetPrice(
			int			company,
			string		wharehouse,
			string		item,
			int			quantity,
			string		customer,
			string		ordertype,
			string		currency,
			string		pricelist,
			DateTime	date,
			string		unit_of_measure
			)
		{
			return GetPrice(
				company,
				string.Empty,
				wharehouse,
				item,
				quantity,
				customer,
				ordertype,
				currency,
				pricelist,
				string.Empty,
				date,
				unit_of_measure);
		}

		public static string GetPriceLine(
			int			company,
			string		facility,
			string		customer,
			string		item,
			string		wharehouse,
			DateTime	date,
			int			quantity,
			string		unit_of_measure,
			string		currency,
			string		ordertype,
			string		pricelist
			)
		{
			MvxSckCommandBuilder cb = new MvxSckCommandBuilder("GetPriceLine");
			cb.Add(company, 3);
			cb.Add(facility, 3);
			cb.Add(customer, 10);
			cb.Add(item, 15);
			cb.Add(wharehouse, 3);
			cb.Add(date);
			cb.Add(quantity, 16);
			cb.Add(unit_of_measure, 3);
			cb.Add(currency, 3);
			cb.Add(ordertype, 3);
			cb.Add(pricelist, 2);

			return cb.Command;
		}

		public struct GetPriceMLineItem
		{
			public string	ItemNumber;
			public string	Wharehouse;
			public double	Quantity;
			public string	UnitOfMeasure;

			public GetPriceMLineItem(string item, string wharehouse, double quantity, string unit_of_measure)
			{
				this.ItemNumber		= item;
				this.Wharehouse		= wharehouse;
				this.Quantity		= quantity;
				this.UnitOfMeasure	= unit_of_measure;
			}
		}

		public static string GetPriceMLine(
			int			company,
			string		facility,
			string		customer,
			DateTime	order_date,
			string		currency,
			string		ordertype,
			string		pricelist,
			string		discount_model,
			DateTime	date,
			GetPriceMLineItem[] items
			)
		{
			if (items.Length == 0)
				throw new ApplicationException("No items specified.");

			if (items.Length > 39)
				throw new ApplicationException("Too many items specified. Only 39 is allowed.");

			MvxSckCommandBuilder cb = new MvxSckCommandBuilder("GetPriceMLine");
			cb.Add(company, 3);
			cb.Add(facility, 3);
			cb.Add(customer, 10);
			cb.Add(order_date);
			cb.Add(currency, 3);
			cb.Add(ordertype, 3);
			cb.Add(pricelist, 2);
			cb.Add(discount_model, 10);
			cb.Add(date);

			for (int i = 0; i < items.Length && i < 39; i++)
			{
				cb.Add(items[i].ItemNumber.ToUpper(), 15);
				cb.Add(items[i].Wharehouse, 3);
				cb.Add(items[i].Quantity, 16);
				cb.Add(items[i].UnitOfMeasure, 3);
			}

			return cb.Command;
		}

		public static string GetLastOrderErrorMessage(
			int			company,
			string		temporary_order_number
			)
		{
			MvxSckCommandBuilder cb = new MvxSckCommandBuilder("LstErrMsgOrder");
			cb.Add(company, 3);
			cb.Add(temporary_order_number, 7);

			return cb.Command;
		}
	}
}
