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
			GD.Print("opened the sqlite connection");
		}
		catch (Exception exception)
		{
			GD.Print(exception.ToString());
		}
		return sqlite_conn;
	}
	
	#endregion Private Methods
}
