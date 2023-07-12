using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using FinancistoAdapter.Entities;

namespace FinancistoAdapter
{
	public static class RecordReader
	{
		private static IReadOnlyDictionary<string, RecordInfo> GetRecordTypes()
		{
			Type recordType = typeof(Record);
			Dictionary<string, RecordInfo> records = new Dictionary<string, RecordInfo>();
			IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(a => a.GetTypes())
				.Where(recordType.IsAssignableFrom);
			
			foreach (Type t in types)
			{
				RecordAttribute attr = t.GetCustomAttributes(typeof(RecordAttribute), true).Cast<RecordAttribute>().FirstOrDefault();
				if (attr != null)
				{
					RecordInfo info = new RecordInfo() { RecordType = t };
					records[attr.Name] = info;
					foreach (PropertyInfo p in t.GetProperties())
					{
						RecordPropertyAttribute propertyAttr = (RecordPropertyAttribute) p.GetCustomAttribute(typeof (RecordPropertyAttribute));
						if (propertyAttr != null)
						{
							RecordPropertyInfo pInfo = new RecordPropertyInfo(p)
							{
								Converter = (IPropertyConverter) Activator.CreateInstance(propertyAttr.Converter)
							};
							pInfo.Converter!.PropertyType = p.PropertyType;
							info.Properties[propertyAttr.Key] = pInfo;
						}
					}
				}
			}

			return new ReadOnlyDictionary<string, RecordInfo>(records);
		}

		public static IEnumerable<Record> GetRecords(string fileName)
		{
			using var reader = new BackupReader(fileName);
			
			List<Record> entities = new List<Record>();
			var map = new Dictionary<Type, Dictionary<int, Record>>();
			var foreignKeys = new List<Tuple<RecordPropertyInfo, Action<object>, int>>();

			var recordTypes = GetRecordTypes();
			Record record = null;
			RecordInfo recordInfo = null;

			foreach (Line line in reader.GetLines().Select(s => new Line(s)))
			{
				if (line.Key == "$ENTITY")
				{
					if (!String.IsNullOrEmpty(line.Value) && recordTypes.TryGetValue(line.Value, out recordInfo))
						record = (Record)Activator.CreateInstance(recordInfo.RecordType);
				}
				else if (line.Key == "$$" && record != null)
				{
					if (!map.ContainsKey(recordInfo!.RecordType))
						map[recordInfo.RecordType] = new Dictionary<int, Record>();
					
					map[recordInfo.RecordType][record.Id] = record;

					entities.Add(record);

					record = null;
				}
				else if (record != null && line.Value != null)
				{
					if (recordInfo!.Properties.TryGetValue(line.Key, out var property))
					{
						if (typeof(Record).IsAssignableFrom(property.PropertyType))
						{
							Record cRecord = record;
							foreignKeys.Add(Tuple.Create(property, new Action<object>(v => property.SetValue(cRecord, v)), int.Parse(line.Value)));
						}
						else
							property.SetValue(record, line.Value);
					}
				}
			}

			foreach (Tuple<RecordPropertyInfo, Action<object>, int> link in foreignKeys)
			{
				if (map.TryGetValue(link.Item1.PropertyType, out var mapById) &&
				    mapById.TryGetValue(link.Item3, out var linkedRecord))
				{
					link.Item2(linkedRecord);
				}
				else if (link.Item1.Converter.GetType() != RecordPropertyAttribute.DefaultConverter)
				{
					try
					{
						link.Item2(link.Item3); // try to pass original ID to the converter
					}
					catch (InvalidCastException)
					{
					}
				}
			}

			return entities;
		}
	}
}
