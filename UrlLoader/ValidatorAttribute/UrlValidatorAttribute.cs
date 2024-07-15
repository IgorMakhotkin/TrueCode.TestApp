using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace UrlLoader.ValidatorAttribute
{
    public class UrlValidatorAttribute : ValidationAttribute
    {
        // Регулярное выражение для проверки URL
        private static readonly Regex UrlRegex = new Regex(
            @"^(https?://)" + // Протокол (http или https)
            @"((([a-z\d]([a-z\d-]*[a-z\d])*)\.)+[a-z]{2,}|" + // Доменное имя
            @"((\d{1,3}\.){3}\d{1,3}))" + // Или IP-адрес
            @"(\:\d+)?(/[-a-z\d%_.~+]*)*" + // Порт и путь
            @"(\?[;&a-z\d%_.~+=-]*)?" + // Параметры запроса
            @"(\#[-a-z\d_]*)?$", // Якорь
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string)
           { 
            string url = value.ToString();

            // Проверка с использованием регулярного выражения
            if (!UrlRegex.IsMatch(url))
            {
                return new ValidationResult("Invalid URL format.");
            }

            // Дополнительная проверка с использованием класса Uri
            if (!Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult))
            {
                return new ValidationResult("Invalid URL format.");
            }

                // Проверка, что URI - это http или https
                if (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps)
                {
                    return new ValidationResult("URL must start with http or https.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
