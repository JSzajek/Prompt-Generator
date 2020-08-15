using Godot;
using System;
using System.Linq;

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
		this.Get<Button>("SearchBar/SearchButton").Connect("pressed", this, "OnSearchButtonPressed");
		GenerateCategoryButtons();
	}

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

	private void OnAddBarTextChanged(string newText)
	{
		addError.Visible = false;
	}

	private void OnAddButtonPressed()
	{
		OnAddBarTextEntered(addBar.Text);
	}

	private void OnSearchBarTextChanged(string newText)
	{

	}

	private void OnSearchButtonPressed()
	{

	}

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

	private void AddPrompt(string lableValue)
	{
		var temptPrompt = prompt.Duplicate(8) as Control;
		var value = lableValue;
		temptPrompt.GetFirstChild<Button>().Connect("pressed", this, "OnPromptDeletePressed", new Godot.Collections.Array() {temptPrompt, value});
		temptPrompt.GetFirstChild<Label>().Text = value;
		prompts.AddChild(temptPrompt);
	}

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

	private void OnPromptDeletePressed(object prompt, object value)
	{
		prompts.RemoveChild(prompt as Control);
		RemoveData(currentCategory, value as string);
	}

	private void RemoveData(string table, string value) => sqlite.ExecuteCommandNonQuery("DELETE FROM " + table + " WHERE Value = \'" + value + "\';");

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
}
