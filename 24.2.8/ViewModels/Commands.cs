namespace ComboBoxItemTemplate.ViewModels;

using System.Windows.Input;
using Command = ObjectModel.Command;

public class SeparatorCommand : Command
{
    public SeparatorCommand() 
        : base(NoAction, string.Empty, string.Empty, false)
    {
    }
}

public class TwoLineCommand : Command
{
    public TwoLineCommand(Action<ICommand> action, string text, string description)
        : base(action, text, description)
    {
    }
}

