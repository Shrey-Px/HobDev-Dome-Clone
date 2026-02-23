namespace Player.CustomControls.Buttons
{
    public class MediumButton : Button
    {
        public MediumButton()
        {
            Background = Color.FromArgb("#EF2F50");
            FontFamily = "InterMedium";
            FontSize = 15;
            TextColor = Colors.White;
            CornerRadius = 10;
            Padding = new Thickness(10);
        }
    }
}
