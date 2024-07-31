namespace ComboBoxItemTemplate.Views;

using Syncfusion.Maui.Inputs;
using System.Diagnostics;
using System.Reflection;

static class ComboBoxExtensions
{
    static PropertyInfo _isAutoFitProperty;

    static ComboBoxExtensions()
    {
        Type type = typeof(SfComboBox);
        _isAutoFitProperty = type.GetProperty("IsAutoFit", BindingFlags.Instance | BindingFlags.NonPublic);
        if (_isAutoFitProperty == null)
        {
            Trace.WriteLine($"IsAutoFit property could not be found");
        }
    }

    public static void SetAutoFit(this SfComboBox comboBox, bool value)
    {
        if (_isAutoFitProperty != null)
        {
            _isAutoFitProperty.SetValue(comboBox, value);
        }
        else
        {
            Trace.WriteLine($"IsAutoFit property could not be found");
        }
    }

    public static bool GetAutoFit(this SfComboBox comboBox)
    {
        bool value = false;
        if (_isAutoFitProperty != null)
        {
            value = (bool)_isAutoFitProperty.GetValue(comboBox);
            Trace.WriteLine($"IsAutoFit = {value}");
        }
        else
        {
            Trace.WriteLine($"IsAutoFit property could not be found");
        }
        return value;
    }
}
