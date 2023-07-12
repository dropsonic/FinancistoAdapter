using System;

namespace FinancistoAdapter.Entities
{
	[Record("account")]
	public class Account : Entity
	{
		[RecordProperty("creation_date", Converter = typeof (DateTimeConverter))]
		public DateTime CreationDate { get; set; }
		
		[RecordProperty("last_transaction_date", Converter = typeof(DateTimeConverter))]
		public DateTime? LastTransactionDate { get; set; }
		
		[RecordProperty("currency_id")]
		public Currency Currency { get; set; }
		
		[RecordProperty("type")]
		public string Type { get; set; }
		
		[RecordProperty("card_issuer")]
		public string CardIssuer { get; set; }
		
		[RecordProperty("issuer")]
		public string Issuer { get; set; }
		
		[RecordProperty("number")]
		public string Number { get; set; }
		
		[RecordProperty("total_amount", Converter = typeof (AmountConverter))]
		public double TotalAmount { get; set; }
		
		[RecordProperty("total_limit", Converter = typeof (AmountConverter))]
		public double LimitAmount { get; set; }
		
		[RecordProperty("sort_order")]
		public int SortOrder { get; set; }
		
		[RecordProperty("is_include_into_totals")]
		public bool IncludeIntoTotals { get; set; }
		
		[RecordProperty("last_account_id")]
		public Account LastAccount{ get; set; }
		
		[RecordProperty("last_category_id")]
		public Category LastCategory { get; set; }
		
		[RecordProperty("closing_day")]
		public int? ClosingDay { get; set; }
		
		[RecordProperty("payment_day")]
		public int? PaymentDay { get; set; }
		
		[RecordProperty("note")]
		public string Note { get; set; }
	}
}
