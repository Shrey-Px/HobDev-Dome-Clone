namespace Admin.CustomControls.Entries
{
    public class CustomEntry : Entry
    {
        public CustomEntry()
        {
            this.AppThemeBinding(Entry.TextColorProperty, Colors.Black, Colors.White)
                .AppThemeBinding(Entry.PlaceholderColorProperty, Colors.Black, Colors.White);
            this.FontFamily = "InterRegular";
            this.FontSize = 16;
        }
    }
}
