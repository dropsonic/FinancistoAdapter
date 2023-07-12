using System;

namespace FinancistoAdapter
{
	public class DefaultConverter : IPropertyConverter
	{
		public object Convert(object value)
		{
			Type type = Nullable.GetUnderlyingType(PropertyType) ?? PropertyType;
			if (type == typeof (bool) && value is string str)
			{
				if (bool.TryParse(str, out var result))
					return result;
				
				if (int.TryParse(str, out var i))
					return System.Convert.ToBoolean(i);
			}
			return System.Convert.ChangeType(value, type);
		}

		public Type PropertyType { get; set; }
	}
}
