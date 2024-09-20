using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace TitanTechTask.Models
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
