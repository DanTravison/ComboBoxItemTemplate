namespace ComboBoxEx;

using Syncfusion.Maui.Inputs;
using System.Diagnostics;
using System.Reflection;
using Syncfusion.Maui.Core.Hosting;

public static class ComboBoxExtensions
{
    static readonly PropertyInfo IsAutoFit;

    static ComboBoxExtensions()
    {
        Type type = typeof(SfComboBox);
        IsAutoFit = type.GetProperty(nameof(IsAutoFit), BindingFlags.Instance | BindingFlags.NonPublic);
        if (IsAutoFit == null)
        {
            Trace.WriteLine($"IsAutoFit property could not be found");
        }
    }

    internal static void SetIsAutoFit(this SfComboBox comboBox, bool value)
    {
        if (IsAutoFit != null)
        {
            IsAutoFit.SetValue(comboBox, value);
        }
        else
        {
            Trace.WriteLine($"IsAutoFit property could not be found");
        }
    }

    internal static bool GetIsAutoFit(this SfComboBox comboBox)
    {
        bool value = false;
        if (IsAutoFit != null)
        {
            value = (bool)IsAutoFit.GetValue(comboBox);
            Trace.WriteLine($"IsAutoFit = {value}");
        }
        else
        {
            Trace.WriteLine($"IsAutoFit property could not be found");
        }
        return value;
    }

    /// <summary>
    /// Provide sync fusion wrapper.
    /// </summary>
    /// <param name="builder">The <see cref="MauiAppBuilder"/> to configure.</param>
    /// <returns>The updated <see cref="MauiAppBuilder"/>.</returns>
    /// <remarks>
    /// This provides a proxy to ConfigureSyncfusionCore.
    /// </remarks>
    public static MauiAppBuilder ConfigureSyncFusion(this MauiAppBuilder builder)
    {
        return builder.ConfigureSyncfusionCore();
    }
}
