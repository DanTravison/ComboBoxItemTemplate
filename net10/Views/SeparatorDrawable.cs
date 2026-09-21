namespace ComboBoxItemTemplate.Views;

class SeparatorDrawable : IDrawable
{
    /// <summary>
    /// Gets or sets the color to use to fill the bar.
    /// </summary>
    public Color ForegroundColor
    {
        get;
        set;
    } = Colors.Gray;

    /// <summary>
    /// Gets or sets the color to use to fill the bar.
    /// </summary>
    public Color BackgroundColor
    {
        get;
        set;
    } = Colors.Transparent;

    /// <summary>
    /// Gets or sets the orientation of the bar.
    /// </summary>
    public StackOrientation Orientation
    {
        get;
        set;
    } = StackOrientation.Horizontal;

    /// <summary>
    /// Gets or sets the margin for the control
    /// </summary>
    /// <value>
    /// The <see cref="Thickness"/> value defining the margin.
    /// </value>
    public Thickness Margin
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the size of the containing control
    /// </summary>
    /// <value>
    /// The <see cref="Size"/> value defining size of the containing control.
    /// </value>
    public Size Size
    {
        get;
        set;
    }

    /// <summary>
    /// Gets the thickness of the line to draw the separator.
    /// </summary>
    public double Thickness
    {
        get;
        set;
    } = Separator.DefaultThickness;

    /// <summary>
    /// Draws the bar.
    /// </summary>
    /// <param name="canvas">The <see cref="ICanvas"/> to draw on.</param>
    /// <param name="dirtyRect">The area of the <paramref name="canvas"/> to draw.</param>
    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        double widthMargin = Margin.HorizontalThickness;
        double heightMargin = Margin.VerticalThickness;

        canvas.FillColor = BackgroundColor;
        canvas.StrokeColor = ForegroundColor;

        canvas.FillRectangle(dirtyRect.X, dirtyRect.Y, dirtyRect.Width, dirtyRect.Height);

        canvas.StrokeSize = (float)Thickness;
        canvas.StrokeLineCap = LineCap.Round;

        float startX = 0;
        float startY = 0;
        float endX;
        float endY;
        if (Orientation == StackOrientation.Horizontal)
        {
            startX = dirtyRect.Left;
            endX = dirtyRect.Right;

            startY = dirtyRect.Top + (float)(dirtyRect.Height - Thickness) / 2;
            endY = startY;
        }
        else
        {
            startY = dirtyRect.Top;
            endY = dirtyRect.Bottom;

            startX = dirtyRect.Left + (float)(dirtyRect.Width - Thickness) / 2;
            endX = startX;
            
        }
        canvas.DrawLine(startX, startY, endX, endY);
    }
}
