namespace ComboBoxEx;

using Microsoft.Maui.Controls;
using Syncfusion.Maui.Inputs;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Runtime.CompilerServices;

/// <summary>
/// Provides an <see cref="SfComboBox"/> with the ability to automatically set
/// the dropdown height based on the items in the dropdown list.
/// </summary>
public class ComboBox : SfComboBox
{
    #region Fields

    double _comboBoxContentsHeight;
    INotifyCollectionChanged _itemsSource;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of this class.
    /// </summary>
    public ComboBox()
    {
        if (AutoFitContent)
        {
            this.SetIsAutoFit(AutoFitContent);
        }
    }

    #region Event Handlers

    protected override void OnPropertyChanging(string propertyName)
    {
        if (propertyName == IsDropDownOpenProperty.PropertyName)
        {
            // If the dropdown is opening and the content height needs to be calculated.
            if (_comboBoxContentsHeight == 0 && !IsDropDownOpen && ContentLayout != null)
            {
                _comboBoxContentsHeight = MeasureDropdownContents(out double dropdownItemHeight).Height;
                Trace.WriteLine($"ComboBox: MaxDropdownHeight = {_comboBoxContentsHeight}");
                MaxDropDownHeight = _comboBoxContentsHeight;
                DropDownItemHeight = dropdownItemHeight;
            }
        }
        base.OnPropertyChanging(propertyName);
    }

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        if (propertyName == ItemsSourceProperty.PropertyName)
        {
            if (_itemsSource != null)
            {
                _itemsSource.CollectionChanged -= OnItemsSourceCollectionChanged;
            }
            _itemsSource = ItemsSource as INotifyCollectionChanged;
            if (_itemsSource != null)
            {
                _itemsSource.CollectionChanged += OnItemsSourceCollectionChanged;
            }
            _comboBoxContentsHeight = 0;
        }
        else if (propertyName == ItemTemplateProperty.PropertyName)
        {
            _comboBoxContentsHeight = 0;
        }
        else if (propertyName == IsDropDownOpenProperty.PropertyName)
        {
            if (IsDropDownOpen)
            {
                Trace.WriteLine($"{IsDropDownOpen}");
            }
        }
    }

    private void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        _comboBoxContentsHeight = 0;
    }

    #endregion Event Handlers

    #region AutoFitContent

    /// <summary>
    /// Provides access to the SfComboBox.IsAutoFit internal property.
    /// </summary>
    public bool AutoFitContent
    {
        get => (bool)GetValue(AutoFitContentProperty);
        set => SetValue(AutoFitContentProperty, value);
    }

    /// <summary>
    /// Provides the <see cref="BindableProperty"/> for <see cref="AutoFitContent"/>.
    /// </summary>
    public static readonly BindableProperty AutoFitContentProperty = BindableProperty.Create
    (
        nameof(AutoFitContent),
        typeof(bool),
        typeof(ComboBox),
        (bool)false,
        BindingMode.OneWay,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is ComboBox comboBox)
            {
                comboBox.SetIsAutoFit((bool)newValue);
            }
        }
    );

    #endregion AutoFitContent

    #region MaxDropDownItems

    /// <summary>
    /// Defines the maximum number of items to display in the dropdown.
    /// </summary>
    /// <value>
    /// The maximum number of items to display in the dropdown; otherwise, 
    /// zero to not constrain the number of items displayed.
    /// </value>
    public int MaxDropDownItems
    {
        get => (int)GetValue(MaxDropDownItemsProperty);
        set => SetValue(MaxDropDownItemsProperty, value);
    }

    /// <summary>
    /// Provides the <see cref="BindableProperty"/> for <see cref="MaxDropDownItems"/>.
    /// </summary>
    public static readonly BindableProperty MaxDropDownItemsProperty = BindableProperty.Create
    (
        nameof(MaxDropDownItems),
        typeof(int),
        typeof(ComboBox),
        (int)0,
        BindingMode.OneWay,
        coerceValue: (bindable, value) =>
        {
            if ((int)value < 0)
            {
                value = 0;
            }
            return value;
        }
    );

    #endregion MaxDropDownItems

    #region ContentLayout

    /// <summary>
    /// Provides a <see cref="Layout"/> class for measuring the dropdown contents.
    /// </summary>
    /// <remarks>
    /// The consumer should set a layout class that can be used to measure the dropdown contents.
    /// The typical use case is to use a <see cref="VerticalStackLayout"/>
    /// with margin, spacing and padding set to zero.
    /// </remarks>
    public ComboBoxDropdownLayout ContentLayout
    {
        get => GetValue(ContentLayoutProperty) as ComboBoxDropdownLayout;
        set => SetValue(ContentLayoutProperty, value);
    }

    /// <summary>
    /// Provides the <see cref="BindableProperty"/> for <see cref="ContentLayout"/>.
    /// </summary>
    public static readonly BindableProperty ContentLayoutProperty = BindableProperty.Create
    (
        nameof(ContentLayout),
        typeof(ComboBoxDropdownLayout),
        typeof(ComboBox),
        null,
        BindingMode.OneWay,
        propertyChanging: (bindable, oldValue, newValue) =>
        {
            if (bindable is ComboBox comboBox)
            {
                comboBox._comboBoxContentsHeight = 0;
            }
        }
    );

    #endregion ContentLayout

    #region Content Measurement

    Size MeasureDropdownContents(out double dropdownItemHeight)
    {
        if (ContentLayout != null && ItemsSource != null && ItemTemplate != null)
        {
            try
            {
                // NOTE: Setting owner on demand to avoid layout outside of IsDropdownOpen
                ContentLayout.Owner = this;
                Size size = ContentLayout.Measure(double.PositiveInfinity, double.PositiveInfinity);
                dropdownItemHeight = ContentLayout.DropDownItemHeight;
                return size;
            }
            finally
            {
                ContentLayout.Owner = null;
            }
        }
        dropdownItemHeight = 0;
        return Size.Zero;
    }

    #endregion Content Measurement
}
