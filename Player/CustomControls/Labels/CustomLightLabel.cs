namespace Player.CustomControls.Labels
{
    public class CustomLightLabel : Label
    {
        public CustomLightLabel()
        {
            FontFamily = "InterLight";
            LineBreakMode = LineBreakMode.WordWrap;
            this.AppThemeBinding(Label.TextColorProperty, Colors.Black, Colors.White);
        }
    }

    public class Light8Label : CustomLightLabel
    {
        public Light8Label()
        {
            FontSize = 8;
        }
    }

    public class Light9Label : CustomLightLabel
    {
        public Light9Label()
        {
            FontSize = 9;
        }
    }

    public class Light10Label : CustomLightLabel
    {
        public Light10Label()
        {
            FontSize = 10;
        }
    }

    public class Light12Label : CustomLightLabel
    {
        public Light12Label()
        {
            FontSize = 12;
        }
    }

    public class Light14Label : CustomLightLabel
    {
        public Light14Label()
        {
            FontSize = 14;
        }
    }

    public class Light16Label : CustomLightLabel
    {
        public Light16Label()
        {
            FontSize = 16;
        }
    }

    public class Light18Label : CustomLightLabel
    {
        public Light18Label()
        {
            FontSize = 18;
        }
    }
}
