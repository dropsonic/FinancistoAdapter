using System;

namespace FinancistoAdapter;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class RecordAttribute : Attribute
{
	public string Name { get; private set; }
	public RecordAttribute(string name)
	{
		if (String.IsNullOrEmpty(name)) 
			throw new ArgumentException("Record name cannot be null or empty.", nameof(name));
			
		Name = name;
	}
}