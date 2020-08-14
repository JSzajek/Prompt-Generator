using System.ComponentModel;

/// <summary>
/// Extension class for Prompt Category class.
/// </summary>
public static class PromptCategoryExtension
{
    /// <summary>
    /// Converts the prompt category into a friendly string.
    /// </summary>
    /// <param name="category"></param>
    /// <returns>The friendly string of the prompt category</returns>
    public static string ToFriendlyString(this PromptCategory category)
    {
        DescriptionAttribute[] attributes = (DescriptionAttribute[])category
           .GetType()
           .GetField(category.ToString())
           .GetCustomAttributes(typeof(DescriptionAttribute), false);
        return attributes.Length > 0 ? attributes[0].Description : string.Empty;
    }
}