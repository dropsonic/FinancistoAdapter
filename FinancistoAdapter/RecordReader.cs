using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using FinancistoAdapter.Entities;

namespace FinancistoAdapter;

public static class RecordReader
{
	private static IReadOnlyDictionary<string, RecordInfo> GetRecordTypes()
	{
		Type recordType = typeof(IRecord);
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

	public static IEnumerable<IRecord> GetRecords(string fileName)
	{
		using var reader = new BackupReader(fileName);
			
		List<IRecord> entities = new();
		var map = new Dictionary<Type, Dictionary<int, IRecord>>();
		var foreignKeys = new List<(RecordPropertyInfo PropertyInfo, Action<object> SetValue, int Value)>();

		var recordTypes = GetRecordTypes();
		IRecord record = null;
		RecordInfo recordInfo = null;

		foreach (Line line in reader.GetLines().Select(s => new Line(s)))
		{
			if (line.Key == "$ENTITY")
			{
				if (!String.IsNullOrEmpty(line.Value) && recordTypes.TryGetValue(line.Value, out recordInfo))
					record = (IRecord) Activator.CreateInstance(recordInfo.RecordType);
			}
			else if (line.Key == "$$" && record != null)
			{
				if (record is Record identifiableRecord)
				{
					if (!map.ContainsKey(recordInfo!.RecordType))
						map[recordInfo.RecordType] = new Dictionary<int, IRecord>();

					map[recordInfo.RecordType][identifiableRecord.Id] = record;
				}

				entities.Add(record);

				record = null;
			}
			else if (record != null && line.Value != null)
			{
				if (recordInfo!.Properties.TryGetValue(line.Key, out var property))
				{
					if (typeof(IRecord).IsAssignableFrom(property.PropertyType))
					{
						IRecord cRecord = record;
						foreignKeys.Add((property, v => property.SetValue(cRecord, v), int.Parse(line.Value)));
					}
					else
						property.SetValue(record, line.Value);
				}
			}
		}

		foreach (var link in foreignKeys)
		{
			if (map.TryGetValue(link.PropertyInfo.PropertyType, out var mapById) &&
			    mapById.TryGetValue(link.Value, out var linkedRecord))
			{
				link.SetValue(linkedRecord);
			}
			else if (link.PropertyInfo.Converter.GetType() != RecordPropertyAttribute.DefaultConverter)
			{
				try
				{
					link.SetValue(link.Value); // try to pass original ID to the converter
				}
				catch (InvalidCastException)
				{
				}
			}
		}

		return entities;
	}
}