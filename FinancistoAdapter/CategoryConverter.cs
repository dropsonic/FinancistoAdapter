using System;
using FinancistoAdapter.Entities;

namespace FinancistoAdapter;

public class CategoryConverter : IPropertyConverter
{
	public Type PropertyType { get; set; }

	public object Convert(object value)
	{
		if (Equals(value, -1))
			return Category.Split;
		if (value is Entity) 
			return value;
		return null;
	}
}