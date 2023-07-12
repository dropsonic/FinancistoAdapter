using System;

namespace FinancistoAdapter
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class RecordPropertyAttribute : Attribute
	{
		private Type _converter = DefaultConverter;

		public string Key { get; private set; }

		public static Type DefaultConverter => typeof (DefaultConverter);

		public Type Converter
		{
			get => _converter;
			set
			{
				if (value != null && !typeof(IPropertyConverter).IsAssignableFrom(value))
					throw new ArgumentException("Converter type must implement IPropertyConverter.", nameof(value));
				_converter = value;
			}
		}

		public RecordPropertyAttribute(string key)
		{
			if (String.IsNullOrEmpty(key)) throw new ArgumentException("Key cannot be null or empty.", nameof(key));
			Key = key;
		}
	}
}
