using System;
using FinancistoAdapter.Entities;

namespace FinancistoAdapter
{
	public class RecordConverter : IPropertyConverter
	{
		public Type PropertyType { get; set; }

		public object Convert(object value)
		{
			if (value is Record) 
				return value;
			return null;
		}
	}
}
