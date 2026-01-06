namespace ProniaWebSeyid.ViewModels.AppUserViewModels
{
    public class LoginVM
    {
        [Required, MaxLength(50), EmailAddress]
        public string EmailAddress { get; set; } = string.Empty;

        [Required, DataType(DataType.Password), MinLength(6)]
        public string Password { get; set; } = string.Empty;
        public bool IsRemember { get; set; }
    }
}
