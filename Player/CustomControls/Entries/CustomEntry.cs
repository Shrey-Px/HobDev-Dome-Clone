namespace Player.CustomControls.Entries
{
    public class CustomEntry : Entry
    {
        public CustomEntry()
        {
            this.AppThemeBinding(Entry.TextColorProperty, Colors.Black, Colors.White)
                .AppThemeBinding(
                    Entry.PlaceholderColorProperty,
                    Color.FromArgb("#626262"),
                    Color.FromArgb("#BBBBBB")
                );
            this.FontFamily = "InterMedium";
            this.FontSize = 16;
            Background = Colors.Transparent;
        }
    }
}
