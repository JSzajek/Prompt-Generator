using System;
using System.ComponentModel;

/// <summary>
/// The enum class representing the prompt categories.
/// </summary>
public enum PromptCategory
{
    [Description("Attack Method")]
    AttackMethod,

    [Description("Physical Feature")]
    PhysicalFeature,

    [Description("Habitat")]
    Habitat,

    [Description("Elemental Trait")]
    ElementalTrait,

    [Description("Characteristic")]
    Characteristics
} 