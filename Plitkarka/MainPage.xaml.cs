using Plitkarka.ViewModels;

namespace Plitkarka;
public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        
        BindingContext = new MainViewModel(Navigation);
    }
}