namespace FinancistoAdapter
{
	public class AmountConverter : CustomConverter
	{
		protected override object PerformConversion(string value)
		{
			double d = double.Parse(value);
			return d / 100;
		}
	}
}
