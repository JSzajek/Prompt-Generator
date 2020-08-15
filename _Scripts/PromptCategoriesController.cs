using System;
using System.IO;
using System.Linq;
using Godot;

/// <summary>
/// Controller class controlling all functionality to 
/// generate and retrieve prompts to and from the database.
/// </summary>
public class PromptCategoriesController : Node
{
	#region Constants

	private const string NONE = "\"NONE\"";
	private const string DefaultPromptsPath = "res://Default Prompts.prompt";

	#endregion Constants

	#region Fields

	private SqLiteController sqlite;	

	#endregion Fields

	#region Constructors

	/// <summary>
    /// Initializes a new instance of the <see cref="PromptCategoriesController"/> class.
    /// </summary>
	public override void _Ready()
	{
		sqlite = Navigator.SqLiteController;
		CheckDefaultTable();
	}

	#endregion Constructors
	
	#region Public Methods

	/// <summary>
	/// Retrieves a random prompt from the passed category.
	/// </summary>
	/// <param name="category">The category to get the prompt of.</param>
	/// <returns>The random prompt</returns>
	public string GetRandomPrompt(PromptCategory category)
	{
		var dataReader = sqlite.ExecuteReader("SELECT * FROM " + category.ToFriendlyString().Replace(' ', '_') 
												+ " ORDER BY RANDOM() LIMIT 1");
		var result = dataReader.Read() ? dataReader.GetString(0) : "";
		dataReader.Close();
		return result;
	}

	#endregion Public Methods

	#region Private Methods

	/// <summary>
	/// Checks for the default table of prompts and generates it if not found.
	/// </summary>
	private void CheckDefaultTable()
	{
		var filepath = FileSystem.EnsureFilePath(DefaultPromptsPath);

		using (var reader = new StreamReader(filepath))
		{
			string tableName = null;
			string line;
			while ((line = reader.ReadLine()) != null)
			{
				if (string.IsNullOrEmpty(line)) { continue; }

				if (line.Contains('#'))
				{
					tableName = line.Remove(0, 1).Replace(' ', '_');
					CreateDefaultTable(tableName);
					continue;
				}

				if (!sqlite.CheckForRow(tableName, line))
				{
					InsertData(tableName, line);
				}
			}
			reader.Close();
		}
	}

	/// <summary>
	/// Inserts the passed data into the specified table.
	/// </summary>
	/// <param name="table">The character table to insert into</param>
	/// <param name="value">The value to insert into the table</param>
	private void InsertData(string table, string value) => sqlite.ExecuteCommandNonQuery("INSERT INTO " + table + " (Value) " + "VALUES (\"" + value + "\");");

	/// <summary>
	/// Creates a default character table within the database for the passed character name
	/// </summary>
	/// <param name="character">The character name</param>
	private void CreateDefaultTable(string tableName) => sqlite.ExecuteCommandNonQuery("CREATE TABLE if not exists \"" + tableName + "\" (\"Value\"	TEXT);");

	#endregion Private Methods
}
