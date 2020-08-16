using Godot;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// Controller class controlling the functionality
/// contained within the prompts page.
/// </summary>
public class PromptsPageController : Control
{
	#region Fields

	private SqLiteController sqlite;	

	private PackedScene promptOptionInstance;
	private VBoxContainer categories;
	private GridContainer prompts;
	private LineEdit addBar;
	private ColorRect addError;

	private Control prompt;

	private string currentCategory;

	#endregion Fields

	/// <summary>
	/// Initializes a new instance of the <see cref="PromptsPageController"/> class.
	/// </summary>
	public override void _Ready()
	{
		sqlite = Navigator.SqLiteController;

		promptOptionInstance = ResourceLoader.Load("res://_Assets/PromptOptions.tscn") as PackedScene;

		categories = this.Get<VBoxContainer>("CategoriesScroll/VBoxContainer");
		prompts = this.Get<GridContainer>("PromptsScroll/GridContainer");

		prompt = promptOptionInstance.Instance() as Control;


	 	addBar = this.Get<LineEdit>("AddBar");
		addError = addBar.Get<ColorRect>("ErrorColor");
		addBar.Connect("text_entered", this, "OnAddBarTextEntered");
		addBar.Connect("text_changed", this, "OnAddBarTextChanged");
		addBar.Get<Button>("AddButton").Connect("pressed", this, "OnAddButtonPressed");
		this.Get<LineEdit>("SearchBar").Connect("text_changed", this, "OnSearchBarTextChanged");
		this.Get<Button>("ResetButton").Connect("pressed", this, "OnResetButtonPressed");
		GenerateCategoryButtons();
	}

	/// <summary>
	/// Generates the category buttons based on the tables within the current
	/// sqlite database.
	/// </summary>
	private void GenerateCategoryButtons()
	{
		var dataReader = sqlite.ExecuteReader("SELECT name FROM sqlite_master WHERE type = \'table\'");

		var buttonGroup = new ButtonGroup();
		var minSize = new Vector2(145, 50);

		while (dataReader.Read())
		{
			var label = new Button();
			label.RectMinSize = minSize;
			label.Group = buttonGroup;
			label.ToggleMode = true;
			label.Text = dataReader.GetString(0).Replace('_', ' ');          
			categories.AddChild(label);
			label.Connect("toggled", this, "OnCategoryButtonToggled", new Godot.Collections.Array() {dataReader.GetString(0)});
		}
		categories.GetChildren<Button>().First().Pressed = true;
	}

	/// <summary>
	/// Adds prompt labels within the grid container based on the passed string label value.
	/// </summary>
	/// <param name="lableValue">The label value of the prompt</param>
	private void AddPrompt(string lableValue)
	{
		var temptPrompt = prompt.Duplicate(8) as Control;
		var value = lableValue;
		temptPrompt.GetFirstChild<Button>().Connect("pressed", this, "OnPromptDeletePressed", new Godot.Collections.Array() {temptPrompt, value});
		temptPrompt.GetFirstChild<Label>().Text = value;
		prompts.AddChild(temptPrompt);
	}

	private void RemoveData(string table, string value) => sqlite.ExecuteCommandNonQuery("DELETE FROM " + table + " WHERE Value = \'" + value + "\';");

	/// <summary>
	/// Inserts the passed data into the specified table.
	/// </summary>
	/// <param name="table">The character table to insert into</param>
	/// <param name="value">The value to insert into the table</param>
	private void InsertData(string table, string value) => sqlite.ExecuteCommandNonQuery("INSERT INTO " + table + " (Value) " + "VALUES (\"" + value + "\");");

	/// <summary>
	/// Executes the reset command. Reseting the current 
	/// </summary>
	private void OnResetButtonPressed()
	{
		// Forcefully resetting the table - should switch to only updating if there has been changes
		var dataReader = sqlite.ExecuteReader("SELECT name FROM sqlite_master WHERE type = \'table\'");
		var names = new List<string>();
		
		while (dataReader.Read())
		{
			names.Add(dataReader.GetString(0));
		}
		dataReader.Close();

		foreach(var name in names)
		{
			sqlite.ExecuteCommandNonQuery("DROP TABLE if exists \"" + name + "\";");
			sqlite.ExecuteCommandNonQuery("VACUUM;");
		}

		PromptCategoriesController.ResetToDefaultTable();

		categories.RemoveAllChildren<Button>();
		prompts.RemoveAllChildren<Control>();
		GenerateCategoryButtons();
		
	}

	/// <summary>
	/// Executes the text entered command. Adding the current text as
	/// a prompt into the currently selected category.
	/// </summary>
	/// <param name="newText">The text to convert to a prompt</param>
	private void OnAddBarTextEntered(string newText)
	{
		if(!sqlite.CheckForRow(currentCategory, newText))
		{
			InsertData(currentCategory, newText);
			AddPrompt(newText);
			addBar.Clear();
		}
		else
		{
			addError.Visible = true;
		}
	}

	/// <summary>
	/// Executes the text changed command. Checking whether the current
	/// category contains the string value and showing the error if it does.
	/// </summary>
	/// <param name="newText">The current additional prompt value</param>
	private void OnAddBarTextChanged(string newText)
	{
		addError.Visible = sqlite.CheckForRow(currentCategory, newText);
	}

	/// <summary>
	/// Executes the add command. Adding the current text value into the
	/// currently selected category as a new prompt.
	/// </summary>
	private void OnAddButtonPressed()
	{
		OnAddBarTextEntered(addBar.Text);
	}

	/// <summary>
	/// Executes the text changed command. Searching the category list for
	/// the currently passed string value. Limiting the displayed categories
	/// to matches.
	/// </summary>
	/// <param name="newText">The currently searching string value</param>
	private void OnSearchBarTextChanged(string newText)
	{

	}

	/// <summary>
	/// Executes the category button toggled command. Switching the displayed
	/// prompts to the toggled category.
	/// </summary>
	/// <param name="state">The toggled state</param>
	/// <param name="sender">The sender (table name)</param>
	private void OnCategoryButtonToggled(bool state, object sender)
	{
		if (state && sender is string tableName)
		{
			prompts.RemoveAllChildren<Control>();
			currentCategory = tableName;
			
			var dataReader = sqlite.ExecuteReader("SELECT * FROM " + tableName);
			while(dataReader.Read())
			{
				AddPrompt(dataReader.GetString(0));
			}
			dataReader.Close();
		}
	}

	/// <summary>
	/// Executes the prompts delete command. Removing the 
	/// prompt from the display and from the database table.
	/// </summary>
	/// <param name="prompt">The prompt ui control to delete</param>
	/// <param name="value">The prompt string value to remove from the database</param>
	private void OnPromptDeletePressed(object prompt, object value)
	{
		prompts.RemoveChild(prompt as Control);
		RemoveData(currentCategory, value as string);
	}
}
