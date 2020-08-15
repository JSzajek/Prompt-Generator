using Godot;

public class OptionsController : Control
{
    #region Fields

    private Control promptsPage;
    private Control othersPage;

    #endregion Fields

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="OptionsController"/> class.
    /// </summary>
    public override void _Ready()
    {
        promptsPage = this.Get<Control>("PromptsPage");
        othersPage = this.Get<Control>("OthersPage");

        this.Get<Button>("Prompts").Connect("toggled", this, "OnPromptsToggled");        
        this.Get<Button>("Others").Connect("toggled", this, "OnOthersToggled");
        
        promptsPage.Visible = true;
        othersPage.Visible = false;     
    }

    #endregion Constructors


    #region Public Methods

    public override void _ExitTree()
    {
        this.Get<Button>("Prompts").Disconnect("toggled", this, "OnPromptsToggled");        
        this.Get<Button>("Others").Disconnect("toggled", this, "OnOthersToggled");
    }

    #endregion Public Methods

    #region Private Methods

    private void OnPromptsToggled(bool state)
    {
        promptsPage.Visible = state;
        othersPage.Visible = !state;
    }

    private void OnOthersToggled(bool state)
    {
        promptsPage.Visible = !state;
        othersPage.Visible = state;
    }
    
    #endregion Private Methods
}
