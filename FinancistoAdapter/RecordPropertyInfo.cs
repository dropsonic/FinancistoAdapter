using System;
using System.Reflection;
using FinancistoAdapter.Entities;

namespace FinancistoAdapter;

public class RecordPropertyInfo
{
	private delegate void SetValueDelegate(IRecord record, object value);

	private readonly SetValueDelegate _delegate;

	public RecordPropertyInfo(PropertyInfo info)
	{
		PropertyName = info.Name;
		PropertyType = info.PropertyType;
		_delegate = info.SetValue;
	}

	public string PropertyName { get; private set; }

	public Type PropertyType { get; private set; }

	public void SetValue(IRecord record, object value)
	{
		object v = Converter.Convert(value);
		_delegate(record, v);
	}

	public IPropertyConverter Converter { get; set; }
}