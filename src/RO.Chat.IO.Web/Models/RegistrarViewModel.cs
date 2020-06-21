using System;
using System.ComponentModel.DataAnnotations;

namespace RO.Chat.IO.Web.Models
{
    public class RegistrarViewModel
    {
        public RegistrarViewModel()
        {
            Id = Guid.NewGuid();
        }
        
        public Guid Id { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        public string Nome { get; set; }
        public string Nome_Usuario { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Senha { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar senha")]
        [Compare("Senha", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmarSenha { get; set; }

    }
}