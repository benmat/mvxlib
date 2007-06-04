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

#if NET_2_0
// Disable warnings of missing XML comments in this class.
#pragma warning disable 1591,1592,1573,1571,1570,1572
#endif

namespace MvxLib
{
	public class MvxSckCommand
	{
		#region Prices
		public static string GetPrice(
			int			company,
			string		division,
			string		warehouse,
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
			cb.Add(warehouse, 3);
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
			string		warehouse,
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
				warehouse,
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
			string		warehouse,
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
			cb.Add(warehouse, 3);
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
			public string	Warehouse;
			public double	Quantity;
			public string	UnitOfMeasure;

			public GetPriceMLineItem(string item, string warehouse, double quantity, string unit_of_measure)
			{
				this.ItemNumber		= item;
				this.Warehouse		= warehouse;
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
				cb.Add(items[i].Warehouse, 3);
				cb.Add(items[i].Quantity, 16);
				cb.Add(items[i].UnitOfMeasure, 3);
			}

			return cb.Command;
		}
		#endregion

		#region Orders
		public static string AddBatchHead(
			int			company,
			string		customer_no,
			string		language,
			string		order_type,
			DateTime	requested_delivery,
			string		facility,
			string		customer_order_no,
			string		agent,
			string		salesman,
			string		delivery_method,
			string		delivery_term,
			string		customer_reference,
			string		project,
			string		order_discount_amount,
			string		payment_terms,
			string		payer,
			string		delivery_address,
			string		currency,
			string		our_reference,
			string		goods_mark
			)
		{
			MvxSckCommandBuilder cb = new MvxSckCommandBuilder("AddBatchHead");
			cb.Add(company, 3);
			cb.Add(customer_no, 10);
			cb.Add(language, 2);
			cb.Add(order_type, 3);
			if (requested_delivery == DateTime.MinValue)
				cb.Add(string.Empty, 10);
			else
				cb.Add(requested_delivery);
			cb.Add(facility, 3);
			cb.Add(customer_order_no, 20);
			cb.Add(agent, 10);
			cb.Add(salesman, 4);
			cb.Add(delivery_method, 3);
			cb.Add(delivery_term, 3);
			cb.Add(customer_reference, 30);
			cb.Add(project, 6);
			cb.Add(order_discount_amount, 16);
			cb.Add(payment_terms, 3);
			cb.Add(payer, 10);
			cb.Add(delivery_address, 6);
			cb.Add(currency, 3);
			cb.Add(our_reference, 30);
			/*
			// According to documentation
			cb.Add(string.Empty, 10);	// quotation_no
			cb.Add(string.Empty, 36);	// delivery_terms

			// According to MRS001
			cb.Add(string.Empty, 107);	// filler
			cb.Add(string.Empty, 3);	// contact_method
			*/
			// Workaround
			cb.Add(string.Empty, 292);	// unknown param(s)
			cb.Add(goods_mark, 30);
			return cb.Command;
		}

		public static string AddBatchText(
			int			company,
			string		temp_order_no,
			int			text_location,
			int			order_line,
			int			text_type,
			string		document_class,
			string		text
			)
		{
			MvxSckCommandBuilder cb = new MvxSckCommandBuilder("AddBatchText");
			cb.Add(company, 3);
			cb.Add(temp_order_no, 7);
			cb.Add(text_location, 1);
			cb.Add(order_line, 3);
			cb.Add(string.Empty, 2);	// line suffix
			cb.Add(text_type, 1);
			cb.Add(document_class, 4);
			cb.Add(text, 240);
			return cb.Command;
		}

