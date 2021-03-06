﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancistoAdapter
{
	public class DateTimeConverter : CustomConverter
	{
		protected override object PerformConvertion(string value)
		{
			double timestamp = double.Parse(value);
			return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddMilliseconds(timestamp);
		}
	}
}
