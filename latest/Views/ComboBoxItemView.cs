namespace ComboBoxItemTemplate.Views;

using System.Diagnostics;
using Command = ObjectModel.Command;

public class ComboBoxItemView : ContentView
{
    public override SizeRequest Measure(double widthConstraint, double heightConstraint, MeasureFlags flags = MeasureFlags.None)
    {
        SizeRequest size = base.Measure(widthConstraint, heightConstraint, flags);
        BindingInfo(out string type, out string text);
        double width = Math.Round(size.Request.Width);
        double height = Math.Round(size.Request.Height);
        Trace.WriteLine($"MeasureOverride.{type}[{text}]: {width}x{height}");

        return size;
    }

    protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
    {
        SizeRequest size = base.MeasureOverride(widthConstraint, heightConstraint);

        BindingInfo(out string type, out string text);
        double width = Math.Round(size.Request.Width);
        double height = Math.Round(size.Request.Height);
        Trace.WriteLine($"MeasureOverride.{type}[{text}]: {width}x{height}");

        return size;
    }

    protected override Size ArrangeOverride(Rect bounds)
    {
        BindingInfo(out string type, out string text);
        Rect rect = new
        (
            Math.Round(bounds.X),
            Math.Round(bounds.Y),
            Math.Round(bounds.Width),
            Math.Round(bounds.Height)
        );
        Trace.WriteLine($"ArrangeOverride.{type}[{text}]: {rect.X}:{rect.Y} {rect.Width}x{rect.Height}");
        return base.ArrangeOverride(bounds);
    }

    protected override void OnBindingContextChanged()
    {
        TraceBindingContext(nameof(OnBindingContextChanged));

        base.OnBindingContextChanged();
    }

    void TraceBindingContext(string memberName)
    {
        BindingInfo(out string type, out string text);
        string viewName = GetType().Name;
        if (BindingContext == null)
        {
            memberName += ".Alert";
        }
        Trace.WriteLine($"{viewName}.{memberName}: BindingContext:{type}[{text}]");
    }

    void BindingInfo(out string type, out string text)
    {
        type = BindingContext?.GetType().Name ?? "null";

        if (BindingContext is Command command)
        {
            text = command.Text;
        }
        else
        {
            text = "null";
        }

    }
}
