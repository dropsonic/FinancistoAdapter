using System.Diagnostics;

namespace FinancistoAdapter.Entities
{
	[DebuggerDisplay("{Title}")]
	[Entity("attributes")]
	public class AttributeDefinition : Entity
	{
		[EntityProperty("title")]
		public string Title { get; set; }

		[EntityProperty("default_value")]
		public string DefaultValue { get; set; }
	}
}
