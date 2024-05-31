using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography.X509Certificates;

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
	private readonly Stream _stream;
	private GZipStream _zipStream;
	private TextReader _reader;
	private bool _readToEnd;

	private string _package;
	private int _versionCode;
	private string _version;
	private int _dbVersion;

	public string Package => _package;
	public int VersionCode => _versionCode;
	public string Version => _version;
	public int DatabaseVersion => _dbVersion;

	public BackupReader(Stream stream)
	{
		_stream = stream;
		Initialize();
		ReadHeader();
	}

	private void Initialize()
	{
		_reader?.Dispose();
		_zipStream?.Dispose();
		
		_zipStream = new GZipStream(_stream, CompressionMode.Decompress, leaveOpen: true);
		_reader = new StreamReader(_zipStream, leaveOpen: true);
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
					_version = line.Value;
					break;
				case "DATABASE_VERSION":
					_dbVersion = int.Parse(line.Value);
					break;
			}
		}
	}

	public IEnumerable<Line> ReadAll()
	{
		if (_readToEnd)
		{
			if (_stream.CanSeek)
			{
				_stream.Seek(0, SeekOrigin.Begin);
				Initialize();
				ReadHeader();
				_readToEnd = false;
			}
			else
			{
				throw new InvalidOperationException("The backup has already been read to end.");
			}
		}

		while (_reader.ReadLine() is { } line && line != "#END")
		{
			if (!String.IsNullOrEmpty(line))
				yield return new Line(line);
		}

		_readToEnd = true;
	}

	public void Dispose()
	{
		_reader.Dispose();
		_zipStream.Dispose();
	}
}