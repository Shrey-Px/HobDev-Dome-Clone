namespace Admin.CustomControls.Labels
{
    public class CutomBoldLabel : Label
    {
        public CutomBoldLabel()
        {
            FontFamily = "InterBold";
            LineBreakMode = LineBreakMode.WordWrap;
            this.AppThemeBinding(Label.TextColorProperty, Colors.Black, Colors.White);
        }
    }

    public class Bold10Label : CutomBoldLabel
    {
        public Bold10Label()
        {
            FontSize = 10;
        }
    }

    public class Bold12Label : CutomBoldLabel
    {
        public Bold12Label()
        {
            FontSize = 12;
        }
    }

    public class Bold14Label : CutomBoldLabel
    {
        public Bold14Label()
        {
            FontSize = 14;
        }
    }

    public class Bold16Label : CutomBoldLabel
    {
        public Bold16Label()
        {
            FontSize = 16;
        }
    }

    public class Bold18Label : CutomBoldLabel
    {
        public Bold18Label()
        {
            FontSize = 18;
        }
    }

    public class Bold20Label : CutomBoldLabel
    {
        public Bold20Label()
        {
            FontSize = 20;
        }
    }

    public class Bold22Label : CutomBoldLabel
    {
        public Bold22Label()
        {
            FontSize = 22;
        }
    }

    public class Bold24Label : CutomBoldLabel
    {
        public Bold24Label()
        {
            FontSize = 24;
        }
    }

    public class Bold32Label : CutomBoldLabel
    {
        public Bold32Label()
        {
            FontSize = 32;
        }
    }

    public class Bold40Label : CutomBoldLabel
    {
        public Bold40Label()
        {
            FontSize = 40;
        }
    }
}
