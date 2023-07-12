namespace FinancistoAdapter.Entities
{
	[Record("payee")]
	public class Payee : Entity
	{
		[RecordProperty("last_category_id")]
		public Category LastCategory { get; set; }
	}
}
