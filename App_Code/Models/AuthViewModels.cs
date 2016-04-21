using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Umbraco.Web;
using Umbraco.Core.Models;
using System.Globalization;
using Umbraco.Web.Models;

namespace InShow.Models
{


    /// <summary>
    /// Login View Model
    /// </summary>
    public class LoginViewModel
    {
        [DisplayName("Email address")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Please enter your email address")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string EmailAddress { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please enter your password")]
        public string Password { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string ReturnUrl { get; set; }
    }







    /// <summary>
    /// Register Buyer View Model
    /// </summary>
    public class RegisterViewModel
    {
        public RegisterViewModel()
        {
            StepIndex = 0;
            RegisterBuyer = new RegisterBuyer();
            RegisterAgent = new RegisterAgent();
        }

        public bool Previous { get; set; }
        public bool Next { get; set; }
        public int StepIndex { get; set; }

        public string UserType { get; set; }
        public bool Complete { get; set; }

        public RegisterBuyer RegisterBuyer { get; set; }
        public RegisterAgent RegisterAgent { get; set; }
    }



    public class RegisterBuyer
    {

        [Required(ErrorMessage = "Please enter your first name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your surname")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter your cell number")]
        public int Cellphone { get; set; }

        [DisplayName("Email address")]
        [Required(ErrorMessage = "Please enter your email address")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        //[Remote("CheckEmailIsUsed", "AuthSurface", ErrorMessage = "The email address has already been registered")]
        public string EmailAddress { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please enter your password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [DisplayName("Confirm Password")]
        [Required(ErrorMessage = "Please enter your password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Your passwords do not match")]
        public string ConfirmPassword { get; set; }

        //[Required]
        [Remote("CheckProfileURLAvailable", "ProfileSurface", ErrorMessage = "The profile URL is already in use")]
        [DisplayName("Profile URL")]
        public string ProfileURL { get; set; }

    }

    public class RegisterAgent
    {

        [Required(ErrorMessage = "Please enter your first name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your surname")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter your cell number")]
        public int Cellphone { get; set; }

        [Required(ErrorMessage = "Please enter your realestate agency")]
        public string Agency { get; set; }

        [Required(ErrorMessage = "Please enter your realestate agency pin number")]
        public int AgencyPin { get; set; }

        [DisplayName("Email address")]
        [Required(ErrorMessage = "Please enter your email address")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        //[Remote("CheckEmailIsUsed", "AuthSurface", ErrorMessage = "The email address has already been registered")]
        public string EmailAddress { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please enter your password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [DisplayName("Confirm Password")]
        [Required(ErrorMessage = "Please enter your password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Your passwords do not match")]
        public string ConfirmPassword { get; set; }

        //[Required]
        [Remote("CheckProfileURLAvailable", "ProfileSurface", ErrorMessage = "The profile URL is already in use")]
        [DisplayName("Profile URL")]
        public string ProfileURL { get; set; }

        [Required]
        [Display(Name = "I agree to the terms and conditions")]
        //[Compare("IsTrue", ErrorMessage = "Please agree to Terms and Conditions")]
        public bool Agreed { get; set; }

        public bool IsTrue
        { get { return true; } }
    }







    //Forgotten Password View Model
    public class ForgottenPasswordViewModel
    {
        [DisplayName("Email address")]
        [Required(ErrorMessage = "Please enter your email address")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string EmailAddress { get; set; }
    }


    //Reset Password View Model
    public class ResetPasswordViewModel
    {
        [DisplayName("Email address")]
        [Required(ErrorMessage = "Please enter your email address")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string EmailAddress { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please enter your password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [DisplayName("Confirm Password")]
        [Required(ErrorMessage = "Please enter your password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Your passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}