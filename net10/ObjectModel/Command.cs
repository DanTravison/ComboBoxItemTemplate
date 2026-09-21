using System.ComponentModel;
using System.Windows.Input;

namespace ObjectModel;

/// <summary>
/// Provides an <see cref="ICommand"/> base class.
/// </summary>
public class Command : ObservableObject, ICommand
{
    #region Static

    /// <summary>
    /// Provides an <see cref="Action{ICommand}"/> that does nothing.
    /// </summary>
    static public readonly Action<ICommand> NoAction = (ICommand) => { };

    #endregion Static

    #region Fields

    bool _isEnabled;
    string _text;
    string _description;
    readonly Action<ICommand> _action;

    #endregion Fields

    #region Constructors

    /// <summary>
    /// Initializes a new instance of this class.
    /// </summary>
    /// <param name="action">The action invoked by <see cref="Execute"/>.</param>
    /// <param name="text">The display text to associate with this instance.</param>
    /// <param name="description">The description of this instance.</param>
    /// <param name="isEnabled">Gets the value indicating if the command can be executed.</param>
    public Command(Action<ICommand> action, string text, string description, bool isEnabled = true)
    {
        ArgumentNullException.ThrowIfNull(action, nameof(action));
        _action = action;
        _text = text ?? string.Empty;
        _description = description ?? string.Empty;
        _isEnabled = isEnabled;
    }

    #endregion Constructors

    #region Properties

    /// <summary>
    /// Gets or sets the value indicating if the item is enabled.
    /// </summary>
    /// <value>
    /// true if the item is enabled; otherwise, false.
    /// </value>
    public bool IsEnabled
    {
        get
        {
            return _isEnabled;
        }
        set
        {
            if (_isEnabled != value)
            {
                _isEnabled = value;
                OnPropertyChanged(IsEnabledChangedEventArgs);
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    /// <summary>
    /// Gets the parameter that was passed to <see cref="Execute"/>
    /// </summary>
    /// <value>
    /// The parameter passed to <see cref="Execute"/>; otherwise, a null reference.
    /// </value>
    public object Parameter
    {
        get;
        protected set;
    }

    /// <summary>
    /// Gets or sets the display text for the command.
    /// </summary>
    public string Text
    {
        get => _text;
        set => SetProperty(ref _text, value, TextChangedEventArgs);
    }

    /// <summary>
    /// Gets or sets the description of the command.
    /// </summary>
    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value, DescriptionChangedEventArgs);
    }

    #endregion Properties

    #region Execute

    /// <summary>
    /// Occurs when changes occur that affect whether or not the command should execute. 
    /// </summary>
    public event EventHandler CanExecuteChanged;

    /// <summary>
    /// Defines the method that determines whether the command can execute in its current state
    /// </summary>
    /// <param name="parameter">not used.</param>
    /// <returns>true if this command can be executed; otherwise, false.</returns>
    public bool CanExecute(object parameter)
    {
        return IsEnabled;
    }

    /// <summary>
    /// Defines the method to be called when the command is invoked.
    /// </summary>
    /// <param name="parameter">Not used and can be set to null.</param>
    public void Execute(object parameter)
    {
        if (_isEnabled)
        {
            Parameter = parameter;
            try
            {
                OnExecute();
            }
            finally
            {
                Parameter = null;
            }
        }
    }

    /// <summary>
    /// Executes the associated action.
    /// </summary>
    /// <remarks>
    /// When overridden in the derived class, perform work before the action executes
    /// then call this method to perform the action.
    /// </remarks>
    protected virtual void OnExecute()
    {
        _action.Invoke(this);
    }

    #endregion Execute

    #region Cached PropertyChangedEventArgs

    /// <summary>
    /// The <see cref="PropertyChangedEventArgs"/> passed to <see cref="INotifyPropertyChanged.PropertyChanged"/> when <see cref="IsEnabled"/> changes.
    /// </summary>
    public static readonly PropertyChangedEventArgs IsEnabledChangedEventArgs = new(nameof(IsEnabled));

    /// <summary>
    /// The <see cref="PropertyChangedEventArgs"/> passed to <see cref="INotifyPropertyChanged.PropertyChanged"/> when <see cref="Text"/> changes.
    /// </summary>
    public static readonly PropertyChangedEventArgs TextChangedEventArgs = new(nameof(Text));

    /// <summary>
    /// The <see cref="PropertyChangedEventArgs"/> passed to <see cref="INotifyPropertyChanged.PropertyChanged"/> when <see cref="Description"/> changes.
    /// </summary>
    public static readonly PropertyChangedEventArgs DescriptionChangedEventArgs = new(nameof(Description));

    #endregion Cached PropertyChangedEventArgs
}
