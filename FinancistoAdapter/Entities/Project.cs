using System.Diagnostics;

namespace FinancistoAdapter.Entities
{
	[DebuggerDisplay("{Title}")]
	[Entity("project")]
	public class Project : Entity
	{
		[EntityProperty("title")]
		public string Title { get; set; }
	}
}
