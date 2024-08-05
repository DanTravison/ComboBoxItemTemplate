using Microsoft.Maui.Layouts;

namespace ComboBoxEx;

/// <summary>
/// Provides a layout for measuring combobox dropdown items.
/// </summary>
/// <remarks>
/// This class is intended for use by <see cref="ComboBox.ContentLayout"/> and should
/// not be used for general layout purposes.
/// </remarks>
public sealed class ComboBoxDropdownLayout : Layout
{
    ComboBox _owner;

    /// <summary>
    /// Initializes a new instance of this class.
    /// </summary>
    public ComboBoxDropdownLayout()
    {
        Margin = Thickness.Zero;
        Padding = Thickness.Zero;
    }

    /// <summary>
    /// Creates the associated <see cref="ILayoutManager"/>.
    /// </summary>
    /// <returns>A new instance of a <see cref="ILayoutManager"/>.</returns>
    protected override ILayoutManager CreateLayoutManager()
    {
        return new ComboBoxDropdownLayoutManager(this);
    }

    #region Owner

    /// <summary>
    /// Gets or sets the owning <see cref="ComboBox"/>.
    /// </summary>
    internal ComboBox Owner
    {
        get => _owner;
        set => _owner = value;
    }

    #endregion Owner
}
