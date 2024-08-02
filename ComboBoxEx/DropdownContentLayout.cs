using Microsoft.Maui.Layouts;
using System.Diagnostics;

namespace ComboBoxEx;

/// <summary>
/// Provides a layout for measuring combobox dropdown items.
/// </summary>
/// <remarks>
/// This class is intended for use by <see cref="ComboBox.ContentLayout"/> and should
/// not be used for general layout purposes.
/// </remarks>
public sealed class DropdownContentLayout : Layout
{
    ComboBox _owner;

    public DropdownContentLayout()
    {
        Margin = Thickness.Zero;
        Padding = Thickness.Zero;
    }

    protected override ILayoutManager CreateLayoutManager()
    {
        return new DropdownContentLayoutManager(this);
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

/// <summary>
/// Provide a <see cref="LayoutManager"/> for <see cref="DropdownContentLayout"/>.
/// </summary>
sealed class DropdownContentLayoutManager : LayoutManager
{
    DropdownContentLayout _container;

    public DropdownContentLayoutManager(DropdownContentLayout layout) :
        base(layout)
    {
        _container = layout;
    }

    /// <summary>
    /// Measures the size of the dropdown content.
    /// </summary>
    /// <param name="widthConstraint"></param>
    /// <param name="heightConstraint"></param>
    /// <returns></returns>
    public override Size Measure(double widthConstraint, double heightConstraint)
    {
        ComboBox owner = _container.Owner;

        if (owner == null || owner.ItemsSource == null || owner.ItemTemplate == null)
        {
            return Size.Zero;
        }

        double measuredWidth = 0;
        double measuredHeight = 0;
        int maxItems = owner.MaxDropDownItems;

        try
        {
            double minimumItemHeight = double.MaxValue;
            IEnumerable<object> items = owner.ItemsSource as IEnumerable<object>;
            DataTemplate itemTemplate = owner.ItemTemplate;
            if (itemTemplate is DataTemplateSelector selector)
            {
                BindableLayout.SetItemTemplateSelector(_container, selector);
            }
            else
            {
                BindableLayout.SetItemTemplate(_container, itemTemplate);
            }
            BindableLayout.SetItemsSource(_container, items);

            #region Measure Items

            Thickness padding = Layout.Padding;
            int itemCount = 0;

            for (int x = 0; x < _container.Count; x++)
            {
                if (maxItems != 0 && itemCount >= maxItems)
                {
                    break;
                }

                IView child = _container.Children[x];
                if (child == null || child.Visibility == Visibility.Collapsed)
                {
                    continue;
                }

                Size measured = child.Measure(double.PositiveInfinity, double.PositiveInfinity);
                minimumItemHeight = Math.Min(minimumItemHeight, measured.Height);
                Thickness margin = child.Margin;

                measuredWidth = Math.Max
                (
                    measuredWidth,
                    measured.Width + margin.HorizontalThickness
                );
                measuredHeight += measured.Height;
                itemCount++;
            }
            measuredWidth += padding.HorizontalThickness;
            measuredHeight += padding.VerticalThickness;

            // Remove padding before the first and after the last item.
            measuredHeight += (itemCount - 1) * owner.ItemPadding.VerticalThickness;

            #endregion Measure Items

            #region Adjust for Header an Footer

            double adjust = 0;

            // ISSUE: When a header or footer is present, there is a gap after the last item.
            // 1: Footer: A gap appears between the last item and the footer.
            // 2: Header and No Footer: A gap between the last item and the bottom of the dropdown.
            // TODO: Determine if this is by design.

            if (owner.ShowDropdownHeaderView)
            {
                adjust += owner.DropdownHeaderViewHeight;
            }
            if (owner.ShowDropdownFooterView)
            {
                adjust += owner.DropdownFooterViewHeight;
            }

            // ISSUE: SfComboBox 'appears' to add spacing after the last item 
            // when there is no footer or header which causes a small amount of scrolling.
            if (adjust == 0)
            {
                // Adjust the height to prevent the scrolling.
                adjust += 2;
            }

            measuredHeight += adjust;

            #endregion Adjust for Header an Footer
        }
        finally
        {
            // clear the items.
            BindableLayout.SetItemsSource(_container, null);
            BindableLayout.SetItemTemplate(_container, null);
        }

        Trace.WriteLine($"DropdownContentLayoutManager.Measure: {measuredWidth} x {measuredHeight}");
        return new(measuredWidth, measuredHeight);
    }

    /// <summary>
    /// Arranges child items.
    /// </summary>
    /// <param name="bounds"></param>
    /// <returns>The method does nothing and returns the size of the <paramref name="bounds"/>.</returns>
    public override Size ArrangeChildren(Rect bounds)
    {
        // Do nothing, 
        return bounds.Size;
    }
}
