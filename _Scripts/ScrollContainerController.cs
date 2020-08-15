using Godot;

/// <summary>
/// Custom behavior class for drag and drop scrolling
/// </summary>
public class ScrollContainerController : ScrollContainer
{
    #region Fields

    private bool swiping = false;
    private Vector2 swipeStart;
    private Vector2 swipeMouseStart;

    #endregion Fields

    #region Public Methods

    /// <summary>
	/// Input method called when input events are triggered.
	/// </summary>
	/// <param name="eventArgs">The event information</param>
    public override void _Input(InputEvent eventArgs) {
        if (eventArgs is InputEventMouseButton mouseButton ) 
        {
            if (mouseButton.Pressed) 
            {
                swiping = true;
                swipeStart = new Vector2(ScrollHorizontal, ScrollVertical);
                swipeMouseStart = mouseButton.Position;
            }
            else 
            {
                swiping = false;
            }
        }
        else if (swiping && eventArgs is InputEventMouseMotion motion) 
        {
            var delta = motion.Position - swipeMouseStart;
            ScrollHorizontal = (int)(swipeStart.x - delta.x);
            ScrollVertical = (int)(swipeStart.y - delta.y);
        }
    }

    #endregion Public Methods
}