    namespace ComboBoxItemTemplate.Views;

using ComboBoxItemTemplate.ViewModels;
using Syncfusion.Maui.Inputs;
using System.Diagnostics;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        BindingContext = new MainViewModel();
        InitializeComponent();
        ComboBox.PropertyChanged += OnComboBoxPropertyChanged;
        ComboBox.PropertyChanging += OnComboBoxPropertyChanging;
    }

    private void OnComboBoxPropertyChanging(object sender, PropertyChangingEventArgs e)
    {
        if (e.PropertyName == SfComboBox.IsDropDownOpenProperty.PropertyName)
        {
            Trace.WriteLine($"SfComboBox.PropertyChanging: IsDropDownOpen = {ComboBox.IsDropDownOpen}");
        }
    }

    private void OnComboBoxPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == SfComboBox.IsDropDownOpenProperty.PropertyName)
        {
            Trace.WriteLine($"SfComboBox.PropertyChanged: IsDropDownOpen = {ComboBox.IsDropDownOpen}");
        }
    }
}
