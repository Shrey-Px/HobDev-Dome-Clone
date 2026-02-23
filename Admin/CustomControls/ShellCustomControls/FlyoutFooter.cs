namespace Admin.CustomControls.ShellCustomControls
{
    public class FlyoutFooter : ContentView
    {
        public static readonly BindableProperty FooterCommandProperty = BindableProperty.Create(
            nameof(FooterCommand),
            typeof(ICommand),
            typeof(FlyoutFooter),
            null,
            BindingMode.TwoWay,
            propertyChanged: FlyoutCommandPropertyChanged
        );

        private static void FlyoutCommandPropertyChanged(
            BindableObject bindable,
            object oldValue,
            object newValue
        )
        {
            FlyoutFooter control = (FlyoutFooter)bindable;
            if (control != null)
            {
                control.imageButton.Command = (AsyncRelayCommand)newValue;
            }
        }

        public AsyncRelayCommand FooterCommand
        {
            get => (AsyncRelayCommand)GetValue(FooterCommandProperty);
            set => SetValue(FooterCommandProperty, value);
        }

        ImageButton imageButton;

        public FlyoutFooter()
        {
            imageButton = new ImageButton
            {
                Source = "logoutcurve.png",
                Background = Colors.Transparent,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.End,
                HeightRequest = 60,
                WidthRequest = 64,
                Margin = new Thickness(0, 0, 0, 40),
            };

            imageButton.SetBinding(
                ImageButton.CommandProperty,
                new Binding(nameof(FooterCommand), source: this)
            );
            Content = imageButton;
        }
    }
}
