using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using InShow.Helpers;
using InShow.Models;
using Umbraco.Core;
using Umbraco.Core.Persistence.Querying;
using Umbraco.Web.Mvc;
using System.Collections.Generic;
using Umbraco.Core.Models;


namespace InShow.Controllers
{
    public class AuthSurfaceController : SurfaceController
    {

        /// <summary>
        /// Renders the Login view
        /// @Html.Action("RenderLogin","AuthSurface");
        /// </summary>
        /// <returns></returns>
        public ActionResult RenderLogin()
        {
            var loginModel = new LoginViewModel();

            loginModel.ReturnUrl = string.IsNullOrEmpty(HttpContext.Request["ReturnUrl"]) ? "/" : HttpContext.Request["ReturnUrl"];

            return PartialView("LoginPartialView", loginModel);
        }


        /// <summary>
        /// Handles the login form when user posts the form/attempts to login
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HandleLogin(LoginViewModel model)
        {
            model.ReturnUrl = "/dashboard/home";
            var membershipService = ApplicationContext.Current.Services.MemberService;

            if (!ModelState.IsValid)
            {
                //return RedirectToCurrentUmbracoPage();
                return PartialView("Login", model);
            }

            //Member already logged in - redirect to home
            if (Members.IsLoggedIn())
            {
                return Redirect("/dashboard/home");
            }

            //Lets TRY to log the user in
            try
            {
                //Try and login the user...
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    //Valid credentials

                    //Get the member from their email address
                    var checkMember = membershipService.GetByUsername(model.UserName);

                    //Check the member exists
                    if (checkMember != null)
                    {
                        //Let's check they have verified their email address
                        if (Convert.ToBoolean(checkMember.Properties["hasVerifiedEmail"].Value))
                        {
                            //Update number of logins counter
                            int noLogins = 0;
                            if (int.TryParse(checkMember.Properties["numberOfLogins"].Value.ToString(), out noLogins))
                            {
                                //Managed to parse it to a number
                                //Don't need to do anything as we have default value of 0
                            }

                            //Update the counter
                            checkMember.Properties["numberOfLogins"].Value = noLogins + 1;

                            //Update label with last login date to now
                            checkMember.LastLoginDate = DateTime.Now;

                            //Update label with last logged in IP address & Host Name
                            string hostName = Dns.GetHostName();
                            string clientIPAddress = Dns.GetHostAddresses(hostName).GetValue(0).ToString();

                            checkMember.Properties["hostNameOfLastLogin"].Value = hostName;
                            checkMember.Properties["iPofLastLogin"].Value = clientIPAddress;

                            //Save the details
                            membershipService.Save(checkMember);

                            //If they have verified then lets log them in
                            //Set Auth cookie
                            FormsAuthentication.SetAuthCookie(model.UserName, true);

                            //Once logged in - redirect them back to the return URL
                            return new RedirectResult(model.ReturnUrl);
                        }
                        else
                        {
                            //User has not verified their email yet
                            ModelState.AddModelError("LoginForm.", "Email account has not been verified");

                            //Get the verify guid on the member (so we can resend out verification email)
                            var verifyGUID = checkMember.Properties["emailVerifyGUID"].Value.ToString();

                            // TODO: Implement the Email helper/send the Email :)
                            //Get Email Settings from Login Node (current node)
                            //var emailFrom = CurrentPage.GetPropertyValue("emailFrom", "robot@your-site.co.uk").ToString();
                            //var emailSubject = CurrentPage.GetPropertyValue("emailSubject", "CWS - Verify Email").ToString();

                            //Send out verification email, with GUID in it
                            //EmailHelper.SendVerifyEmail(checkMember.Email, emailFrom, emailSubject, verifyGUID);

                            return CurrentUmbracoPage();
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("LoginForm.", "Invalid details");
                    return CurrentUmbracoPage();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("LoginForm.", "Error: " + ex.ToString());
                return CurrentUmbracoPage();
            }

            //In theory should never hit this, but you never know...
            return RedirectToCurrentUmbracoPage();
        }

        //Used with an ActionLink
        //@Html.ActionLink("Logout", "Logout", "AuthSurface")

        public ActionResult Logout()
        {
            //Member already logged in, lets log them out and redirect them home
            if (Members.IsLoggedIn())
            {
                //Log member out
                FormsAuthentication.SignOut();

                //Redirect home
                return Redirect("/");
            }
            else
            {
                //Redirect home
                return Redirect("/");
            }
        }

        /// <summary>
        /// Renders the Forgotten Password view
        /// @Html.Action("RenderForgottenPassword","AuthSurface");
        /// </summary>
        /// <returns></returns>
        public ActionResult RenderForgottenPassword()
        {
            return PartialView("ForgottenPasswordPartial", new ForgottenPasswordViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HandleForgottenPassword(ForgottenPasswordViewModel model)
        {
            var membershipService = ApplicationContext.Current.Services.MemberService;

            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            //Find the member with the email address
            var findMember = membershipService.GetByUsername(model.Username);

            if (findMember != null)
            {
                //We found the member with that email

                //Set expiry date to 
                DateTime expiryTime = DateTime.Now.AddMinutes(15);

                //Lets update resetGUID property
                findMember.Properties["resetGUID"].Value = expiryTime.ToString("ddMMyyyyHHmmssFFFF");

                //Save the member with the updated property value
                membershipService.Save(findMember);


                //Send Reset Password Email

                //Create temporary GUID
                var resetGUID = expiryTime.ToString("ddMMyyyyHHmmssFFFF");

                //Fetch our new member we created by their email
                var updateMember = membershipService.GetByUsername(model.Username);

                String MemberName = model.Username;

                //Verify link
                string baseURL = System.Web.HttpContext.Current.Request.Url.AbsoluteUri.Replace(System.Web.HttpContext.Current.Request.Url.AbsolutePath, string.Empty);
                var resetURL = baseURL + "/reset-password?resetGUID=" + resetGUID.ToString() + "&user=" + MemberName.ToString();

                List<PerplexMail.EmailTag> listOfTags = new List<PerplexMail.EmailTag>();
                listOfTags.Add(new PerplexMail.EmailTag("[#email#]", findMember.Email));
                listOfTags.Add(new PerplexMail.EmailTag("[#name#]", MemberName));
                listOfTags.Add(new PerplexMail.EmailTag("[#GUID#]", resetURL));


                var contentService = ApplicationContext.Current.Services.ContentService;

                var Emails = contentService.GetRootContent().Where(x => x.Name.ToString() == "PerplexMail").First().Descendants().Where(x => x.Name == "Reset Password Email").First();

                int emailNodeToSend = Emails.Id; //umbraco Email node ID.

                PerplexMail.Email.SendUmbracoEmail(emailNodeToSend, listOfTags);

                //Update success flag (in a TempData key)
                TempData["IsSuccessful"] = true;

                //Send user an email to reset password with GUID in it
                //EmailHelper email = new EmailHelper();
                //email.SendResetPasswordEmail(findMember.Email, expiryTime.ToString("ddMMyyyyHHmmssFFFF"));

            }
            else
            {
                ModelState.AddModelError("ForgottenPasswordForm.", "No member found");
                return CurrentUmbracoPage();
            }

            return RedirectToCurrentUmbracoPage();
        }

        /// <summary>
        /// Renders the Reset Password View
        /// @Html.Action("RenderResetPassword","AuthSurface");
        /// </summary>
        /// <returns></returns>
        public ActionResult RenderResetPassword()
        {
            return PartialView("ResetPasswordPartial", new ResetPasswordViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HandleResetPassword(ResetPasswordViewModel model)
        {
            var membershipService = ApplicationContext.Current.Services.MemberService;

            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            var username = Request.QueryString["user"];

            //Get member from email
            var resetMember = membershipService.GetByUsername(username);

            //Ensure we have that member
            if (resetMember != null)
            {
                //Get the querystring GUID
                var resetQueryString = Request.QueryString["resetGUID"];

                //Ensure we have a vlaue in QS
                if (!string.IsNullOrEmpty(resetQueryString))
                {
                    //See if the QS matches the value on the member property
                    if (resetMember.Properties["resetGUID"].Value.ToString() == resetQueryString)
                    {

                        //Got a match, now check to see if the 15min window hasnt expired
                        DateTime expiryTime = DateTime.ParseExact(resetQueryString, "ddMMyyyyHHmmssFFFF", null);

                        //Check the current time is less than the expiry time
                        DateTime currentTime = DateTime.Now;

                        //Check if date has NOT expired (been and gone)
                        if (currentTime.CompareTo(expiryTime) < 0)
                        {
                            //Got a match, we can allow user to update password
                            //resetMember.RawPasswordValue.Password = model.Password;
                            membershipService.SavePassword(resetMember, model.Password);

                            //Remove the resetGUID value
                            resetMember.Properties["resetGUID"].Value = string.Empty;

                            //Save the member
                            membershipService.Save(resetMember);

                            return Redirect("/login");
                        }
                        else
                        {
                            //ERROR: Reset GUID has expired
                            ModelState.AddModelError("ResetPasswordForm.", "Your password reset has has expired, please try again");
                            return CurrentUmbracoPage();
                        }
                    }
                    else
                    {
                        //ERROR: QS does not match what is stored on member property
                        //Invalid GUID
                        ModelState.AddModelError("ResetPasswordForm.", "Invalid GUID");
                        return CurrentUmbracoPage();
                    }
                }
                else
                {
                    //ERROR: No QS present
                    //Invalid GUID
                    ModelState.AddModelError("ResetPasswordForm.", "Invalid GUID");
                    return CurrentUmbracoPage();
                }
            }

            return RedirectToCurrentUmbracoPage();
        }


        /// <summary>
        /// Renders the Reset Password View
        /// @Html.Action("RenderResendUsername","AuthSurface");
        /// </summary>
        /// <returns></returns>
        public ActionResult RenderResendUsername()
        {
            return PartialView("ResendUsernamePartial", new ResendUsernameViewModel());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HandleResendUsername(ResendUsernameViewModel model)
        {

            var membershipService = ApplicationContext.Current.Services.MemberService;

            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            //Find the member with the email address
            var findMember = membershipService.GetByEmail(model.EmailAddress);

            if (findMember != null)
            {
                //We found the member with that email

                String BuyerUsername = "";
                String SellerUsername = "";
                String AgentUsername = "";
                String AgencyUsername = "";


                //member loop
                var members = membershipService.GetAllMembers();

                foreach (Member m in members)
                {

                    if (m.Email == model.EmailAddress)
                    {
                        if (m.ContentTypeAlias == "buyer")
                        {
                            BuyerUsername = m.Username.ToString() + "</br></br>";
                        }
                        if (m.ContentTypeAlias == "private")
                        {
                            SellerUsername = m.Username.ToString() + "</br></br>";
                        }
                        if (m.ContentTypeAlias == "agent")
                        {
                            AgentUsername = m.Username.ToString() + "</br></br>";
                        }
                        if (m.ContentTypeAlias == "agency")
                        {
                            AgencyUsername = m.Username.ToString() + "</br></br>";
                        }
                    }

                }

                String BufferTextBuyer = "";
                String BufferTextSeller = "";
                String BufferTextAgent = "";
                String BufferTextAgency = "";

                if (BuyerUsername != "")
                {
                    BufferTextBuyer = "Your Buyer Username is: </br>";
                }
                if (SellerUsername != "")
                {
                    BufferTextSeller = "Your Private Seller Username is: </br>";
                }
                if (AgentUsername != "")
                {
                    BufferTextAgent = "Your Agent Username is: </br>";
                }
                if (AgencyUsername != "")
                {
                    BufferTextAgency = "Your Agency Username is: </br>";
                }

                var username = findMember.Username.ToString();

                List<PerplexMail.EmailTag> listOfTags = new List<PerplexMail.EmailTag>();
                listOfTags.Add(new PerplexMail.EmailTag("[#email#]", findMember.Email));

                listOfTags.Add(new PerplexMail.EmailTag("[#bufferTextBuyer#]", BufferTextBuyer));
                listOfTags.Add(new PerplexMail.EmailTag("[#bufferTextSeller#]", BufferTextSeller));
                listOfTags.Add(new PerplexMail.EmailTag("[#bufferTextAgent#]", BufferTextAgent));
                listOfTags.Add(new PerplexMail.EmailTag("[#bufferTextAgency#]", BufferTextAgency));

                listOfTags.Add(new PerplexMail.EmailTag("[#buyername#]", BuyerUsername));
                listOfTags.Add(new PerplexMail.EmailTag("[#sellername#]", SellerUsername));
                listOfTags.Add(new PerplexMail.EmailTag("[#agentname#]", AgentUsername));
                listOfTags.Add(new PerplexMail.EmailTag("[#agencyname#]", AgencyUsername));


                var contentService = ApplicationContext.Current.Services.ContentService;

                var Emails = contentService.GetRootContent().Where(x => x.Name.ToString() == "PerplexMail").First().Descendants().Where(x => x.Name == "Resend Username Email").First();

                int emailNodeToSend = Emails.Id; //umbraco Email node ID.

                PerplexMail.Email.SendUmbracoEmail(emailNodeToSend, listOfTags);

                //Update success flag (in a TempData key)
                TempData["IsSuccessful"] = true;

            }
            else
            {
                ModelState.AddModelError("ForgottenPasswordForm.", "No member found");
                return CurrentUmbracoPage();
            }

            return RedirectToCurrentUmbracoPage();


        }

        /// <summary>
        /// Renders the Register View
        /// @Html.Action("RenderRegister","AuthSurface");
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public ActionResult RenderRegister(RegisterViewModel model)
        {

            return PartialView("Register", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HandleRegister(RegisterViewModel model)
        {


            //create email id


            var contentService = ApplicationContext.Current.Services.ContentService;

            var Emails = contentService.GetRootContent().Where(x => x.Name.ToString() == "PerplexMail").First().Descendants().Where(x => x.Name == "Register Member Email").First();

            int emailNodeToSend = Emails.Id; //umbraco Email node ID.


            ViewBag.AgencyDropList = new SelectList(new[] { "myVal1", "myVal2", "myVal3" });


            model = model ?? new RegisterViewModel();

            if (model.UserType == "Buyer")
            {
                model.StepIndex = 1;
            }

            if (model.UserType == "PrivateSeller")
            {
                model.StepIndex = 2;
            }

            if (model.UserType == "Agent")
            {
                model.StepIndex = 3;
            }

            if (model.UserType == "Agency")
            {
                model.StepIndex = 4;
            }



            //ignore validation or saving data when going backwards
            if (model.Previous)
                return RedirectToCurrentUmbracoPage();

            var validationStep = string.Empty;

            switch (model.StepIndex)
            {

                case 1:
                    validationStep = "RegisterBuyer";
                    break;
                case 2:
                    validationStep = "RegisterPrivateSeller";
                    break;
                case 3:
                    validationStep = "RegisterAgent";
                    break;
                case 4:
                    validationStep = "RegisterAgency";
                    break;

            }

            //remove all errors except for the current step
            foreach (var key in ModelState.Keys.Where(k => k.StartsWith(string.Format("{0}.", validationStep)) == false))
            {
                ModelState[key].Errors.Clear();
            }


            if (!ModelState.IsValid)
            {
                //return PartialView("Register", model);
                return null;
            }

            //Its the final step, do some saving
            if (model.StepIndex == 0)
            {

                //TODO: Do something with the form data

                return RedirectToCurrentUmbracoPage();
            }

            //Its the final step, do some saving



            //BUYER CONTROLLER
            if (model.StepIndex == 1)
            {

                var membershipService = ApplicationContext.Current.Services.MemberService;

                if (!ModelState.IsValid)
                {
                    //return PartialView("Register", model);
                    return CurrentUmbracoPage();
                }

                //Model valid let's create the member
                try
                {
                    //Member createMember = Member.MakeNew(model.Name, model.EmailAddress, model.EmailAddress, umbJobMemberType, umbUser);
                    // WARNING: update to your desired MembertypeAlias...
                    var createMember = membershipService.CreateMember(model.RegisterBuyer.UserName, model.RegisterBuyer.EmailAddress, model.RegisterBuyer.FirstName + " " + model.RegisterBuyer.LastName, "buyer");

                    //Set the verified email to false
                    createMember.Properties["hasVerifiedEmail"].Value = false;

                    //Save the changes, if we do not do so, we cannot save the password.
                    membershipService.Save(createMember);

                    //Set password on the newly created member
                    membershipService.SavePassword(createMember, model.RegisterBuyer.Password);

                    //Send verification Email

                    //Create temporary GUID
                    var tempGUID = Guid.NewGuid();

                    //Fetch our new member we created by their email
                    var updateMember = membershipService.GetByUsername(model.RegisterBuyer.UserName);

                    //Just to be sure...
                    if (updateMember != null)
                    {
                        //Set the verification email GUID value on the member
                        updateMember.Properties["emailVerifyGUID"].Value = tempGUID.ToString();

                        //Set the Joined Date label on the member
                        updateMember.Properties["joinedDate"].Value = DateTime.Now.ToString("dd/MM/yyyy @ HH:mm:ss");

                        updateMember.Properties["firstName"].Value = model.RegisterBuyer.FirstName;

                        updateMember.Properties["lastName"].Value = model.RegisterBuyer.LastName;

                        updateMember.Properties["cellNumber"].Value = model.RegisterBuyer.CellNumber;

                        updateMember.Properties["email"].Value = model.RegisterBuyer.EmailAddress;

                        updateMember.Properties["numberOfLogins"].Value = 0;



                        //Save changes
                        membershipService.Save(updateMember);
                    }


                    String BuyerName = model.RegisterBuyer.FirstName + " " + model.RegisterBuyer.LastName;
                    //Verify link
                    string baseURL = System.Web.HttpContext.Current.Request.Url.AbsoluteUri.Replace(System.Web.HttpContext.Current.Request.Url.AbsolutePath, string.Empty);
                    var verifyURL = baseURL + "/verify-email?verifyGUID=" + tempGUID.ToString();

                    List<PerplexMail.EmailTag> listOfTags = new List<PerplexMail.EmailTag>();
                    listOfTags.Add(new PerplexMail.EmailTag("[#email#]", model.RegisterBuyer.EmailAddress));
                    listOfTags.Add(new PerplexMail.EmailTag("[#name#]", BuyerName));
                    listOfTags.Add(new PerplexMail.EmailTag("[#GUID#]", verifyURL));
                    listOfTags.Add(new PerplexMail.EmailTag("[#buysell#]", "buying"));

                    PerplexMail.Email.SendUmbracoEmail(emailNodeToSend, listOfTags);

                }
                catch (Exception ex)
                {
                    //EG: Duplicate email address - already exists
                    ModelState.AddModelError("memberCreation", ex.Message);

                    return CurrentUmbracoPage();
                }

                //Send out verification email, with GUID in it
                //EmailHelper email = new EmailHelper();
                //email.SendVerifyEmail(model.RegisterBuyer.EmailAddress, tempGUID.ToString());

                //Update success flag (in a TempData key)
                TempData["IsSuccessful"] = true;

                TempData.Add("CustomMessage", "Your form was successfully submitted at " + DateTime.Now);


                //All done - redirect back to page
                return CurrentUmbracoPage();

            }

            //Its the final step, do some saving

            //PRIVATE SELLER CONTROLLER
            if (model.StepIndex == 2)
            {

                var membershipService = ApplicationContext.Current.Services.MemberService;

                if (!ModelState.IsValid)
                {
                    //return PartialView("Register", model);
                    return CurrentUmbracoPage();
                }

                //Model valid let's create the member
                try
                {
                    //Member createMember = Member.MakeNew(model.Name, model.EmailAddress, model.EmailAddress, umbJobMemberType, umbUser);
                    // WARNING: update to your desired MembertypeAlias...
                    var createMember = membershipService.CreateMember(model.RegisterPrivateSeller.UserName, model.RegisterPrivateSeller.EmailAddress, model.RegisterPrivateSeller.FirstName + " " + model.RegisterPrivateSeller.LastName, "private");

                    //Set the verified email to false
                    createMember.Properties["hasVerifiedEmail"].Value = false;

                    //Save the changes, if we do not do so, we cannot save the password.
                    membershipService.Save(createMember);

                    //Set password on the newly created member
                    membershipService.SavePassword(createMember, model.RegisterPrivateSeller.Password);

                    //Send verification Email

                    //Create temporary GUID
                    var tempGUID = Guid.NewGuid();

                    //Fetch our new member we created by their email
                    var updateMember = membershipService.GetByUsername(model.RegisterPrivateSeller.UserName);

                    //Just to be sure...
                    if (updateMember != null)
                    {
                        //Set the verification email GUID value on the member
                        updateMember.Properties["emailVerifyGUID"].Value = tempGUID.ToString();

                        //Set the Joined Date label on the member
                        updateMember.Properties["joinedDate"].Value = DateTime.Now.ToString("dd/MM/yyyy @ HH:mm:ss");

                        updateMember.Properties["firstName"].Value = model.RegisterPrivateSeller.FirstName;

                        updateMember.Properties["lastName"].Value = model.RegisterPrivateSeller.LastName;

                        updateMember.Properties["cellNumber"].Value = model.RegisterPrivateSeller.CellNumber;

                        updateMember.Properties["email"].Value = model.RegisterPrivateSeller.EmailAddress;

                        updateMember.Properties["numberOfLogins"].Value = 0;


                        //Save changes
                        membershipService.Save(updateMember);
                    }

                    String PrivateSellerName = model.RegisterPrivateSeller.FirstName + " " + model.RegisterPrivateSeller.LastName;
                    //Verify link
                    string baseURL = System.Web.HttpContext.Current.Request.Url.AbsoluteUri.Replace(System.Web.HttpContext.Current.Request.Url.AbsolutePath, string.Empty);
                    var verifyURL = baseURL + "/verify-email?verifyGUID=" + tempGUID.ToString();

                    List<PerplexMail.EmailTag> listOfTags = new List<PerplexMail.EmailTag>();
                    listOfTags.Add(new PerplexMail.EmailTag("[#email#]", model.RegisterPrivateSeller.EmailAddress));
                    listOfTags.Add(new PerplexMail.EmailTag("[#name#]", PrivateSellerName));
                    listOfTags.Add(new PerplexMail.EmailTag("[#GUID#]", verifyURL));
                    listOfTags.Add(new PerplexMail.EmailTag("[#buysell#]", "selling"));

                    PerplexMail.Email.SendUmbracoEmail(emailNodeToSend, listOfTags);

                }
                catch (Exception ex)
                {
                    //EG: Duplicate email address - already exists
                    ModelState.AddModelError("memberCreation", ex.Message);

                    return CurrentUmbracoPage();
                }

                //Send out verification email, with GUID in it
                //EmailHelper email = new EmailHelper();
                //email.SendVerifyEmail(model.RegisterPrivateSeller.EmailAddress, tempGUID.ToString());

                //Update success flag (in a TempData key)
                TempData["IsSuccessful"] = true;

                TempData.Add("CustomMessage", "Your form was successfully submitted at " + DateTime.Now);


                //All done - redirect back to page
                return CurrentUmbracoPage();

            }

            //Its the final step, do some saving

            //AGENT CONTROLLER
            if (model.StepIndex == 3)
            {

                var membershipService = ApplicationContext.Current.Services.MemberService;

                if (!ModelState.IsValid)
                {
                    //return PartialView("Register", model);
                    return CurrentUmbracoPage();
                }

                //Model valid let's create the member
                try
                {
                    //Member createMember = Member.MakeNew(model.Name, model.EmailAddress, model.EmailAddress, umbJobMemberType, umbUser);
                    // WARNING: update to your desired MembertypeAlias...
                    var createMember = membershipService.CreateMember(model.RegisterAgent.UserName, model.RegisterAgent.EmailAddress, model.RegisterAgent.FirstName + " " + model.RegisterAgent.LastName, "agent");

                    //Set the verified email to false
                    createMember.Properties["hasVerifiedEmail"].Value = false;

                    //Save the changes, if we do not do so, we cannot save the password.
                    membershipService.Save(createMember);

                    //Set password on the newly created member
                    membershipService.SavePassword(createMember, model.RegisterAgent.Password);

                    //Send verification Email

                    //Create temporary GUID
                    var tempGUID = Guid.NewGuid();

                    //Fetch our new member we created by their email
                    var updateMember = membershipService.GetByUsername(model.RegisterAgent.UserName);

                    //Just to be sure...
                    if (updateMember != null)
                    {
                        //Set the verification email GUID value on the member
                        updateMember.Properties["emailVerifyGUID"].Value = tempGUID.ToString();

                        //Set the Joined Date label on the member
                        updateMember.Properties["joinedDate"].Value = DateTime.Now.ToString("dd/MM/yyyy @ HH:mm:ss");

                        updateMember.Properties["firstName"].Value = model.RegisterAgent.FirstName;

                        updateMember.Properties["lastName"].Value = model.RegisterAgent.LastName;

                        updateMember.Properties["cellNumber"].Value = model.RegisterAgent.CellNumber;

                        updateMember.Properties["agencyPin"].Value = model.RegisterAgent.AgencyPin;

                        updateMember.Properties["agency"].Value = model.RegisterAgent.Agency;

                        updateMember.Properties["email"].Value = model.RegisterAgent.EmailAddress;

                        updateMember.Properties["numberOfLogins"].Value = 0;


                        //Save changes
                        membershipService.Save(updateMember);
                    }


                    String AgentName = model.RegisterAgent.FirstName + " " + model.RegisterAgent.LastName;
                    //Verify link
                    string baseURL = System.Web.HttpContext.Current.Request.Url.AbsoluteUri.Replace(System.Web.HttpContext.Current.Request.Url.AbsolutePath, string.Empty);
                    var verifyURL = baseURL + "/verify-email?verifyGUID=" + tempGUID.ToString();

                    List<PerplexMail.EmailTag> listOfTags = new List<PerplexMail.EmailTag>();
                    listOfTags.Add(new PerplexMail.EmailTag("[#email#]", model.RegisterAgent.EmailAddress));
                    listOfTags.Add(new PerplexMail.EmailTag("[#name#]", AgentName));
                    listOfTags.Add(new PerplexMail.EmailTag("[#GUID#]", verifyURL));
                    listOfTags.Add(new PerplexMail.EmailTag("[#buysell#]", "selling"));

                    PerplexMail.Email.SendUmbracoEmail(emailNodeToSend, listOfTags);

                }
                catch (Exception ex)
                {
                    //EG: Duplicate email address - already exists
                    ModelState.AddModelError("memberCreation", ex.Message);

                    return CurrentUmbracoPage();
                }

                //Send out verification email, with GUID in it
                //EmailHelper email = new EmailHelper();
                //email.SendVerifyEmail(model.RegisterAgent.EmailAddress, tempGUID.ToString());

                //Update success flag (in a TempData key)
                TempData["IsSuccessful"] = true;

                TempData.Add("CustomMessage", "Your form was successfully submitted at " + DateTime.Now);

                //All done - redirect back to page
                return CurrentUmbracoPage();

            }

            //Boolean agencyCheck = false;

            //Its the final step, do some saving
            if (model.StepIndex == 4)
            {
                var membershipService = ApplicationContext.Current.Services.MemberService;

                if (!ModelState.IsValid)
                {
                    //return PartialView("Register", model);
                    return CurrentUmbracoPage();
                }


                //First Create Agency...---------------------------------------------------------

                //Model valid let's create the member
                try
                {
                    //Member createMember = Member.MakeNew(model.Name, model.EmailAddress, model.EmailAddress, umbJobMemberType, umbUser);
                    // WARNING: update to your desired MembertypeAlias...
                    var createMember = membershipService.CreateMember(model.RegisterAgency.Name, model.RegisterAgency.EmailAddress, model.RegisterAgency.Name, "agency");


                    //Set the verified email to false
                    createMember.Properties["hasVerifiedEmail"].Value = false;

                    //Save the changes, if we do not do so, we cannot save the password.
                    membershipService.Save(createMember);

                    //Set password on the newly created member
                    membershipService.SavePassword(createMember, model.RegisterAgency.Password);

                    //Send verification Email

                    //Create temporary GUID
                    var tempGUID = Guid.NewGuid();

                    //Fetch our new member we created by their email
                    var updateMember = membershipService.GetByUsername(model.RegisterAgency.Name);

                    //Just to be sure...
                    if (updateMember != null)
                    {
                        //Set the verification email GUID value on the member
                        updateMember.Properties["emailVerifyGUID"].Value = tempGUID.ToString();

                        //Set the Joined Date label on the member
                        updateMember.Properties["joinedDate"].Value = DateTime.Now.ToString("dd/MM/yyyy @ HH:mm:ss");

                        updateMember.Properties["cellNumber"].Value = model.RegisterAgency.CellNumber;

                        updateMember.Properties["agencyPin"].Value = model.RegisterAgency.AgencyPin;

                        updateMember.Properties["agencyName"].Value = model.RegisterAgency.Name;

                        //Put the registration of the agent admin after agent member has been created. Here temporarilly for testing.
                        updateMember.Properties["agencyAdminEmail"].Value = model.RegisterAgent.EmailAddress;

                        updateMember.Properties["numberOfLogins"].Value = 0;


                        //Save changes
                        membershipService.Save(updateMember);
                    }


                    String AgencyName = model.RegisterAgency.Name;
                    //Verify link
                    string baseURL = System.Web.HttpContext.Current.Request.Url.AbsoluteUri.Replace(System.Web.HttpContext.Current.Request.Url.AbsolutePath, string.Empty);
                    var verifyURL = baseURL + "/verify-email?verifyGUID=" + tempGUID.ToString();

                    List<PerplexMail.EmailTag> listOfTags = new List<PerplexMail.EmailTag>();
                    listOfTags.Add(new PerplexMail.EmailTag("[#email#]", model.RegisterAgency.EmailAddress));
                    listOfTags.Add(new PerplexMail.EmailTag("[#name#]", AgencyName));
                    listOfTags.Add(new PerplexMail.EmailTag("[#GUID#]", verifyURL));
                    listOfTags.Add(new PerplexMail.EmailTag("[#buysell#]", "selling"));

                    PerplexMail.Email.SendUmbracoEmail(emailNodeToSend, listOfTags);

                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.ToString());
                    //EG: Duplicate email address - already exists
                    ModelState.AddModelError("memberCreation", ex.Message);

                    return CurrentUmbracoPage();
                }

                //And cleanup for both agency and agent...------------------------------------------------

                //Send out verification email, with GUID in it
                //EmailHelper email = new EmailHelper();
                //email.SendVerifyEmail(model.RegisterAgent.EmailAddress, tempGUID.ToString());

                //Update success flag (in a TempData key)
                TempData["IsSuccessful"] = true;

                TempData.Add("CustomMessage", "Your form was successfully submitted at " + DateTime.Now);

                //agencyCheck = true;
            }

            //All done - redirect back to page
            return CurrentUmbracoPage();
        }


        /// <summary>
        /// Renders the Verify Email
        /// @Html.Action("RenderVerifyEmail","AuthSurface");
        /// </summary>
        /// <returns></returns>
        public ActionResult RenderVerifyEmail(string verifyGUID)
        {
            var membershipService = ApplicationContext.Current.Services.MemberService;

            //Auto binds and gets guid from the querystring
            var findMember = membershipService.GetMembersByPropertyValue("emailVerifyGUID", verifyGUID, StringPropertyMatchType.Exact).SingleOrDefault();
            //Member findMember = Member.GetAllAsList().SingleOrDefault(x => x.getProperty("emailVerifyGUID").Value.ToString() == verifyGUID);

            //Ensure we find a member with the verifyGUID
            if (findMember != null)
            {
                //We got the member, so let's update the verify email checkbox
                findMember.Properties["hasVerifiedEmail"].Value = true;

                //Save the member
                membershipService.Save(findMember);
            }
            else
            {
                //Update success flag (in a TempData key)
                TempData["IsSuccessful"] = false;

                //Couldn't find them - most likely invalid GUID
                return PartialView("VerifyEmail");
            }

            //Update success flag (in a TempData key)
            TempData["IsSuccessful"] = true;

            //All sorted let's redirect to root/homepage
            return PartialView("VerifyEmailPartial");
        }


        //REMOTE Validation
        /// <summary>
        /// Used with jQuery Validate to check when user registers that email address not already used
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public JsonResult CheckEmailIsUsedBuyer()
        {
            foreach (String key in Request.Params.AllKeys)
            {
                if (key.Contains("EmailAddress"))
                {
                    var checkEmail = Members.GetByEmail(Request.Params[key]);

                    if (checkEmail != null) { }

                    var membershipService = ApplicationContext.Current.Services.MemberService;
                    var members = membershipService.GetAllMembers();

                    foreach (var m in members)
                    {
                        if (m.ContentTypeAlias == "buyer")
                        {
                            return Json(String.Format("The email address '{0}' has already been registered under a member who is a buyer.", Request.Params[key].ToString()), JsonRequestBehavior.AllowGet);

                        }

                    }

                }

            }
            //Try and get member by email typed in

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        //REMOTE Validation
        /// <summary>
        /// Used with jQuery Validate to check when user registers that email address not already used
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public JsonResult CheckEmailIsUsedSeller()
        {
            foreach (String key in Request.Params.AllKeys)
            {
                if (key.Contains("EmailAddress"))
                {
                    var checkEmail = Members.GetByEmail(Request.Params[key]);

                    if (checkEmail != null) { }

                    var membershipService = ApplicationContext.Current.Services.MemberService;
                    var members = membershipService.GetAllMembers();

                    foreach (var m in members)
                    {
                        if (m.ContentTypeAlias == "private")
                        {
                            return Json(String.Format("The email address '{0}' has already been registered under a member who is a seller.", Request.Params[key].ToString()), JsonRequestBehavior.AllowGet);

                        }

                    }

                }

            }
            //Try and get member by email typed in

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        //REMOTE Validation
        /// <summary>
        /// Used with jQuery Validate to check when user registers that email address not already used
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public JsonResult CheckEmailIsUsedAgent()
        {
            foreach (String key in Request.Params.AllKeys)
            {
                if (key.Contains("EmailAddress"))
                {
                    var checkEmail = Members.GetByEmail(Request.Params[key]);

                    if (checkEmail != null) { }

                    var membershipService = ApplicationContext.Current.Services.MemberService;
                    var members = membershipService.GetAllMembers();

                    foreach (var m in members)
                    {
                        if (m.ContentTypeAlias == "agent")
                        {
                            return Json(String.Format("The email address '{0}' has already been registered under a member who is an agent.", Request.Params[key].ToString()), JsonRequestBehavior.AllowGet);

                        }

                    }

                }

            }
            //Try and get member by email typed in

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        //REMOTE Validation
        /// <summary>
        /// Used with jQuery Validate to check when user registers that email address not already used
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public JsonResult CheckEmailIsUsedAgency()
        {
            foreach (String key in Request.Params.AllKeys)
            {
                if (key.Contains("EmailAddress"))
                {
                    var checkEmail = Members.GetByEmail(Request.Params[key]);

                    if (checkEmail != null) { }

                    var membershipService = ApplicationContext.Current.Services.MemberService;
                    var members = membershipService.GetAllMembers();

                    foreach (var m in members)
                    {
                        if (m.ContentTypeAlias == "agency")
                        {
                            return Json(String.Format("The email address '{0}' has already been registered under an agency.", Request.Params[key].ToString()), JsonRequestBehavior.AllowGet);

                        }

                    }

                }

            }
            //Try and get member by email typed in

            return Json(true, JsonRequestBehavior.AllowGet);
        }









        //check if user name has already been used
        public JsonResult CheckUserNameIsUsed()
        {

            var membershipService = ApplicationContext.Current.Services.MemberService;

            //membershipService.FindByUsername


            foreach (String key in Request.Params.AllKeys)
            {
                if (key.Contains("UserName"))
                {
                    var checkUser = Members.GetByUsername(Request.Params[key]);
                    if (checkUser != null)
                    {
                        return Json(String.Format("The username '{0}' is already in use.", Request.Params[key].ToString()), JsonRequestBehavior.AllowGet);
                    }
                }
            }
            //Try and get member by email typed in

            return Json(true, JsonRequestBehavior.AllowGet);
        }


        public JsonResult IsUID_Available(string Username)
        {

            return Json(true, JsonRequestBehavior.AllowGet);
        }

    }
}
