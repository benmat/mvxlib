/*
 *
 * MvxLib, an open source C# library used for communication with Intentia Movex.
 * http://mvxlib.sourceforge.net
 *
 * Copyright (C) 2005 - 2007  Mattias Bengtsson
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
	public class MvxSckResponse
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
			MvxSckResponseBuilder rb = new MvxSckResponseBuilder(returned);
			rb.GetString(69);
			double price = rb.GetDouble(10);
			rb.GetString(8);
			int stagger_quantity = rb.GetInt(3);

			return new PriceItem(price, stagger_quantity == 0 ? 1 : stagger_quantity, rb.ResponseValues[1] == "");
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
			MvxSckResponseBuilder rb = new MvxSckResponseBuilder(returned);
			rb.GetString(18);
			double price = rb.GetDouble(15);

			return new PriceLineItem(price, rb.ResponseValues[1] == "");
		}
	}
}
