using System.Diagnostics;

namespace FinancistoAdapter.Entities
{
	[Record("attributes")]
	public class AttributeDefinition : Entity
	{
		[RecordProperty("default_value")]
		public string DefaultValue { get; set; }
	}
}
