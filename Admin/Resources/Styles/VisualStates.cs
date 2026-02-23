namespace Admin.Resources.Styles
{
    public class VisualStates : ResourceDictionary
    {
        public VisualStates()
        {
            Add(nameof(BorderVisualState), BorderVisualState);
            Add(nameof(LabelVisualState), LabelVisualState);
        }

        public static Style BorderVisualState = new Style<Border>(
            (
                VisualStateManager.VisualStateGroupsProperty,
                new VisualStateGroupList
                {
                    new VisualStateGroup
                    {
                        Name = nameof(VisualStateManager.CommonStates),
                        States =
                        {
                            new VisualState
                            {
                                Name = VisualStateManager.CommonStates.Selected,
                                Setters =
                                {
                                    new Setter
                                    {
                                        Property = Border.BackgroundProperty,
                                        Value = Color.FromArgb("#EF2F50"),
                                    },
                                },
                            },
                            new VisualState
                            {
                                Name = VisualStateManager.CommonStates.Normal,
                                Setters =
                                {
                                    new Setter
                                    {
                                        Property = Border.BackgroundProperty,
                                        Value = Colors.LightGray,
                                    },
                                },
                            },
                        },
                    },
                }
            ),
            (Border.StrokeThicknessProperty, 0),
            (Border.PaddingProperty, new Thickness(8, 4)),
            (Border.StrokeShapeProperty, new RoundRectangle { CornerRadius = new CornerRadius(10) })
        ).ApplyToDerivedTypes(true);

        public static Style LabelVisualState = new Style<Label>(
            VisualStateManager.VisualStateGroupsProperty,
            new VisualStateGroupList
            {
                new VisualStateGroup
                {
                    Name = nameof(VisualStateManager.CommonStates),
                    States =
                    {
                        new VisualState
                        {
                            Name = VisualStateManager.CommonStates.Selected,
                            Setters =
                            {
                                new Setter
                                {
                                    Property = Label.TextColorProperty,
                                    Value = Colors.White,
                                },
                            },
                        },
                        new VisualState
                        {
                            Name = VisualStateManager.CommonStates.Normal,
                            Setters =
                            {
                                new Setter
                                {
                                    Property = Label.TextColorProperty,
                                    Value = Colors.Black,
                                },
                            },
                        },
                    },
                },
            }
        );
    }
}
