using Godot;

/// <summary>
/// Static class containing functions and constants relevant across
/// the application.
/// </summary>
public static class BuildEnvironment
{
    // The current build state.
    public static bool IsDebugBuild => !Engine.EditorHint;
}