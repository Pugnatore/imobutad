using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Projeto_Lab4.Models
{
    public class RegistoInfo
    {
        [Required]
        public String Nome { get; set; }


        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "A {0} deve conter pelo menos {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar password")]
        [Compare("Password", ErrorMessage = "As passwords não correspondem.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Data de Nascimento")]
        public DateTime Data_Nascimento { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [Display(Name = "Numero de Telemovel")]
        public string Numero_de_Telemovel { get; set; }

        [Required]
        [Display(Name = "Numero de Cartao de Cidadão")]
        public int Cartao_de_Cidadao { get; set; }

  
        [Display(Name = "Foto de perfil")]
        public string Foto_perfil { get; set; }

        [Required]
        public String Utilizador { get; set; }



    }
}