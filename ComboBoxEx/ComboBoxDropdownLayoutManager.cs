namespace ComboBoxEx;

using Microsoft.Maui.Layouts;
using System.Diagnostics;


/// <summary>
/// Provide a <see cref="LayoutManager"/> for <see cref="ComboBoxDropdownLayout"/>.
/// </summary>
sealed class ComboBoxDropdownLayoutManager : LayoutManager
{
    ComboBoxDropdownLayout _container;

    public ComboBoxDropdownLayoutManager(ComboBoxDropdownLayout layout) :
        base(layout)
    {
        _container = layout;
    }

    /// <summary>
    /// Measures the size of the dropdown content.
    /// </summary>
    /// <param name="widthConstraint">The width constraint.</param>
    /// <param name="heightConstraint">The height constraint.</param>
    /// <returns>The required <see cref="Size"/> for the content.</returns>
    public override Size Measure(double widthConstraint, double heightConstraint)
    {
        ComboBox owner = _container.Owner;

        if (owner == null || owner.ItemsSource == null || owner.ItemTemplate == null)
        {
            return Size.Zero;
        }

        double measuredWidth = 0;
        double measuredHeight = 0;

        // ISSUE: When a header or a footer is present
        // the height is too large resulting in an extra item being
        // visible.
        // Its not clear if this is an issue with this layout
        // or SfComboBox but no workaround has been found.
        int maxItems = owner.MaxDropDownItems;

        bool showFooter = owner.ShowDropdownFooterView;
        bool showHeader = owner.ShowDropdownHeaderView;

        try
        {
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

            if (showHeader)
            {
                adjust += owner.DropdownHeaderViewHeight;
            }
            if (showFooter)
            {
                adjust += owner.DropdownFooterViewHeight;
            }

            // ISSUE: SfComboBox 'appears' to add spacing after the last item
            // when there is no footer or header which causes a small amount of scrolling.
            if (!showHeader && !showFooter)
            {
                // Adjust the height to avoid the scrolling.
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