		public static string AddBatchAddress(
			int			company,
			string		temp_order_no,
			int			address_type,
			string		address_id,
			string		customer_name,
			string		address_line1,
			string		address_line2,
			string		address_line3,
			string		address_line4,
			string		postal_code,
			string		country_code
			)
		{
			MvxSckCommandBuilder cb = new MvxSckCommandBuilder("AddBatchAddress");
			cb.Add(company, 3);
			cb.Add(temp_order_no, 7);
			cb.Add(address_type, 2);
			cb.Add(address_id, 6);
			cb.Add(customer_name, 36);
			cb.Add(address_line1, 36);
			cb.Add(address_line2, 36);
			cb.Add(address_line3, 36);
			cb.Add(address_line4, 36);
			cb.Add(postal_code, 10);
			cb.Add(string.Empty, 10);	// phone no
			cb.Add(string.Empty, 10);	// fax no
			cb.Add(country_code, 3);
			cb.Add(string.Empty, 16);	// vat
			cb.Add(string.Empty, 3);	// place of load
			cb.Add(string.Empty, 6);	// route
			cb.Add(string.Empty, 3);	// route departure
			cb.Add(string.Empty, 5);	// unloading zone
			cb.Add(string.Empty, 3);	// tax code
			cb.Add(string.Empty, 2);	// area/state
			cb.Add(string.Empty, 6);	// harbor or airport
			cb.Add(string.Empty, 30);	// your reference 1
			return cb.Command;
		}

		public static string AddBatchLine(
			int			company,
			string		temp_order_no,
			string		item_no,
			double		quantity,
			string		warehouse,
			DateTime	requested_delivery,
			string		delivery_code,
			string		customer_order_no,
			double		sales_price,
			bool		sales_price_empty,
			string		delivery_specification,
			string		delivery_specification_text,
			string		configuration_no,
			string		unit,
			DateTime	confirmed_delivery,
			string		item_description
			)
		{
			MvxSckCommandBuilder cb = new MvxSckCommandBuilder("AddBatchLine");
			cb.Add(company, 3);
			cb.Add(temp_order_no, 7);
			cb.Add(item_no, 15);
			cb.Add(quantity, 16);
			cb.Add(warehouse, 3);
			if (requested_delivery == DateTime.MinValue)
				cb.Add(string.Empty, 10);
			else
				cb.Add(requested_delivery);
			cb.Add(delivery_code, 3);
			cb.Add(customer_order_no, 7);
			if (sales_price_empty)
				cb.Add(string.Empty, 18);
			else
				cb.Add(sales_price, 18);
			cb.Add(string.Empty, 16);	// discount amount 1
			cb.Add(string.Empty, 16);	// discount amount 2
			cb.Add(string.Empty, 16);	// discount amount 3
			cb.Add(string.Empty, 16);	// discount amount 4
			cb.Add(string.Empty, 16);	// discount amount 5
			cb.Add(string.Empty, 16);	// discount amount 6
			cb.Add(delivery_specification, 12);
			cb.Add(delivery_specification_text, 36);
			cb.Add(configuration_no, 7);
			cb.Add(string.Empty, 3);	// simulation no
			cb.Add(unit, 3);
			if (confirmed_delivery == DateTime.MinValue)
				cb.Add(string.Empty, 10);
			else
				cb.Add(confirmed_delivery);
			cb.Add(item_description, 30);
			return cb.Command;
		}

		public static string AddBatchHeadChg(
			int			company,
			string		temp_order_no,
			string		charge,
			double		charge_amount,
			double		recalc_factor,
			string		description
			)
		{
			MvxSckCommandBuilder cb = new MvxSckCommandBuilder("AddBatchHeadChg");
			cb.Add(company, 3);
			cb.Add(temp_order_no, 10);
			cb.Add(charge, 6);
			cb.Add(charge_amount, 16);
			cb.Add(recalc_factor, 12);
			cb.Add(description, 30);
			return cb.Command;
		}

		public static string AddBatchLineChg(
			int			company,
			string		temp_order_no,
			int			line_no,
			int			line_suffix,
			string		charge,
			double		charge_amount,
			double		recalc_factor,
			string		description
			)
		{
			MvxSckCommandBuilder cb = new MvxSckCommandBuilder("AddBatchLineChg");
			cb.Add(company, 3);
			cb.Add(temp_order_no, 10);
			cb.Add(line_no, 3);
			cb.Add(line_suffix, 2);
			cb.Add(charge, 6);
			cb.Add(charge_amount, 16);
			cb.Add(recalc_factor, 12);
			cb.Add(description, 30);
			return cb.Command;
		}

		public static string Confirm(
			int			company,
			string		temp_order_no,
			string		confirmation_type
			)
		{
			MvxSckCommandBuilder cb = new MvxSckCommandBuilder("Confirm");
			cb.Add(company, 3);
			cb.Add(temp_order_no, 7);
			cb.Add(confirmation_type, 1);
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
		#endregion
	}
}
