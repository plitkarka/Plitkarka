using System.Reactive;
using Plitkarka.Views;
using ReactiveUI;

namespace Plitkarka.ViewModels;

public class MainViewModel : ReactiveObject
{
    private readonly INavigation _navigation;
    public ReactiveCommand<Unit, Task> OpenRegistrationFormCommand { get; }
    public ReactiveCommand<Unit, Task> OpenLoginFormCommand { get; }
    
    public MainViewModel(INavigation navigation)
    {
        _navigation = navigation;
        
        OpenRegistrationFormCommand = ReactiveCommand.Create(async () =>
        {
            await _navigation.PushAsync(new RegistrationPage());
        });
        
        OpenLoginFormCommand = ReactiveCommand.Create(async () =>
        {
            await _navigation.PushAsync(new LoginPage());
        });
    }
}