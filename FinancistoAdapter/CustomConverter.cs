using System;

namespace FinancistoAdapter
{
	public abstract class CustomConverter : IPropertyConverter
	{
		public Type PropertyType { get; set; }

		protected abstract object PerformConversion(string value);

		public object Convert(object value)
		{
			string s = value as string;
			if (s == null) throw new NotSupportedException("Only string values are supported by this convertor.");
			return PerformConversion(s);
		}
	}
}
