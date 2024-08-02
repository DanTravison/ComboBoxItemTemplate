namespace ComboBoxEx;

using Microsoft.Maui.Controls;
using Syncfusion.Maui.Inputs;
using System.Collections.Specialized;
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
            if (_comboBoxContentsHeight == 0 && !IsDropDownOpen && LayoutContainer != null)
            {
                _comboBoxContentsHeight = MeasureDropdownContents().Height;
                MaxDropDownHeight = _comboBoxContentsHeight;
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

    #region LayoutContainer

    /// <summary>
    /// Provides a <see cref="Layout"/> class for measuring the dropdown contents.
    /// </summary>
    /// <remarks>
    /// The consumer should set a layout class that can be used to measure the dropdown contents.
    /// The typical use case is to use a <see cref="VerticalStackLayout"/>
    /// with margin, spacing and padding set to zero.
    /// </remarks>
    public Layout LayoutContainer
    {
        get => GetValue(LayoutContainerProperty) as Layout;
        set => SetValue(LayoutContainerProperty, value);
    }

    /// <summary>
    /// Provides the <see cref="BindableProperty"/> for <see cref="LayoutContainer"/>.
    /// </summary>
    public static readonly BindableProperty LayoutContainerProperty = BindableProperty.Create
    (
        nameof(LayoutContainer),
        typeof(Layout),
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

    #endregion LayoutContainer

    #region Content Measurement

    Size MeasureDropdownContents()
    {
        IEnumerable<object> items = ItemsSource as IEnumerable<object>;
        DataTemplate itemsTemplate = ItemTemplate;
        if (LayoutContainer != null && items != null && itemsTemplate != null)
        {
            Thickness itemPadding = ItemPadding;

            if (ItemTemplate is DataTemplateSelector selector)
            {
                BindableLayout.SetItemTemplateSelector(LayoutContainer, selector);
            }
            else
            {
                BindableLayout.SetItemTemplate(LayoutContainer, ItemTemplate);
            }
            BindableLayout.SetItemsSource(LayoutContainer, items);

            SizeRequest size = LayoutContainer.Measure(double.PositiveInfinity, double.PositiveInfinity);
            int itemCount = LayoutContainer.Children.Count;

            BindableLayout.SetItemTemplate(LayoutContainer, null);
            BindableLayout.SetItemsSource(LayoutContainer, null);

            //
            // TODO: Get accurate information for calculating the height.
            // Currently, we're making a best guess based on the content.
            // This is not always accurate since we don't really know
            // how the dropdown arranges it's content.
            //

            // Assuming no spacing above the first item and below the last item.
            double height = size.Request.Height + (itemCount - 1) * itemPadding.VerticalThickness;
            double adjust = 0;
            if (ShowDropdownHeaderView)
            {
                adjust += DropdownHeaderViewHeight;
                // ISSUE: When a header is present, but no footer,
                // The dropdown has a gap at the bottom of the items.
                // The size 'appears' to be the same as the gap
                // between the last item and the footer when the footer
                // is present.
            }
            if (ShowDropdownFooterView)
            {
                // ISSUE: DropdownFooterViewHeight appears to be too large
                // leaving a gap between the last item and the footer.
                // TODO: Determine if this is by design.
                adjust += DropdownFooterViewHeight;
            }
            if (adjust == 0)
            {
                // ISSUE: SfComboBox appears to add a small gap after the last item
                // when there is no footer or header which causes a small amount of scrolling.
                // This is a best guess to avoid the gap.
                adjust = 2;
            }

            return new
            (
                size.Request.Width,
                height + adjust
            );
        }
        return Size.Zero;
    }

    #endregion Content Measurement
}
