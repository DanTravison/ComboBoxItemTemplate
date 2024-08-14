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
            // NOTE: DropdownItemHeight needs to be set to ensure DropDowmMaxHeight is honored.
            // It is a best guess as the items may have different heights.
            // If ComboBox.MaxDropdownItems is not set or all of the items are the same height,
            // it will be the required height; otherwise, it is simply an average
            // of the first MaxDropdownItems and may not be accurate.
            // A better approach might be to measure all items to get the average then set
            // the height to the average * MaxDropdownItems.
            _container.DropDownItemHeight = measuredHeight / itemCount;

            #endregion Measure Items

            #region Adjust for Header an Footer

            double adjust = 0;

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
