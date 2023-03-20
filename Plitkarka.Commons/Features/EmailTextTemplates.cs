namespace Plitkarka.Commons.Features;

public static class EmailTextTemplates
{
    public static readonly string VerificationCode = "VerificationCode";
    public static string VerificationCodeText(string name,string emailCode) => 
        $"{name} ласкаво просимо до спільноти Plitkarka. Ось ваш верифікаційний код для підтвердження облікового запису {emailCode}";

}
