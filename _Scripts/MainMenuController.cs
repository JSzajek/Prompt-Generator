using Godot;
using System;

/// <summary>
/// Controller class controlling all functionality related
/// to the main menu ui.
/// </summary>
public class MainMenuController : Control
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="MainMenuController"/> class.
    /// </summary>
    public override void _Ready()
    {
        this.Get<Button>("Start").Connect("pressed", this, "OnStartPressed");
        this.Get<Button>("Options").Connect("pressed", this, "OnOptionsPressed");
        this.Get<Button>("Quit").Connect("pressed", this, "OnQuitPressed");
    }

    #endregion Constructors

    #region Public Methods

    /// <summary>
	/// Cleans up the signal connections.
	/// </summary>
	public override void _ExitTree()
	{
		this.Get<Button>("Start").Disconnect("pressed", this, "OnStartPressed");
        this.Get<Button>("Options").Disconnect("pressed", this, "OnOptionsPressed");
        this.Get<Button>("Quit").Disconnect("pressed", this, "OnQuitPressed");
	}

    #endregion Public Methods

    /// <summary>
	/// Executes start command.
	/// </summary>
    private void OnStartPressed()
    {
		Navigator.SceneController.GotoScene("res://_Scenes/PromptGenerator.tscn");
    }

    /// <summary>
	/// Executes the options command.
	/// </summary>
	private void OnOptionsPressed() {
		// To Do - Implement
	}

    /// <summary>
	/// Executes the quit application command.
	/// </summary>
	private void OnQuitPressed() {
		GetTree().Quit();
	}
}
