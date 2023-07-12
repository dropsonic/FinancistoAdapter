using System;
using System.Collections.Generic;

namespace FinancistoAdapter;

public class RecordInfo
{
	public RecordInfo()
	{
		Properties = new Dictionary<string, RecordPropertyInfo>();
	}

	public Type RecordType { get; init; }
	public IDictionary<string, RecordPropertyInfo> Properties { get; }
}