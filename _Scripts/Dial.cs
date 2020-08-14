using Godot;

/// <summary>
/// Custom control class representing a larger spin box dial
/// </summary>
public class Dial : Control
{
    #region Fields
    
    private Label label;
    private int _value;

    #endregion Fields

    #region Properties

    /// <summary>
    /// Gets or sets the max value of the dial.
    /// </summary>
    [Export]
    public int MaxValue  {get; set;} = 100;

    /// <summary>
    /// Gets or sets the min value of the dial.
    /// </summary>
    [Export]
    public int MinValue  {get; set;} = 0;

    /// <summary>
    /// Gets or sets the value of the dial.
    /// </summary>
    public int Value 
    {
        get => _value; 
        set {
            _value = value;
            label.Text = Value.ToString();
        }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes the parameters
    /// </summary>
    public override void _Ready()
    {
        label = this.Get<Label>("Value");
        Value = MinValue;

        this.Get<Button>("Up").Connect("pressed", this, "OnUpPressed");
        this.Get<Button>("Down").Connect("pressed", this, "OnDownPressed");
    }

    #endregion Constructors

    #region Private Methods

    /// <summary>
    /// Down button pressed signal catch
    /// </summary>
    private void OnDownPressed() {
        Value = Mathf.Max(MinValue, Value - 1);
    }

    /// <summary>
    /// Up button pressed signal catch
    /// </summary>
    private void OnUpPressed() {
        Value = Mathf.Min(Value + 1, MaxValue);
    }

    #endregion Private Methods
}
