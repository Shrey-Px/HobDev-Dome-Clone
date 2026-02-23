namespace Admin.CustomControls.Borders
{
    public class CustomBorder : Border
    {
        public CustomBorder()
        {
            StrokeThickness = .5;
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(10) };
            Padding = new Thickness(0);

            this.AppThemeBinding(Border.StrokeProperty, Brush.Black, Brush.White);

            this.AppThemeBinding(Border.BackgroundProperty, Colors.White, Colors.Black);
        }
    }
}
