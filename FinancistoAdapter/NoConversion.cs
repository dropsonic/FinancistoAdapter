using System;

namespace FinancistoAdapter;

public class NoConversion : IPropertyConverter
{
	public Type PropertyType { get; set; }

	public object Convert(object value)
	{
		return value;
	}
}