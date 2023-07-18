using System;

namespace Win8.Core.Tasks.Attributes
{
    /// <summary>
    /// Attributes marking properties in the ViewModel that should not be logged and reported.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SensitiveAttribute : Attribute
    {
    }
}
