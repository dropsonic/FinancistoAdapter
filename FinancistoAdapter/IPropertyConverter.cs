using System;

namespace FinancistoAdapter
{
	public interface IPropertyConverter
	{
		Type PropertyType { get; set; }
		object Convert(object value);
	}
}
