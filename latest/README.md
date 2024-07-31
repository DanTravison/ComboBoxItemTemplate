# Autosizing SfComboBox to fit the contents.

Demonstrates a derived SfComboBox control that illustrates calculating 
SfComboBox.MaxDropDownHeight to size to the contents of the ComboBox.

The auto-sizing logic requires SfComboBox.ItemsSource and SfComboBox.ItemTemplate to be set.
Additionally, the internal IsAutoFit is set to true when items are variable sizes.

Within PropertyChanging for IsDropDownOpen, the items are instantiated in a VerticalStackLayout with BindableLayout
and using it to measure the size of the items in ItemSource. Once the size is calculated, 
The BindableLayout.ItemsSource and BindableLayout.ItemTemplate are set to null to release the resources.

DropdownHeaderView or DropdownFooterView are used, the associated sizes are added
to the height. The resulting height is cached and used to set SfComboBox.MaxDropDownHeight.

The derived control invalidates the cached value if ItemsSource or ItemTemplate changes.
If ItemsSource implmements INotifyCollectionChanged, the cached value is invalidated when the collection changes.

NOTES:
1: I found that a unique VerticalStackLayout needs to be created for each ComboBox instance.
Sharing it breaks the subsequent ComboBox instances.

2: The VerticalStackLayout needs to be added to the visual tree to ensure all bindings are resolved.
The sample places them in the main page.
