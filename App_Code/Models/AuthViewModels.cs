﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Umbraco.Web;
using Umbraco.Core.Models;
using System.Globalization;
using Umbraco.Web.Models;
using Umbraco.Web.PublishedContentModels;
using AutoMapper.Internal;
using Umbraco.Core.Services;
using Umbraco.Core;


namespace InShow.Models
{

    ////Debugmodeon

    public class CreateUserModel
    {
        [Required]
        [StringLength(6, MinimumLength = 3)]
        [Remote("IsUID_Available", "Validation")]
        [RegularExpression(@"(\S)+", ErrorMessage = "White space is not allowed.")]
        [Editable(true)]
        string UserName { get; set; }
    }





    /// <summary>
    /// Login View Model
    /// </summary>
    public class LoginViewModel
    {
        [DisplayName("Username")]
        [Required(ErrorMessage = "Please enter your Username")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please enter your Password")]
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
            RegisterAgency = new RegisterAgency();
        }
        public RegisterViewModel(int memberId)
        {

            IMemberService service = ApplicationContext.Current.Services.MemberService;
            var member = service.GetById(memberId);
            if (member.ContentTypeAlias == "agent")
            {
                RegisterAgent = new RegisterAgent();
                RegisterAgent.Agency = member.GetValue("agency").ToNullSafeString();
                RegisterAgent.AgencyPin = member.GetValue("agencyPin").ToNullSafeString();
                RegisterAgent.CellNumber = member.GetValue("cellNumber").ToNullSafeString();
                RegisterAgent.EmailAddress = member.Email;
                RegisterAgent.FirstName = member.GetValue("firstName").ToNullSafeString();
                RegisterAgent.LastName = member.GetValue("lastName").ToNullSafeString();
                RegisterAgent.Picture = member.GetValue("picture").ToNullSafeString();
               




            }
            if (member.ContentTypeAlias == "buyer")
            {
                RegisterBuyer = new RegisterBuyer();
            }
            if (member.ContentTypeAlias == "agency")
            {
                RegisterAgency = new RegisterAgency();
            }
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string MemberType { get; set; }
        public bool Previous { get; set; }
        public bool Next { get; set; }
        public int StepIndex { get; set; }

        public string UserType { get; set; }
        public bool Complete { get; set; }

        public RegisterBuyer RegisterBuyer { get; set; }
        public RegisterPrivateSeller RegisterPrivateSeller { get; set; }
        public RegisterAgent RegisterAgent { get; set; }
        public RegisterAgency RegisterAgency { get; set; }

    }



    public class RegisterBuyer
    {

        [DisplayName("User Name")]
        [Required(ErrorMessage = "Please enter your user name")]
        //Regex for validation
        [Remote("CheckUserNameIsUsed", "AuthSurface", HttpMethod = "POST", ErrorMessage = "This username has already been registered")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter your first name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your surname")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter your cell number")]
        [RegularExpression("(\\(0\\d\\d\\)\\s\\d{3}[\\s-]+\\d{4})|(0\\d\\d[\\s-]+\\d{3}[\\s-]+\\d{4})|(0\\d{9})|(\\+\\d\\d\\s?[\\(\\s]\\d\\d[\\)\\s]\\s?\\d{3}[\\s-]?\\d{4})", ErrorMessage = "Please enter a valid South African phone number")]
        public string CellNumber { get; set; }

        [DisplayName("Email address")]
        [Required(ErrorMessage = "Please enter your email address")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Remote("CheckEmailIsUsedBuyer", "AuthSurface", HttpMethod = "POST", ErrorMessage = "The email address has already been registered under a member who is a buyer")]
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

    public class RegisterPrivateSeller
    {

        [DisplayName("User Name")]
        [Required(ErrorMessage = "Please enter your user name")]
        //Regex for validation
        [Remote("CheckUserNameIsUsed", "AuthSurface", HttpMethod = "POST", ErrorMessage = "This username has already been registered")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter your first name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your surname")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter your cell number")]
        [RegularExpression("(\\(0\\d\\d\\)\\s\\d{3}[\\s-]+\\d{4})|(0\\d\\d[\\s-]+\\d{3}[\\s-]+\\d{4})|(0\\d{9})|(\\+\\d\\d\\s?[\\(\\s]\\d\\d[\\)\\s]\\s?\\d{3}[\\s-]?\\d{4})", ErrorMessage = "Please enter a valid South African phone number")]
        public string CellNumber { get; set; }

        [DisplayName("Email address")]
        [Required(ErrorMessage = "Please enter your email address")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Remote("CheckEmailIsUsedSeller", "AuthSurface", ErrorMessage = "The email address has already been registered under a member who is a seller")]
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

    public class RegisterAgent
    {

        [DisplayName("User Name")]
        [Required(ErrorMessage = "Please enter your user name")]
        //Regex for validation
        [Remote("CheckUserNameIsUsed", "AuthSurface", HttpMethod = "POST", ErrorMessage = "This username has already been registered")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter your first name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your surname")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter your cell number")]
        [RegularExpression("(\\(0\\d\\d\\)\\s\\d{3}[\\s-]+\\d{4})|(0\\d\\d[\\s-]+\\d{3}[\\s-]+\\d{4})|(0\\d{9})|(\\+\\d\\d\\s?[\\(\\s]\\d\\d[\\)\\s]\\s?\\d{3}[\\s-]?\\d{4})", ErrorMessage = "Please enter a valid South African phone number")]
        public string CellNumber { get; set; }

        [Required(ErrorMessage = "Please enter your realestate agency")]
        public string Agency { get; set; }

        [Required(ErrorMessage = "Please enter your realestate agency pin number")]
        public string AgencyPin { get; set; }
        public string Picture { get; set; }

        [DisplayName("Email address")]
        [Required(ErrorMessage = "Please enter your email address")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Remote("CheckEmailIsUsedAgent", "AuthSurface", ErrorMessage = "The email address has already been registered under a member who is an agent")]
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

    public class RegisterAgency
    {

        [DisplayName("Agency Name")]
        [Required(ErrorMessage = "Please enter your user name")]
        //Regex for validation
        [Remote("CheckUserNameIsUsed", "AuthSurface", HttpMethod = "POST", ErrorMessage = "This username has already been registered")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter your realestate agency pin number")]
        public string AgencyPin { get; set; }

        public string Logo { get; set; }
        public string AdminAgents { get; set; }
        public int  Credits { get; set; }
        public string Agents { get; set; }

        [Required(ErrorMessage = "Please enter your cell number")]
        [RegularExpression("(\\(0\\d\\d\\)\\s\\d{3}[\\s-]+\\d{4})|(0\\d\\d[\\s-]+\\d{3}[\\s-]+\\d{4})|(0\\d{9})|(\\+\\d\\d\\s?[\\(\\s]\\d\\d[\\)\\s]\\s?\\d{3}[\\s-]?\\d{4})", ErrorMessage = "Please enter a valid South African phone number")]
        public string CellNumber { get; set; }

        [DisplayName("Email address")]
        [Required(ErrorMessage = "Please enter your email address")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Remote("CheckEmailIsUsedAgency", "AuthSurface", ErrorMessage = "The email address has already been registered under a realestate agency")]
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

    
    //Forgotten Password View Model
    public class ForgottenPasswordViewModel
    {
        [DisplayName("Username")]
        [Required(ErrorMessage = "Please enter your username")]
        public string Username { get; set; }

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

    //Reset Password View Model
    public class ResendUsernameViewModel
    {
        [DisplayName("Email address")]
        [Required(ErrorMessage = "Please enter your email address")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string EmailAddress { get; set; }

    }
}