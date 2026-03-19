using FluentValidation;
using Parking.API.scr.Shared.Resource.ApiResponse;
using SimpleResults;

namespace Parking.API.scr.Shared.ValidationRules
{
    public static class PasswordValidator
    {
        public static IRuleBuilderOptions<T, string> MustBeSecurePassword<T>(
            this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must((rootObject, password, context) =>
            {
                if (string.IsNullOrWhiteSpace(password))
                {
                    context.AddFailure(messages.PasswordIsEmpty);
                    return false;
                }

                Result result = IsPasswordSecure(password);
                if (result.IsSuccess)
                    return true;

                foreach (string error in result.Errors)
                    context.AddFailure(error);

                return false;
            });
        }

        private static Result IsPasswordSecure(string password)
        {
            var errors = new List<string>();
            if (password.Length < 5)
                errors.Add(messages.PasswordMinimumCharacters);

            if (HasNotUpperCaseLetters(password))
                errors.Add(messages.PasswordHasNotUpperCaseLetters);

            if (HasNotLowerCaseLetters(password))
                errors.Add(messages.PasswordHasNotLowerCaseLetters);

            if (HasNotNumbers(password))
                errors.Add(messages.PasswordHasNotNumbers);

            return errors.Count > 0 ? Result.Invalid(errors) : Result.Success();
        }

        private static bool HasNotUpperCaseLetters(string password)
            => !password.Where(c => c is >= 'A' and <= 'Z').Any();

        private static bool HasNotLowerCaseLetters(string password)
            => !password.Where(c => c is >= 'a' and <= 'z').Any();

        private static bool HasNotNumbers(string password)
            => !password.Where(c => c is >= '1' and <= '9').Any();
    }
}
