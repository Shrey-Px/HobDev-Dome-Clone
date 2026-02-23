namespace Admin.CustomControls.Labels
{
    public class CutomLightLabel : Label
    {
        public CutomLightLabel()
        {
            FontFamily = "InterLight";
            LineBreakMode = LineBreakMode.WordWrap;
            this.AppThemeBinding(Label.TextColorProperty, Colors.Black, Colors.White);
        }
    }

    public class Light12Label : CutomLightLabel
    {
        public Light12Label()
        {
            FontSize = 12;
        }
    }

    public class Light14Label : CutomLightLabel
    {
        public Light14Label()
        {
            FontSize = 14;
        }
    }

    public class Light16Label : CutomLightLabel
    {
        public Light16Label()
        {
            FontSize = 16;
        }
    }

    public class Light18Label : CutomLightLabel
    {
        public Light18Label()
        {
            FontSize = 18;
        }
    }

    public class Light20Label : CutomLightLabel
    {
        public Light20Label()
        {
            FontSize = 20;
        }
    }

    public class Light22Label : CutomLightLabel
    {
        public Light22Label()
        {
            FontSize = 22;
        }
    }

    public class Light24Label : CutomLightLabel
    {
        public Light24Label()
        {
            FontSize = 24;
        }
    }

    public class Light32Label : CutomLightLabel
    {
        public Light32Label()
        {
            FontSize = 32;
        }
    }

    public class Light40Label : CutomLightLabel
    {
        public Light40Label()
        {
            FontSize = 40;
        }
    }
}
