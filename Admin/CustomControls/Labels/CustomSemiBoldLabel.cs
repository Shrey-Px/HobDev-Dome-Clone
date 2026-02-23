namespace Admin.CustomControls.Labels
{
    public class CustomSemiBoldLabel : Label
    {
        public CustomSemiBoldLabel()
        {
            FontFamily = "InterSemiBold";
            LineBreakMode = LineBreakMode.WordWrap;
            this.AppThemeBinding(Label.TextColorProperty, Colors.Black, Colors.White);
        }
    }

    public class SemiBold12Label : CustomSemiBoldLabel
    {
        public SemiBold12Label()
        {
            FontSize = 12;
        }
    }

    public class SemiBold14Label : CustomSemiBoldLabel
    {
        public SemiBold14Label()
        {
            FontSize = 14;
        }
    }

    public class SemiBold16Label : CustomSemiBoldLabel
    {
        public SemiBold16Label()
        {
            FontSize = 16;
        }
    }

    public class SemiBold18Label : CustomSemiBoldLabel
    {
        public SemiBold18Label()
        {
            FontSize = 18;
        }
    }

    public class SemiBold20Label : CustomSemiBoldLabel
    {
        public SemiBold20Label()
        {
            FontSize = 20;
        }
    }

    public class SemiBold22Label : CustomSemiBoldLabel
    {
        public SemiBold22Label()
        {
            FontSize = 22;
        }
    }

    public class SemiBold24Label : CustomSemiBoldLabel
    {
        public SemiBold24Label()
        {
            FontSize = 24;
        }
    }
}
