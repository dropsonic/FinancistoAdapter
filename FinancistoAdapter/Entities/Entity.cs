namespace FinancistoAdapter.Entities
{
	public abstract class Entity
	{
		[EntityProperty("_id")]
		public virtual int Id { get; set; }
	}
}
