using System;
using System.Collections.Generic;

namespace FinancistoAdapter
{
	public class EntityInfo
	{
		public EntityInfo()
		{
			Properties = new Dictionary<string, EntityPropertyInfo>();
		}

		public Type EntityType { get; init; }
		public IDictionary<string, EntityPropertyInfo> Properties { get; }
	}
}