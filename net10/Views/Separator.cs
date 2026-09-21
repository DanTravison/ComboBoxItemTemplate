namespace ComboBoxItemTemplate.Views;

/// <summary>
/// Provides a control for drawing a filled rectangle.
/// </summary>
public sealed class Separator : GraphicsView
{
    /// <summary>
    /// Defines the minimum thickness.
    /// </summary>
    /// <remarks>
    /// If <see cref="Orientation"/> equals <see cref="StackOrientation.Horizontal"/>,
    /// the default height; otherwise, the width.
    /// </remarks>
    public const double MinimumThickness = 3.0;

    /// <summary>
    /// Defines the default thickness.
    /// </summary>
    /// <remarks>
    /// If <see cref="Orientation"/> equals <see cref="StackOrientation.Horizontal"/>,
    /// the default height; otherwise, the width.
    /// </remarks>
    public const double DefaultThickness = 3.0;

    readonly SeparatorDrawable _drawable = new();

    /// <summary>
    /// Initializes a new instance of this class.
    /// </summary>
    public Separator()
    {
        base.Drawable = _drawable;
    }

    protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
    {
        Size size = base.MeasureOverride(widthConstraint, heightConstraint);
        if (Orientation == StackOrientation.Vertical)
        {
            size.Width = Thickness + Margin.HorizontalThickness;
        }
        else
        {
            size.Height = Thickness + Margin.VerticalThickness;
        }
        return size;
    }

    protected override void OnPropertyChanged(string propertyName = null)
    {
        if (StringComparer.Ordinal.Compare(propertyName, BackgroundColorProperty.PropertyName) == 0)
        {
            _drawable.BackgroundColor = BackgroundColor;
            Invalidate();
        }
        else if (StringComparer.Ordinal.Compare(propertyName, MarginProperty.PropertyName) == 0)
        {
            _drawable.BackgroundColor = BackgroundColor;
            Invalidate();
        }
        base.OnPropertyChanging(propertyName);
    }

    protected override Size ArrangeOverride(Rect bounds)
    {
        _drawable.Size = bounds.Size;
        return base.ArrangeOverride(bounds);
    }

    #region Properties

    #region ForegroundColor

    /// <summary>
    /// Gets or sets the color to use to fill the drawn area.
    /// </summary>
    public Color ForegroundColor
    {
        get => (Color)GetValue(ForegroundColorProperty);
        set => SetValue(ForegroundColorProperty, value);
    }

    /// <summary>
    /// Provides the <see cref="BindableProperty"/> for <see cref="ForegroundColor"/>.
    /// </summary>
    public static readonly BindableProperty ForegroundColorProperty = BindableProperty.Create
    (
        nameof(ForegroundColor),
        typeof(Color),
        typeof(Separator),
        Colors.White,
        propertyChanged: (bindableObject, oldValue, newValue) =>
        {
            if (bindableObject is Separator separator)
            {
                separator.OnForegroundColorChanged();
            }
        }
    );

    void OnForegroundColorChanged()
    {
        _drawable.ForegroundColor = ForegroundColor;
        Invalidate();
    }

    #endregion ForegroundColor

    #region Orientation

    /// <summary>
    /// Gets or sets the color to use to fill the drawn area.
    /// </summary>
    public StackOrientation Orientation
    {
        get => (StackOrientation)GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    /// <summary>
    /// Provides the <see cref="BindableProperty"/> for <see cref="Orientation"/>.
    /// </summary>
    public static readonly BindableProperty OrientationProperty = BindableProperty.Create
    (
        nameof(Orientation),
        typeof(StackOrientation),
        typeof(Separator),
        StackOrientation.Horizontal,
        propertyChanged: (bindableObject, oldValue, newValue) =>
        {
            if (bindableObject is Separator separator)
            {
                separator.OnOrientationChanged((StackOrientation)newValue);
            }
        }
    );

    void OnOrientationChanged(StackOrientation newValue)
    {
        if (Orientation == StackOrientation.Horizontal)
        {
            HorizontalOptions = LayoutOptions.Fill;
            VerticalOptions = LayoutOptions.Center;
        }
        else
        {
            HorizontalOptions = LayoutOptions.Center;
            VerticalOptions = LayoutOptions.Fill;
        }
        _drawable.Orientation = newValue;
        OnThicknessChanged();
    }

    #endregion Orientation

    #region Thickness

    /// <summary>
    /// Gets or sets the thickness of the separator.
    /// </summary>
    /// <value>
    /// If <see cref="Orientation"/> is <see cref="StackOrientation.Horizontal"/>,
    /// the height; otherwise, the width.
    /// <para>
    /// The default value is <see cref="MinimumThickness"/>.
    /// </para>
    /// </value>
    public double Thickness
    {
        get => (double)GetValue(ThicknessProperty);
        set => SetValue(ThicknessProperty, value);
    }

    /// <summary>
    /// Provides the <see cref="BindableProperty"/> for <see cref="Thickness"/>.
    /// </summary>
    public static readonly BindableProperty ThicknessProperty = BindableProperty.Create
    (
        nameof(Thickness),
        typeof(double),
        typeof(Separator),
        MinimumThickness,
        coerceValue: (bindableObject, newValue) =>
        {
            if ((double)newValue < MinimumThickness)
            {
                newValue = MinimumThickness;
            }
            return newValue;
        },
        propertyChanged: (bindableObject, oldValue, newValue) =>
        {
            if (bindableObject is Separator separator)
            {
                separator.OnThicknessChanged();
            }
        }
    );

    void OnThicknessChanged()
    {
        if (Orientation == StackOrientation.Horizontal)
        {
            WidthRequest = -1;
            HeightRequest = Margin.VerticalThickness + Thickness;
        }
        else
        {
            HeightRequest = -1;
            WidthRequest = Margin.HorizontalThickness + Thickness;
        }
        InvalidateMeasure();
    }

    #endregion Thickness

    #endregion Properties
}
