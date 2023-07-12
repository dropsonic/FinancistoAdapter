using System.Diagnostics;

namespace FinancistoAdapter.Entities
{
	[DebuggerDisplay("{Title}")]
	public abstract class Entity : Record
	{
		[RecordProperty("title")]
		public virtual string Title { get; set; }
		
		[RecordProperty("is_active")]
		public virtual bool IsActive { get; set; }
	}
}
