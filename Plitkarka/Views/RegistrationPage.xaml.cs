using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plitkarka.ViewModels;

namespace Plitkarka.Views;

public partial class RegistrationPage : ContentPage
{
    public RegistrationPage()
    {
        InitializeComponent();
        
        BindingContext = new RegistrationViewModel(Navigation);
    }
}