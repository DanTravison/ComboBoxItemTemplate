namespace ComboBoxItemTemplate.Views;

using ComboBoxItemTemplate.ViewModels;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        BindingContext = new MainViewModel();
        InitializeComponent();
    }
}
