## Autosizing SfComboBox to fit the contents.
Demonstrates a derived SfComboBox control that illustrates calculating 
SfComboBox.MaxDropDownHeight based on the contents of the ComboBox versus using
a hard-code pixel value.

The goal is to avoid the use of the pixel-based MaxDropDownHeight property and use the 
contents of the ComboBox to determine the size of the dropdown.

## ComboBox.ContentLayout
Ideally, this property would be boolean and named AutoSizeDropdownContent (or something similar).
The expected behavior is to size the dropdown based it's contents.

Since there is no visiblity to the underlying Dropdown layout, the ComboBox.ContentLayout uses a custom DropdownContentLayout class to calculate the size of the dropdown.

The logic requires SfComboBox.ItemsSource and SfComboBox.ItemTemplate and the internal IsAutoFit is set to true.

Size calculation is requested by ComboBox within PropertyChanging for SfComboBox.IsDropdownOpen by calling ComboBox.MeasureDropdownContents. 

The return value is cached and used to set SfComboBox.MaxDropDownHeight.

ComboBox invalidates the cached value if ItemsSource or ItemTemplate changes or if ItemsSource implements INotifyCollectionChanged and the collection changes. 

## ComboBox.MaxDropdownItems
This property provides the capability to limit the maximum size of the ComboBox using an item count versus a hard-code pixel value.

This logically replaces MaxDropDownHeight with an item count instead of a pixel value. It is assumed that SfComboBox would constrain the dropdown height based on screen size.

## DropdownContentLayout
- DropdownContentLayout contains the logic for calculating the size of the dropdown. It is tightly coupled to ComboBox and not intended as a reusable layout class.
- The DropdownContentLayout must to be added to the visual tree to ensure all bindings are resolved. The sample does this in MainPage.xaml.
- Unlike the prvious use of VerticalStackLayout, DropdownContentLayout can be shared across multiple ComboBox instances.
- Sharing is only tested when sharing within the same page.
- DropdownContentLayout.Owner should only be set for the duration of Measure. This ensures calls to Measure and Arrange outside ComboBox's usage do nothing.