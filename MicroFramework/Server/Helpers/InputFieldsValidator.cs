using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using FluentValidation;
using System.Threading.Tasks;
using FluentValidation.Validators;
using Framework.AccountData;

namespace Framework.Server.Helpers;


public class UserValidator : AbstractValidator<Account>
{
    public UserValidator()
    {
        RuleFor(u => u.Email)
            .NotEmpty()
            .EmailAddress()
            .NotNull().WithMessage("Почта введена не корректно!");
        RuleFor(u => u.Password)
            .MatchPassword()
            .WithMessage("Пароль не соответствует требованиям!");
    }
}

public static class CustomValidators
{
    public static IRuleBuilderOptions<T, string> MatchPassword<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.SetValidator(new RegularExpressionValidator<T>(@"(?=.*[0-9])(?=.*[!@#$%^&*])(?=.*[a-z])(?=.*[A-Z])[0-9a-zA-Z!@#$%^&*]{8,}"));
    }
}

public class InputFieldsValidator
{
    public static bool ValidateLoginString(string input)
        => Regex.IsMatch(input,
            @"^[a-zA-Z_]([a-zA-Z0-9\-_\.])*$");

    public static bool ValidatePasswordString(string input, out string errorMsg)
    {
        errorMsg = string.Empty;
        if (input.Length < 8)
        {
            errorMsg = "Пароль должен содержать хотя бы 8 символов";
            return false;
        }
        if (!Regex.IsMatch(input, @"[a-z]"))
        {
            errorMsg = "Пароль должен содержать хотя бы одну строчную английскую букву";
            return false;
        }
        if (!Regex.IsMatch(input, @"[A-Z]"))
        {
            errorMsg = "Пароль должен содержать хотя бы одну заглавную английскую букву";
            return false;
        }
        if (!Regex.IsMatch(input, @"[0-9]"))
        {
            errorMsg = "Пароль должен содержать хотя бы одну цифру";
            return false;
        }
        if (!Regex.IsMatch(input, @"[,\.?<>\\!@#$%^&*()_\+\-=/~]"))
        {
            errorMsg = "Пароль должен содержать хотя бы один спец. символ: ,.?<>\\!@#$%^&*()_+-=/~";
            return false;
        }
        if (Regex.IsMatch(input, @"[^a-zA-Z0-9,\.?<>\\!@#$%^&*()_\+\-=/~]"))
        {
            errorMsg = "Пароль содержит недопустимый символ";
            return false;
        }

        return true;
    }

    public static bool ValidateEmailString(string email)
        => Regex.IsMatch(email,
            @"^([0-9a-zA-Z]([\+\-_\.][0-9a-zA-Z]+)*)+@(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[a-zA-Z0-9]{2,17})$");

    public static bool ValidateTextString(string text)
        => !text.Contains('\'');
}
