using System;

namespace FinancistoAdapter.Entities
{
	[Entity("transactions")]
	public class Transaction : Entity
	{
		[EntityProperty("from_account_id")]
		public Account From { get; set; }
		[EntityProperty("to_account_id")]
		public Account To { get; set; }
		[EntityProperty("category_id", Converter = typeof(CategoryConverter))]
		public Category Category { get; set; }
		[EntityProperty("note")]
		public string Note { get; set; }
		[EntityProperty("datetime", Converter = typeof(DateTimeConverter))]
		public DateTime? DateTime { get; set; }
		[EntityProperty("from_amount", Converter = typeof (AmountConverter))]
		public double? FromAmount { get; set; }
		// Amount in the original currency that may not match the currency of the account
		[EntityProperty("original_from_amount", Converter = typeof (AmountConverter))]
		public double? OriginalFromAmount { get; set; }
		[EntityProperty("original_currency_id")]
		public Currency OriginalCurrency { get; set; }
		[EntityProperty("to_amount", Converter = typeof(AmountConverter))]
		public double? ToAmount { get; set; }
		[EntityProperty("payee_id")]
		public Payee Payee { get; set; }
		[EntityProperty("project_id")]
		public Project Project { get; set; }
		[EntityProperty("parent_id")]
		public Transaction Parent { get; set; }
	}
}
