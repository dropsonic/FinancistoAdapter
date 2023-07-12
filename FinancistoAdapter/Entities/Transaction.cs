using System;

namespace FinancistoAdapter.Entities
{
	[Record("transactions")]
	public class Transaction : Record
	{
		[RecordProperty("from_account_id")]
		public Account From { get; set; }
		
		[RecordProperty("to_account_id")]
		public Account To { get; set; }
		
		[RecordProperty("category_id", Converter = typeof(CategoryConverter))]
		public Category Category { get; set; }
		
		[RecordProperty("note")]
		public string Note { get; set; }
		
		[RecordProperty("datetime", Converter = typeof(DateTimeConverter))]
		public DateTime? DateTime { get; set; }
		
		[RecordProperty("from_amount", Converter = typeof (AmountConverter))]
		public double? FromAmount { get; set; }
		
		// Amount in the original currency that may not match the currency of the account
		[RecordProperty("original_from_amount", Converter = typeof (AmountConverter))]
		public double? OriginalFromAmount { get; set; }
		
		[RecordProperty("original_currency_id")]
		public Currency OriginalCurrency { get; set; }
		
		[RecordProperty("to_amount", Converter = typeof(AmountConverter))]
		public double? ToAmount { get; set; }
		
		[RecordProperty("payee_id")]
		public Payee Payee { get; set; }
		
		[RecordProperty("project_id")]
		public Project Project { get; set; }
		
		[RecordProperty("parent_id")]
		public Transaction Parent { get; set; }
	}
}
