namespace Player.CustomControls.Labels
{
    public class CustomMediumLabel : Label
    {
        public CustomMediumLabel()
        {
            FontFamily = "InterMedium";
            LineBreakMode = LineBreakMode.WordWrap;
            this.AppThemeBinding(Label.TextColorProperty, Colors.Black, Colors.White);
        }
    }

    public class Medium8Label : CustomMediumLabel
    {
        public Medium8Label()
        {
            FontSize = 8;
        }
    }

    public class Medium9Label : CustomMediumLabel
    {
        public Medium9Label()
        {
            FontSize = 9;
        }
    }

    public class Medium10Label : CustomMediumLabel
    {
        public Medium10Label()
        {
            FontSize = 10;
        }
    }

    public class Medium11Label : CustomMediumLabel
    {
        public Medium11Label()
        {
            FontSize = 11;
        }
    }

    public class Medium12Label : CustomMediumLabel
    {
        public Medium12Label()
        {
            FontSize = 12;
        }
    }

    public class Medium14Label : CustomMediumLabel
    {
        public Medium14Label()
        {
            FontSize = 14;
        }
    }

    public class Medium15Label : CustomMediumLabel
    {
        public Medium15Label()
        {
            FontSize = 15;
        }
    }

    public class Medium16Label : CustomMediumLabel
    {
        public Medium16Label()
        {
            FontSize = 16;
        }
    }

    public class Medium18Label : CustomMediumLabel
    {
        public Medium18Label()
        {
            FontSize = 18;
        }
    }

    public class Medium20Label : CustomMediumLabel
    {
        public Medium20Label()
        {
            FontSize = 20;
        }
    }

    public class Medium22Label : CustomMediumLabel
    {
        public Medium22Label()
        {
            FontSize = 22;
        }
    }

    public class Medium26Label : CustomMediumLabel
    {
        public Medium26Label()
        {
            FontSize = 26;
        }
    }

    public class Medium30Label : CustomMediumLabel
    {
        public Medium30Label()
        {
            FontSize = 30;
        }
    }
}
