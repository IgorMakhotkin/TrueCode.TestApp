using UrlLoader.ValidatorAttribute;

namespace UrlLoader.Database.Model
{
    public class UrlRequestDto
    {
        [UrlValidator(ErrorMessage = "Please enter a valid URL.")]
        public string Url { get; set; }
    }
}
