using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// Controller class controlling all functionality to the
/// categories view.
/// </summary>
public class CategoriesController : Control
{
	#region Fields

	private GridContainer grid;
    private Button generateButton;
	private HashSet<PromptCategory> toggledCategories;
    private PromptsController promptsController;
	private CheckBox timedPromptsButton;
	private Control timer;

	#endregion Fields

	#region Constructors

	/// <summary>
    /// Initializes a new instance of the <see cref="CategoriesController"/> class.
    /// </summary>
	public override void _Ready()
	{
        promptsController = this.GetRoot().Get<PromptsController>("Main/Generator/Prompts");
		promptsController.Visible = false;
		toggledCategories = new HashSet<PromptCategory>();
		
		timer = this.Get<Control>("Settings/Timer");
		timer.Visible = false;
		grid = this.Get<GridContainer>("GridContainer");

        var font = ResourceLoader.Load("res://_GUI/_Imports/Normal_Font.res") as Font;

		Button button = null;
		var minSize = new Vector2(208, 50);
		foreach(var category in Enum.GetValues(typeof(PromptCategory)).Cast<PromptCategory>())
		{
			button = new Button() {ToggleMode = true};
			button.Text = category.ToFriendlyString();
			button.RectMinSize = minSize;
            button.ClipText = true;
            button.Set("custom_fonts/font", font);
			button.SetMeta("value", category);
			button.Connect("toggled", this, "OnButtonToggled", new Godot.Collections.Array() {button});
			grid.AddChild(button);
		}

        generateButton = this.Get<Button>("Generate");
		generateButton.Connect("pressed", this, "OnGeneratePressed");
        generateButton.Disabled = true;

		this.Get<CheckBox>("Settings/TimedPrompts/CheckButton").Connect("toggled", this, "OnTimedPromptsToggled");
	}

	#endregion Constructors

	#region Public Methods

	/// <summary>
	/// Generates the prompts of the toggled categories.
	/// </summary>
	public void GeneratePrompts()
	{
		var number = this.Get<Dial>("Settings/NumPrompts/Dial").Value;
        var isTimed = timer.Visible;
		var time = -1;
		if (isTimed)
		{
			time = timer.Get<Dial>("Minute").Value * 60;
			time += timer.Get<Dial>("Second").Value;
		}
        this.Visible = false;
        promptsController.Visible = true;
        promptsController?.GeneratePrompts(toggledCategories, number, isTimed, time);
	}

	#endregion Public Methods

	#region Private Methods
	
	/// <summary>
	/// Executes the button toggled eventhandler. Adding the prompt category
	/// connected to the button to the toggled collection.
	/// </summary>
	/// <param name="pressed">The state of the button.</param>
	/// <param name="obj">The button</param>
	private void OnButtonToggled(bool pressed, object obj)
	{
		if (obj is Button button)
		{
			if (pressed)
			{
				toggledCategories.Add((PromptCategory)button.GetMeta("value"));
			}
			else
			{
				toggledCategories.Remove((PromptCategory)button.GetMeta("value"));
			}
            generateButton.Disabled = toggledCategories.Count < 2;
		}
	}

	/// <summary>
	/// Executes the generate command.
	/// </summary>
	private void OnGeneratePressed() => GeneratePrompts();

	/// <summary>
	/// Executes the timed prompts toggled eventhandle.
	/// </summary>
	/// <param name="pressed">The state of the timed checkbutton</param>
	private void OnTimedPromptsToggled(bool pressed) => timer.Visible = pressed;

	#endregion Private Methods
}
