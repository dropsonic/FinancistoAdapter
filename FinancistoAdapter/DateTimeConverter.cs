using System;

namespace FinancistoAdapter;

public class DateTimeConverter : CustomConverter
{
	protected override object PerformConversion(string value)
	{
		double timestamp = double.Parse(value);
			
		if (timestamp == 0)
		{
			return null;
		}
			
		return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddMilliseconds(timestamp);
	}
}