namespace Plitkarka.Commons.Helpers;

public static class EmailTextTemplates
{
    public static readonly string VerificationCode = "VerificationCode";
    public static readonly string ResetPasswordCode = "ResetPasswordCode";
    public static string VerificationCodeText(string name,string emailCode) => 
        $"{name} ласкаво просимо до спільноти Plitkarka. Ось ваш верифікаційний код для підтвердження облікового запису: {emailCode}";
    public static string ResetPasswordCodeText(string name, string passwordCode) =>
        $"{name} вас вітає команда Plitkarka. Ми отрамали запит на зміну пароля входа в обліковий запит. Код для підтвердження зміни паролю: {passwordCode}";
}
