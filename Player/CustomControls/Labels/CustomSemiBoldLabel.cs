namespace Player.CustomControls.Labels
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

    public class SemiBold8Label : CustomSemiBoldLabel
    {
        public SemiBold8Label()
        {
            FontSize = 8;
        }
    }

    public class SemiBold9Label : CustomSemiBoldLabel
    {
        public SemiBold9Label()
        {
            FontSize = 9;
        }
    }

    public class SemiBold10Label : CustomSemiBoldLabel
    {
        public SemiBold10Label()
        {
            FontSize = 10;
        }
    }

    public class SemiBold12Label : CustomSemiBoldLabel
    {
        public SemiBold12Label()
        {
            FontSize = 12;
        }
    }

    public class SemiBold13Label : CustomSemiBoldLabel
    {
        public SemiBold13Label()
        {
            FontSize = 13;
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

    public class SemiBold24Label : CustomSemiBoldLabel
    {
        public SemiBold24Label()
        {
            FontSize = 24;
        }
    }
}
