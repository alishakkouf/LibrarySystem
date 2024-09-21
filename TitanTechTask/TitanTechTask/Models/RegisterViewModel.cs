using System.ComponentModel.DataAnnotations;

namespace TitanTechTask.Models
{
    public class RegisterViewModel : IValidation
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string ValidationMessage { get; private set; } = string.Empty;

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(Password) || Password.Length < 6)
            {
                ValidationMessage = "Password is required and must be at least 6 characters long.";
                return false;
            }
            if (string.IsNullOrEmpty(Username))
            {
                ValidationMessage = "Username is required";
                return false;
            }

            ValidationMessage = "Valid data";
            return true;
        }
    }
}
