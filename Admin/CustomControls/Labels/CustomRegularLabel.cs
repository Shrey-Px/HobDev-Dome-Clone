namespace Admin.CustomControls.Labels
{
    public class CustomRegularLabel : Label
    {
        public CustomRegularLabel()
        {
            FontFamily = "InterRegular";
            LineBreakMode = LineBreakMode.WordWrap;
            this.AppThemeBinding(Label.TextColorProperty, Colors.Black, Colors.White);
        }
    }

    public class Regular12Label : CustomRegularLabel
    {
        public Regular12Label()
        {
            FontSize = 12;
        }
    }

    public class Regular14Label : CustomRegularLabel
    {
        public Regular14Label()
        {
            FontSize = 14;
        }
    }

    public class Regular16Label : CustomRegularLabel
    {
        public Regular16Label()
        {
            FontSize = 16;
        }
    }

    public class Regular18Label : CustomRegularLabel
    {
        public Regular18Label()
        {
            FontSize = 18;
        }
    }

    public class Regular20Label : CustomRegularLabel
    {
        public Regular20Label()
        {
            FontSize = 20;
        }
    }

    public class Regular21Label : CustomRegularLabel
    {
        public Regular21Label()
        {
            FontSize = 21;
        }
    }

    public class Regular22Label : CustomRegularLabel
    {
        public Regular22Label()
        {
            FontSize = 22;
        }
    }

    public class Regular24Label : CustomRegularLabel
    {
        public Regular24Label()
        {
            FontSize = 24;
        }
    }

    public class Regular26Label : CustomRegularLabel
    {
        public Regular26Label()
        {
            FontSize = 26;
        }
    }

    public class Regular28Label : CustomRegularLabel
    {
        public Regular28Label()
        {
            FontSize = 28;
        }
    }

    public class Regular36Label : CustomRegularLabel
    {
        public Regular36Label()
        {
            FontSize = 36;
        }
    }
}
