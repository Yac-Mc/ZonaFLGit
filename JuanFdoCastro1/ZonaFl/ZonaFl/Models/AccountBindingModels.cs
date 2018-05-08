using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ZonaFl.Models
{
    // Models used as parameters to AccountController actions.




    public class AddExternalLoginBindingModel
    {
        [Required]
        [Display(Name = "External access token")]
        public string ExternalAccessToken { get; set; }
    }

    public class ChangePasswordBindingModel
    {

        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }


        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        
        [Display(Name = "Token")]
        public string Token { get; set; }

    }

    public class RegisterBindingModel
    {
        public string Id { get; set; }


        public enum Rol
        {
            Administrador = 0,
            Freelance = 1,
            Contratante = 2
            
        }

        public enum State
        {
            Activo = 0,
            Suspendido = 1,
            Borrado = 2,
            Baja = 3
        }
        
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Falta digitar el password")]
        [StringLength(100, ErrorMessage = "El {0} debe contener como minimo {2} caracteres de longitud.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string PasswordHash { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("PasswordHash", ErrorMessage = "El password y la confirmación del password no corresponden.")]
        public string ConfirmPassword { get; set; }

        //freelance
        //empresa

        [Required]
        [Display(Name = "Freelance")]
        public Boolean Freelance { get; set; }

        [Required]
        [Display(Name = "Empresa")]
        public Boolean Empresa { get; set; }

        [Required(ErrorMessage ="Falta digitar el nombre de usuario")]
        [Display(Name = "UserName")]
        public String UserName { get; set; }

       
        [Display(Name = "Ciudad")]
        public string City { get; set; }

       
        [Display(Name = "Pais")]
        public string Country { get; set; }

        
        [Display(Name = "Descripcion Usuario")]
        public string DescUser { get; set; }

        [Display(Name = "Telefono")]
        public string PhoneNumber { get; set; }


        [Display(Name = "Telefono Empresa")]
        public string Telefono { get; set; }

        [Display(Name = "Nombres Y apellidos de Usuario")]
        public string FirstMiddleName { get; set; }



        [Display(Name = "Imagen Usuario")]
        public string Image { get; set; }

        public List<Category> Categories { get; set; }
        public List<Skill> Skills { get; set; }

        //--corrección del 19 de julio 2016

        [Display(Name = "Nombre Empresa")]
        public string NombreEmpresa { get; set; }

        [Display(Name = "Razón Social")]
        public string RazonSocial { get; set; }


        [Display(Name = "Nit")]
        public string Nit { get; set; }

        [Display(Name = "Sector Industrial")]
        public string SecIndustrial { get; set; }


        [Display(Name = "Numero de trabajadores")]
        public int NumeroEmp { get; set; }
        
        [Display(Name = "DescEmpresa")]
        public string DesEmpresa { get; set; }

        [Display(Name = "Pag Web")]
        public string UrlEmpresa { get; set; }


        [Display(Name = "Rol")]
        public Rol RolUser { get; set; }

        [Display(Name = "Fecha Creación")]
        public DateTime CreationDate { get; set; }


        [Display(Name = "Ultimo acceso")]
        public DateTime LastAccess { get; set; }

        [Display(Name = "Estado")]
        public State StateUser { get; set; }

        [Display(Name = "Company")]
        public List<Company> Company { get; set; }

        public List<Offer> Offers { get; set; }
        public List<Education> Educations { get;  set; }
        public List<Certification> Certifications { get; set; }
        public List<Language> Languages { get; set; }
        public Curriculum Curriculum { get; set; }

        public List<Experience> Experiences = new List<Experience>();

        public int Count16 { get; set; }
        public int Count17 { get; set; }
        public int Count18 { get; set; }
        public int Count19 { get; set; }
        public int Count20 { get; set; }
        public int Count21 { get; set; }

        public int Count22 { get; set; }

        public int Count23 { get; set; }
        public int Count24 { get; set; }
        public int Count25 { get; set; }
        public bool EmailConfirmed { get; set; }
    }

    public class RegisterExternalBindingModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Freelance")]
        public Boolean Freelance { get; set; }

        [Required]
        [Display(Name = "Empresa")]
        public Boolean Empresa { get; set; }
        public string UserName { get; set; }
        public bool EmailConfirmed { get; set; }
        public string FirstMiddleName { get; set; }
    }

    public class RemoveLoginBindingModel
    {
        [Required]
        [Display(Name = "Login provider")]
        public string LoginProvider { get; set; }

        [Required]
        [Display(Name = "Provider key")]
        public string ProviderKey { get; set; }
    }

    public class SetPasswordBindingModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
