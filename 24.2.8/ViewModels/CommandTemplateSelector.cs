namespace ComboBoxItemTemplate.ViewModels;

using Command = ObjectModel.Command;

public sealed class CommandTemplateSelector : DataTemplateSelector
{
    public DataTemplate Separator
    { 
        get; set; 
    }

    public DataTemplate OneLine
    {
        get; set;
    }

    public DataTemplate TwoLine
    {
        get; set;
    }

    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {
        if (item is SeparatorCommand)
        {
            return Separator;
        }
        if (item is TwoLineCommand)
        {
            return TwoLine;
        }
        if (item is Command)
        {
            return OneLine;
        }
        throw new ArgumentOutOfRangeException(nameof(item));
    }
}
