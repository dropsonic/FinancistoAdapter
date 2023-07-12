using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using FinancistoAdapter;
using FinancistoAdapter.Entities;

string fileName = null;
string outputFileName;
string arg = args.Length > 0 && args[0] != null ? args[0] : Environment.CurrentDirectory;
if (arg != null)
{
	fileName = File.GetAttributes(arg).HasFlag(FileAttributes.Directory) ? Directory.EnumerateFiles(arg, "*.backup").MaxBy(Path.GetFileNameWithoutExtension) : arg;
}

if (String.IsNullOrEmpty(fileName) || !File.Exists(fileName))
	Environment.Exit(-1);
if (args.Length > 1 && args[1] != null)
	outputFileName = args[1];
else
	outputFileName = Path.ChangeExtension(fileName, "csv");

var entities = RecordReader.GetRecords(fileName).ToArray();

var transactions =
	entities
		.OfType<Transaction>()
		.Where(t => t.DateTime >= new DateTime(2015, 12, 1))
		.Where(t => t.To == null)
		.Where(t => t.Category != Category.Split)
		.OrderBy(t => t.DateTime)
		.ToArray();
// ReSharper disable UnusedVariable
var accounts =
	entities
		.OfType<Account>()
		.ToArray();
var currencies =
	entities
		.OfType<Currency>()
		.ToArray();
var payees =
	entities
		.OfType<Payee>()
		.ToArray();
var categories =
	entities
		.OfType<Category>()
		.ToArray();
var projects =
	entities
		.OfType<Project>()
		.ToArray();
var attributes =
	entities
		.OfType<AttributeDefinition>()
		.ToArray();
// ReSharper restore UnusedVariable
var attributeValues =
	entities
		.OfType<TransactionAttribute>()
		.Where(a => a.Attribute != null) // workaround for inconsistent data
		.ToArray();

// "За всех" attribute
var sharedExpenseAttrs = attributeValues.Where(a => String.Equals(a.Attribute.Title, "За всех", StringComparison.OrdinalIgnoreCase))
	.Select(a => new {a.Transaction, Value = bool.Parse(a.Value ?? "false")})
	.GroupBy(a => a.Transaction, a => a.Value);
var sharedExpenseMap = new Dictionary<Transaction, bool>();

foreach (var item in sharedExpenseAttrs)
{
	bool value = false;
	foreach (var v in item)
	{
		value |= v;
	}

	sharedExpenseMap[item.Key] = value;
}

using (FileStream file = File.Create(outputFileName))
{
	using (StreamWriter writer = new StreamWriter(file, Encoding.UTF8))
	{
		using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
		{
			csv.Context.TypeConverterOptionsCache.GetOptions<DateTime>().Formats = new [] { "yyyy-MM-dd HH:mm:ss" };

			//csv.WriteExcelSeparator();
			csv.WriteField("Date&Time");
			csv.WriteField("Account");
			csv.WriteField("Currency");
			csv.WriteField("Amount");
			csv.WriteField("Category");
			csv.WriteField("Payee");
			csv.WriteField("Project");
			csv.WriteField("Note");
			csv.WriteField("Shared Expense");
			csv.NextRecord();

			foreach (Transaction tran in transactions)
			{
				csv.WriteField(tran.DateTime);
				csv.WriteField(tran.From?.Title);
				csv.WriteField(tran.From?.Currency.Name);
				csv.WriteField(tran.FromAmount?.ToString("0.00"));
				csv.WriteField(tran.Category?.Title);
				csv.WriteField(tran.Payee?.Title);
				csv.WriteField(tran.Project?.Title);
				csv.WriteField(tran.Note);
				csv.WriteField(sharedExpenseMap.TryGetValue(tran, out bool value) && value);
				csv.NextRecord();
			}
		}
	}
}

Console.WriteLine("Done!");