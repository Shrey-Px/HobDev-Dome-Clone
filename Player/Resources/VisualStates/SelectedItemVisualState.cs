namespace Player.Resources.VisualStates
{
    public class SelectedItemVisualState : ResourceDictionary
    {
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
                                    //new Setter
                                    //{
                                    //    TargetName="label",
                                    //    Property = Label.TextColorProperty,
                                    //    Value = Colors.White,
                                    //},
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
                                    // new Setter
                                    //{
                                    //    TargetName="label",
                                    //    Property = Label.TextColorProperty,
                                    //    Value = Colors.Black,
                                    //},
                                },
                            },
                        },
                    },
                }
            ),
            (Border.StrokeThicknessProperty, 0),
            (Border.PaddingProperty, 10),
            (Border.StrokeShapeProperty, new RoundRectangle { CornerRadius = new CornerRadius(8) })
        ).ApplyToDerivedTypes(true);
    }
}
