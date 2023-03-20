using System.Reactive;
using Plitkarka.Core.StringResources;
using Plitkarka.Views;
using ReactiveUI;

namespace Plitkarka.ViewModels;

public class LoginViewModel : ReactiveObject
{
    private string _email;
    private string _password;
    private string _errorText;
    private readonly INavigation _navigation;

    public string Email
    {
        get => _email;
        set => this.RaiseAndSetIfChanged(ref _email, value);
    }

    public string Password
    {
        get => _password;
        set => this.RaiseAndSetIfChanged(ref _password, value);
    }

    public string ErrorText
    {
        get => _errorText;
        set => this.RaiseAndSetIfChanged(ref _errorText, value);
    }

    public ReactiveCommand<Unit, Task> LoginCommand { get; }

    public LoginViewModel(INavigation navigation)
    {
        _navigation = navigation;

        LoginCommand = ReactiveCommand.Create(async () =>
        {
            if (string.IsNullOrWhiteSpace(Email) || !Email.Contains("@"))
            {
                ErrorText = Strings.InvalidEmail;
                return;
            }

            if (string.IsNullOrWhiteSpace(Password) || Password.Length < 6)
            {
                ErrorText = Strings.InvalidEmail;
                return;
            }

            await _navigation.PushAsync(new HomePage());
        });
    }
}
