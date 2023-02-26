using System.Reactive;
using ReactiveUI;
using Plitkarka.Core.StringResources;
using Plitkarka.Views;

namespace Plitkarka.ViewModels;

public class RegistrationViewModel : ReactiveObject
{
    private string _name;
    private string _surname;
    private DateTime _birthDate;
    private string _email;
    private string _password;
    private string _confirmPassword;
    private string _errorText;
    
    private readonly INavigation _navigation;

    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }
    
    public string Surname
    {
        get => _surname;
        set => this.RaiseAndSetIfChanged(ref _surname, value);
    }
    
    public DateTime BirthDate
    {
        get => _birthDate;
        set => this.RaiseAndSetIfChanged(ref _birthDate, value);
    }
    
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
    
    public string ConfirmPassword
    {
        get => _confirmPassword;
        set => this.RaiseAndSetIfChanged(ref _confirmPassword, value);
    }

    public string ErrorText
    {
        get => _errorText;
        set => this.RaiseAndSetIfChanged(ref _errorText, value);
    }

    public ReactiveCommand<Unit, Task> RegisterCommand { get; }

    public RegistrationViewModel(INavigation navigation)
    {
        _navigation = navigation;

        RegisterCommand = ReactiveCommand.Create(async () =>
        {
            if (string.IsNullOrWhiteSpace(Email) || !Email.Contains("@"))
            {
                ErrorText = Strings.InvalidEmail;
                return;
            }

            if (string.IsNullOrWhiteSpace(Password) || Password.Length < 6)
            {
                ErrorText = Strings.InvalidPassword;
                return;
            }
            
            if (!Password.Equals(ConfirmPassword))
            {
                ErrorText = Strings.PasswordsDoNotMatch;
                return;
            }

            await _navigation.PushAsync(new HomePage());
        });
    }

}
