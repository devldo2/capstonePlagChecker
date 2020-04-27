namespace AntiPlagiatus
{
    using AntiPlagiatus.ViewModels;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        public BaseViewModel ViewModel
        {
            get { return (BaseViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty = 
            DependencyProperty.Register("ViewModel", typeof(BaseViewModel), typeof(MainPage), null);
    }
}
