using System.Diagnostics;

namespace FinancistoAdapter.Entities
{
	[DebuggerDisplay("{Title}")]
	[Entity("currency")]
	public class Currency : Entity
	{
		[EntityProperty("name")]
		public string Name { get; set; }
		[EntityProperty("title")]
		public string Title { get; set; }
	}
}
