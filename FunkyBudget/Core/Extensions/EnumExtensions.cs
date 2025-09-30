using System.ComponentModel;
using System.Reflection;
using System.Windows.Markup;

namespace FunkyBudget.Core.Extensions;

public static class EnumExtensions
{
    public static string GetDescription(this Enum enumValue)
    {
        FieldInfo field = enumValue.GetType().GetField(enumValue.ToString());

        if (field == null)
            return enumValue.ToString(); // Fallback if no description

        DescriptionAttribute attribute = field.GetCustomAttribute<DescriptionAttribute>();

        return attribute == null ? enumValue.ToString() : attribute.Description;
    }

    public static T GetEnumValueFromDescription<T>(string description) where T : Enum
    {
        foreach (var field in typeof(T).GetFields())
        {
            if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
            {
                if (attribute.Description == description)
                    return (T)field.GetValue(null);
            }
            else if (field.Name == description)
                return (T)field.GetValue(null);
        }

        throw new ArgumentException($"No enum value found with the description: {description}");
    }
}

public class EnumToCollectionExtension : MarkupExtension
{
    public Type EnumType { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        if (EnumType is null)
            throw new ArgumentNullException(nameof(EnumType));

        return Enum.GetValues(EnumType).Cast<Enum>().Select(EnumToDescriptionOrString);
    }

    private string EnumToDescriptionOrString(Enum value)
        => value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false).Cast<DescriptionAttribute>().FirstOrDefault()?.Description
            ?? value.ToString();
}
