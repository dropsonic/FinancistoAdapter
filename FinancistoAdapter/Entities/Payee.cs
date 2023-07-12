using System.Diagnostics;

namespace FinancistoAdapter.Entities
{
	[DebuggerDisplay("{Title}")]
	[Entity("payee")]
	public class Payee : Entity
	{
		[EntityProperty("title")]
		public string Title { get; set; }
	}
}
