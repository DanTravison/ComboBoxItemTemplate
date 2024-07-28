# SfComboBoxItemTemplate
Illustrates a layout regresion when using SfComboBox.ItemTemplate. 

The repro contains 2 solutions directories with an associated project, latest and 24.2.8.
The latest directory builds against SyncFusion.Maui.Inputs v26.2.4 and the 24.2.8 builds against the 
SyncFusion.Maui.Inputs v24.2.8. The sources for the projects are identical.

Each project presents an SfComboBox using a DataTemplateSelector as the ItemTemplate.  The selector
returns 1 of 3 DataTemplates
- A logical separator that contains a BoxView
- A single line Label control.
- A VerticalStackLayout presenting 2 label controls

When building against v24.2.8, the combobox sizes each item based on it's associated DataTemplate/View, as expected.
![v24.2.8](/Layout.24.2.8.png)

When building against v26.2.4, all combo box items are the same size resulting in the 2-line items being clipped.
![v26.2.4](/Layout.26.2.4.png)

The behavior change occurs in v25.1.35 and forward and is likely caused by:

- #FB46095 - Provided support to drop-down elements customizations using the following properties in .NET MAUI SfComboBox