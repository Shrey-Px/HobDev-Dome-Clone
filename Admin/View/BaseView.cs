namespace Admin.View
{
    public class BaseView : ContentPage
    {
        public BaseView(BaseViewModel viewModel)
        {
            this.AppThemeBinding(
                ContentPage.BackgroundProperty,
                Color.FromArgb("#F1F3F2"),
                Colors.Black
            );

            BindingContext = viewModel;
        }
    }
}
