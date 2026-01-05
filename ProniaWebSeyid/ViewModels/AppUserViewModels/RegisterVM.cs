namespace ProniaWebSeyid.ViewModels.AppUserViewModels
{
    public class RegisterVM
    {
       
            [Required, MaxLength(50), MinLength(3)]
            public string FirstName { get; set; } = string.Empty;

            [Required, MaxLength(50), MinLength(3)]
            public string LastName { get; set; } = string.Empty;

            [Required, MaxLength(50), MinLength(3)]
            public string UserName { get; set; } = string.Empty;

            [Required, MaxLength(50), EmailAddress]
            public string EmailAddress { get; set; } = string.Empty;

            [Required, DataType(DataType.Password), MinLength(6)]
            public string Password { get; set; } = string.Empty;

            [Required,DataType(DataType.Password),Compare(nameof(Password))]
            public string ConfirmPassword { get; set; } = string.Empty;

    }
}
