namespace ComboBoxItemTemplate.Views;

using Syncfusion.Maui.Inputs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

internal class ComboBox : SfComboBox
{

    protected override void OnDropdownOpening()
    {
        Trace.WriteLine($"ComboBox.OnDropdownOpening");
        TraceContent();
        base.OnDropdownOpening();
    }

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        if (propertyName == ItemsSourceProperty.PropertyName || propertyName == ItemTemplateProperty.PropertyName)
        {
            Trace.WriteLine($"ComboBox.PropertyChanging.{propertyName}");
            TraceContent();
        }
        if (propertyName == IsDropDownOpenProperty.PropertyName)
        {
            Trace.WriteLine($"ComboBox.PropertyChanging.{propertyName}");
            TraceContent();
        }
        base.OnPropertyChanged(propertyName);
    }

    void TraceContent()
    {
        Trace.WriteLine($"ComboBox.IsDropDownOpen {IsDropDownOpen}");
        string itemTemplate = ItemTemplate is not null ? ItemTemplate.GetType().Name : "[null]";
        Trace.WriteLine($"ComboBox.ItemTemplate {itemTemplate}");

        int count = ItemsSource is ICollection collection ? collection.Count : 0;
        string itemsSource = ItemsSource is not null ? ItemsSource.GetType().Name : "[null]";
        Trace.WriteLine($"ComboBox.ItemsSource {itemsSource}[{count}]");
    }
}
