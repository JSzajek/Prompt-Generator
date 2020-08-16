using Godot;

/// <summary>
/// Controller class controlling all functionality related
/// to the primary canvas ui.
/// </summary>
public class CanvasController : Control
{
	#region Constructors

	/// <summary>
    /// Initializes a new instance of the <see cref="CanvasController"/> class.
    /// </summary>
	public override void _Ready()
	{
		this.Get<Button>("CanvasLayer/BackButton").Connect("pressed", this, "OnBackButtonPressed");
	}

	#endregion Constructors
	
	#region Properties

	/// <summary>
    /// Gets or sets the previous scene path.
    /// </summary>
	[Export]
	public string PreviousScene {get; set;}

	#endregion Properties

	#region Public Methods

	/// <summary>
	/// Cleans up the signal connections.
	/// </summary>
	public override void _ExitTree()
	{
		this.Get<Button>("CanvasLayer/BackButton").Disconnect("pressed", this, "OnBackButtonPressed");
	}

	#endregion Public Methods

	#region Private Methods

	/// <summary>
	/// Executes the back button command, returning to the previous scene.
	/// </summary>
	private void OnBackButtonPressed()
	{
		Navigator.SceneController.GotoScene(PreviousScene);
	}

	#endregion Private Methods
}