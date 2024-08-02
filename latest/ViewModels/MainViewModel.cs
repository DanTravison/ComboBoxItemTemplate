namespace ComboBoxItemTemplate.ViewModels;

using ObjectModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Command = ObjectModel.Command;

internal class MainViewModel : ObservableObject
{
    readonly ObservableCollection<Command> _commands = [];
    Command _selectedCommand;
    readonly Random _random = new();

    public MainViewModel()
    {
        Commands = new(_commands);
        RefreshCommand = new
        (
            OnRefresh,
            "Refresh",
            "Refresh the contents of the combobox."
        );

        PopulateCommands(10);
    }

    /// <summary>
    /// Gets the commands to display in the SfComboBox.
    /// </summary>
    public ReadOnlyObservableCollection<Command> Commands
    {
        get;
    }

    /// <summary>
    /// Gets or sets the selected <see cref="Command"/>.
    /// </summary>
    public Command SelectedCommand
    {
        get => _selectedCommand;
        set => SetProperty(ref _selectedCommand, value, ReferenceEqualityComparer.Instance, SelectedCommandChangedEventArgs);
    }

    /// <summary>
    /// Gets the command to randomly refresh the combobox contents.
    /// </summary>
    public Command RefreshCommand
    {
        get;
    }

    void OnRefresh(ICommand command)
    {
        PopulateCommands(_random.Next(5, 10));
    }

    /// <summary>
    /// Populate <see cref="Commands"/> with a random number of commands.
    /// </summary>
    /// <param name="count">The number of commands to populate.</param>
    void PopulateCommands(int count)
    {
        _commands.Clear();
        for (int x = 0; x < count; x++)
        {
            int item = x + 1;
            if (x == 3 || x == 5)
            {
                _commands.Add(new SeparatorCommand());
            }
            if ((x & 1) == 0)
            {
                _commands.Add(new Command(Command.NoAction, $"One Line {item} of {count}", $"Description {item} of {count}"));
            }
            else
            {
                _commands.Add(new TwoLineCommand(Command.NoAction, $"Two Line {item} of {count}", $"Description {item} of {count}"));
            }
        }
        SelectedCommand = null;
    }

    static readonly PropertyChangedEventArgs CommandsChangedEventArgs = new(nameof(Commands));
    static readonly PropertyChangedEventArgs SelectedCommandChangedEventArgs = new(nameof(SelectedCommand));
}
