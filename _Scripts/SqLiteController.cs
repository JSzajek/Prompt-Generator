using System;
using System.Data.SQLite;
using Godot;

/// <summary>
/// Controller class in charge of sustaining the sqlite connection
/// and provides accessing methods.
/// </summary>
public class SqLiteController : Node
{
	#region Fields

	private SQLiteConnection connection;
	private SQLiteCommand command;

	#endregion Fields

	#region Public Methods

	/// <summary>
	/// Initializes a new instance of the <see cref="SqLiteController"/> class.
	/// </summary>
	public override void _Ready()
	{
		// Ensure that the SQLite.Interop.dll dependency is unpacked
		FileSystem.EnsureFilePath("res://SQLite.Interop.dll");
		connection = CreateConnection();
		command = connection.CreateCommand();
	}

	/// <summary>
	/// Performs memory management when exiting the application.
	/// </summary>
	public override void _ExitTree()
	{
		GD.Print("Closing database");
		connection.Close();
	}

	/// <summary>
	/// Executes a non query sqlite command.
	/// </summary>
	/// <param name="commandString">The command to execute</param>
	public void ExecuteCommandNonQuery(string commandString)
	{
		command.CommandText = commandString;
		command.ExecuteNonQuery();
	}

	/// <summary>
	/// Executes a reader sqlite command. Retrieving a data reader instance.
	/// </summary>
	/// <param name="commandString">The command to execute</param>
	/// <returns>The data reader instance</returns>
	public SQLiteDataReader ExecuteReader(string commandString)
	{
		var readerCommand = connection.CreateCommand();
		readerCommand.CommandText = commandString;
		return readerCommand.ExecuteReader();
	}

	
	/// <summary>
	/// Determines whether the row value exists within the table.
	/// </summary>
	/// <param name="table">The table to check</param>
	/// <param name="rowValue">The row value to check for</param>
	/// <returns>Whether to row exists</returns>
	public bool CheckForRow(string table, string rowValue)
	{
		var dataReader = ExecuteReader("SELECT EXISTS(SELECT 1 FROM " + table + " WHERE Value=\"" + rowValue + "\" LIMIT 1);");
		var result = dataReader.Read() ? dataReader.GetByte(0) == 1 : false;
		dataReader.Close();
		return result;
	}

	/// <summary>
	/// Determines whether the table exists within the table.
	/// </summary>
	/// <param name="tableName">The table to check for</param>
	/// <returns>Whether the table exists</returns>
	public bool CheckForTable(string tableName)
	{
		var dataReader = ExecuteReader("SELECT EXISTS(SELECT name FROM sqlite_master WHERE type = \'table\' AND name=\'" + tableName + "\' LIMIT 1);");
		var result = dataReader.Read() ? dataReader.GetByte(0) == 1 : false;
		dataReader.Close();
		return result;
	}

	#endregion Public Methods

	#region Private Methods

	/// <summary>
	/// Creates a connection to the conversation database.
	/// </summary>
	/// <returns>The sqlite database connection</returns>
	private SQLiteConnection CreateConnection()
	{
		var sqlite_conn = new SQLiteConnection("Data Source=database.db;Version=3;New=True;Compress=True;");
		try
		{
			sqlite_conn.Open();
		}
		catch (Exception exception)
		{
			GD.Print(exception.ToString());
		}
		return sqlite_conn;
	}

	#endregion Private Methods
}
