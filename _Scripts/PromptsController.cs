using Godot;
using System.Collections.Generic;

/// <summary>
/// Controller class controlling all functionality related
/// to the primary canvas ui.
/// </summary>
public class PromptsController : Control
{
	#region Fields

	private PackedScene promptInstance;
	private PackedScene labelInstance;
	private HBoxContainer container;
	private PromptCategoriesController promptCategoriesController;
	private CategoriesController categoriesController;
	private VBoxContainer labelContainer;
	private Timer timer;
	private Color topColor = Color.Color8(175, 175, 175, 190);
	private Label timerLabel;
	private Button pausePlayButton;
	
	#endregion Fields

	#region Constructors

	public override void _Ready()
	{
        categoriesController = this.GetRoot().Get<CategoriesController>("Main/Generator/Categories");
		timer = new Timer();
		timer.Connect("timeout", this, "OnTimerTimeout");
		AddChild(timer);

		promptInstance = ResourceLoader.Load("res://_Assets/Prompt.tscn") as PackedScene;
		labelInstance = ResourceLoader.Load("res://_Assets/Label.tscn") as PackedScene;
		promptCategoriesController = Navigator.PromptCategoriesController;
		container = this.Get<HBoxContainer>("ScrollContainer/Control/Container/HBoxContainer");
		labelContainer = this.Get<VBoxContainer>("ScrollContainer/Control/Labels/VBoxContainer");
		timerLabel = this.Get<Label>("Timer/Label");
		
		this.Get<Button>("Regenerate").Connect("pressed", this, "OnRegeneratePressed");
		this.Get<Button>("Reset").Connect("pressed", this, "OnResetPressed");
		pausePlayButton = this.Get<Button>("Timer/PausePlay");
		pausePlayButton.Connect("pressed", this, "OnPausePlayPressed");
		this.Get<Button>("Timer/Plus").Connect("pressed", this, "OnPlusPressed");
	}

	#endregion Constructors

	#region Public Methods
	
	/// <summary>
	/// Process method called every frame
	/// </summary>
	/// <param name="delta">Time delta</param>
	public override void _Process(float delta)
	{
		timerLabel.Text = "" + (int)timer.TimeLeft;
	}

	/// <summary>
	/// Generates the prompts from the passed collection of categories. Generating the passed number of prompts
	/// and adding timer when applicable.
	/// </summary>
	/// <param name="categories">The collection of categories to generate</param>
	/// <param name="numberOfPrompts">The number of prompts to generate</param>
	/// <param name="isTimed">Whether to time the generation</param>
	/// <param name="timeSeconds">The amount of time for the timed generation</param>
	public void GeneratePrompts(IEnumerable<PromptCategory> categories, int numberOfPrompts, bool isTimed, int timeSeconds)
	{
		this.Get<Control>("Timer").Visible = isTimed;
		if (isTimed)
		{
			timer.Start(timeSeconds);
		}

		// Generate the prompt categories labels
		var instance = labelInstance.Instance() as Label;
		var topLabel = instance.Duplicate(8) as Label;
		topLabel.Text = "Prompts";
		topLabel.GetFirstChild<ColorRect>().Color = topColor;
		labelContainer.AddChild(topLabel);
		foreach(var category in categories)
		{
			var temp = instance.Duplicate(8) as Label;
			temp.Text = category.ToFriendlyString() + ":";
			labelContainer.AddChild(temp);
		}

		var promptLabel = promptInstance.Instance() as Label;
		for(int i = 0; i < numberOfPrompts; i++)
		{
			var vBox = new VBoxContainer();
            vBox.Set("custom_constants/separation", 10);

            vBox.RectMinSize = new Vector2(150, 400);
			container.AddChild(vBox);
			var titleTemp = promptLabel.Duplicate(8) as Label;
			titleTemp.GetFirstChild<ColorRect>().Color = topColor;
			titleTemp.Text = "Prompt "+ container.GetChildCount();
			vBox.AddChild(titleTemp);
			foreach(var togg in categories)
			{
				var temp = promptLabel.Duplicate(8) as Label;
                temp.Text = promptCategoriesController.GetRandomPrompt(togg);
				vBox.AddChild(temp);
			}
		}
	}

	#endregion Public Methods

	#region Private Methods

	/// <summary>
	/// Executes the pause play command. Pausing or playing the timer.
	/// </summary>
	private void OnPausePlayPressed()
	{
		timer.Paused = !timer.Paused;
		pausePlayButton.Text = timer.Paused ? ">" : "||";
	}

	/// <summary>
	/// Executes the plus command. Adding ten seconds to the timer.
	/// </summary>
	private void OnPlusPressed()
	{
		var newTime = timer.TimeLeft + 10;
		timer.Stop();		
		timer.Start(newTime);
	}

	/// <summary>
	/// Executes the regnerate command. Refreshing the prompts.
	/// </summary>
	private void OnRegeneratePressed()
	{
		ClearPrompts();
		categoriesController.GeneratePrompts();
	}

	/// <summary>
	/// Executes the reset command. Clearing and reverting to the categories selection.
	/// </summary>
	private void OnResetPressed()
	{
		ClearPrompts();
		timer?.Stop();
		this.Visible = false;
		categoriesController.Visible = true;
	}

	/// <summary>
	/// Executes the timer timeout command. Regenerating the prompts when the timer timesout.
	/// </summary>
	private void OnTimerTimeout()
	{
		OnRegeneratePressed();
	}

	/// <summary>
	/// Clears the prompts.
	/// </summary>
	private void ClearPrompts()
	{
		labelContainer.RemoveAllChildren<Label>();
		container.RemoveAllChildren<VBoxContainer>();
	}

	#endregion Private Methods
}
