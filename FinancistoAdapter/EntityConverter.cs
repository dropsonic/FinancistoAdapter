using System;
using FinancistoAdapter.Entities;

namespace FinancistoAdapter
{
	public class EntityConverter : IPropertyConverter
	{
		public Type PropertyType { get; set; }

		public object Convert(object value)
		{
			if (value is Entity) 
				return value;
			return null;
		}
	}
}
