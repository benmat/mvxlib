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
	public class MvxSckReturn
	{
		public struct PriceItem
		{
			public double	Price;
			public int		StaggerQuantity;
			public bool		EmptyPrice;

			public PriceItem(
				double	price,
				int		stagger_quantity,
				bool	empty_price
				)
			{
				this.Price				= price;
				this.StaggerQuantity	= stagger_quantity;
				this.EmptyPrice			= empty_price;
			}
		}

		public static PriceItem GetPriceItem(string returned)
		{
			MvxSckReturnBuilder rb = new MvxSckReturnBuilder(returned);
			rb.GetString(69);
			double price = rb.GetDouble(10);
			rb.GetString(8);
			int stagger_quantity = rb.GetInt(3);

			return new PriceItem(price, stagger_quantity == 0 ? 1 : stagger_quantity, rb.ReturnValues[1] == "");
		}

		public struct PriceLineItem
		{
			public double	Price;
			public bool		EmptyPrice;

			public PriceLineItem(
				double	price,
				bool	empty_price
				)
			{
				this.Price		= price;
				this.EmptyPrice	= empty_price;
			}
		}

		public static PriceLineItem GetPriceLineItem(string returned)
		{
			MvxSckReturnBuilder rb = new MvxSckReturnBuilder(returned);
			rb.GetString(18);
			double price = rb.GetDouble(15);

			return new PriceLineItem(price, rb.ReturnValues[1] == "");
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
				string	item,
				double	sales_price,
				bool	empty_price,
				double	line_amount,
				double	order_quantity,
				string	pricelist,
				bool	scaled_pricelist,
				bool	error
				)
			{
				this.ItemNumber			= item;
				this.Salesprice			= sales_price;
				this.EmptyPrice			= empty_price;
				this.LineAmount			= line_amount;
				this.OrderQuantity		= order_quantity;
				this.Pricelist			= pricelist;
				this.ScaledPricelist	= scaled_pricelist;
				this.Error				= error;
			}
		}

		public static PriceMLineItem[] GetPriceMLineItems(string returned)
		{
			ArrayList ret = new ArrayList();
			MvxSckReturnBuilder rb = new MvxSckReturnBuilder(returned.Trim());
			rb.GetString(15); // Return code

			while (rb.CommandStringLeft != "FINITO" && rb.CommandStringLeft.Length > 0)
			{
				string item		= rb.GetString(15);
				bool empty_price		= rb.CommandStringLeft.Substring(0, 15).Trim() == "";
				double price			= rb.GetDouble(15);
				double line_amount		= rb.GetDouble(15);
				double order_quantity	= rb.GetDouble(15);
				string pricelist		= rb.GetString(2);
				bool scaled_pricelist	= rb.GetBool();
				bool error				= rb.GetBool();

				ret.Add(new PriceMLineItem(
					item,
					price,
					empty_price,
					line_amount,
					order_quantity,
					pricelist,
					scaled_pricelist,
					error
					));
			}

			return (PriceMLineItem[]) ret.ToArray(typeof(PriceMLineItem));
		}
	}
}
