using System.Diagnostics;

namespace FinancistoAdapter.Entities;

[DebuggerDisplay("{Attribute.Title} = {Value} (TranId = {Transaction.Id})")]
[Record("transaction_attribute")]
public class TransactionAttribute : Record
{
	[RecordProperty("transaction_id")]
	public Transaction Transaction { get; set; }

	[RecordProperty("attribute_id")]
	public AttributeDefinition Attribute { get; set; }

	[RecordProperty("value")]
	public string Value { get; set; }
}