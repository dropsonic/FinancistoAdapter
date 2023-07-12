using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace FinancistoAdapter;

public class Line
{
	public Line(string rawLine)
	{
		if (!String.IsNullOrEmpty(rawLine))
		{
			string[] split = rawLine.Split(new[] {':'}, 2);
			Key = split[0];
			if (split.Length > 1) 
				Value = split[1];
		}
	}

	public Line(string key, string value)
	{
		Key = key;
		Value = value;
	}

	public string Key { get; set; }
	public string Value { get; set; }
}

public class BackupReader : IDisposable
{
	private readonly FileStream _file;
	private readonly GZipStream _zipStream;
	private readonly TextReader _reader;
	private bool _readToEnd;

	private string _package;
	private int _versionCode;
	private Version _version;
	private int _dbVersion;

	public string Package => _package;
	public int VersionCode => _versionCode;
	public Version Version => _version;
	public int DatabaseVersion => _dbVersion;

	public BackupReader(string fileName)
	{
		if (String.IsNullOrEmpty(fileName)) throw new ArgumentException("File name cannot be null or empty.", nameof(fileName));
		_file = File.OpenRead(fileName);
		_zipStream = new GZipStream(_file, CompressionMode.Decompress);
		_reader = new StreamReader(_zipStream);

		ReadHeader();
	}

	private void ReadHeader()
	{
		while (_reader.ReadLine() is { } rawLine && !String.Equals(rawLine, "#START"))
		{
			Line line = new Line(rawLine);
			switch (line.Key)
			{
				case "PACKAGE": 
					_package = line.Value; 
					break;
				case "VERSION_CODE":
					_versionCode = int.Parse(line.Value);
					break;
				case "VERSION_NAME":
					_version = Version.Parse(line.Value);
					break;
				case "DATABASE_VERSION":
					_dbVersion = int.Parse(line.Value);
					break;
			}
		}
	}

	public IEnumerable<string> GetLines()
	{
		if (_readToEnd)
			throw new InvalidOperationException("The backup has already been read to end.");

		while (_reader.ReadLine() is { } line && line != "#END")
		{
			if (!String.IsNullOrEmpty(line))
				yield return line;
		}

		_readToEnd = true;
	}

	public void Dispose()
	{
		_reader.Dispose();
		_zipStream.Dispose();
		_file.Dispose();
	}
}