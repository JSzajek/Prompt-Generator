using Godot;

/// <summary>
/// Singleton class representing a navigator holding connections
/// to all vital controller classes hosted within the application.
/// </summary>
public class Navigator : Node {

    #region Fields

    private static Navigator instance;

    #endregion Fields

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Navigator"/> class.
    /// </summary>
    public Navigator() {
        instance = this;
    }
    
    #endregion Constructors

    #region Public Properties

    /// <summary>
    /// Gets the SceneController instance of the application.
    /// </summary>
    public static SceneController SceneController => instance.Get<SceneController>("/root/SceneController");

    /// <summary>
    /// Gets the SqLiteController instance of the application.
    /// </summary>
    public static SqLiteController SqLiteController => instance.Get<SqLiteController>("/root/SqLiteController");

    /// <summary>
    /// Gets the PromptCategoriesController instance of the application.
    /// </summary>
    public static PromptCategoriesController PromptCategoriesController => instance.Get<PromptCategoriesController>("/root/PromptCategoriesController");

    #endregion Public Properties
}