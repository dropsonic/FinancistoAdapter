namespace FinancistoAdapter.Entities
{
	[Record("currency")]
	public class Currency : Entity
	{
		[RecordProperty("name")]
		public string Name { get; set; }
		
		[RecordProperty("symbol")]
		public string Symbol { get; set; }
		
		[RecordProperty("is_default")]
		public bool IsDefault { get; set; }
		
		[RecordProperty("decimals")]
		public int Decimals { get; set; }
		
		[RecordProperty("decimal_separator")]
		public string DecimalSeparator { get; set; }
		
		[RecordProperty("group_separator")]
		public string GroupSeparator { get; set; }
	}
}
