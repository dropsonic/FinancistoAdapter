namespace FinancistoAdapter.Entities;

[Record("category")]
public class Category : Entity
{
	private class SplitCategory : Category
	{
		public override int Id
		{
			get => -1;
			set { }
		}

		public override string Title 
		{ 
			get => "Split";
			set { } 
		}
	}

	public static Category Split { get; } = new SplitCategory();
		
	[RecordProperty("type")]
	public CategoryType Type { get; set; }
}

public enum CategoryType
{
	Expense = 0,
	Income = 1
}