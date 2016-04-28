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
                if (Membership.ValidateUser(model.EmailAddress, model.Password))
                {
                    //Valid credentials

                    //Get the member from their email address
                    var checkMember = membershipService.GetByEmail(model.EmailAddress);

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
                            FormsAuthentication.SetAuthCookie(model.EmailAddress, true);

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
            return PartialView("ForgottenPassword", new ForgottenPasswordViewModel());
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
            var findMember = membershipService.GetByEmail(model.EmailAddress);

            if (findMember != null)
            {
                //We found the member with that email

                //Set expiry date to 
                DateTime expiryTime = DateTime.Now.AddMinutes(15);

                //Lets update resetGUID property
                findMember.Properties["resetGUID"].Value = expiryTime.ToString("ddMMyyyyHHmmssFFFF");

                //Save the member with the up[dated property value
                membershipService.Save(findMember);

                //Send user an email to reset password with GUID in it
                EmailHelper email = new EmailHelper();
                email.SendResetPasswordEmail(findMember.Email, expiryTime.ToString("ddMMyyyyHHmmssFFFF"));
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
            return PartialView("ResetPassword", new ResetPasswordViewModel());
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

            //Get member from email
            var resetMember = membershipService.GetByEmail(model.EmailAddress);

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
                            ModelState.AddModelError("ResetPasswordForm.", "Reset GUID has expired");
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
                return CurrentUmbracoPage();

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
                return CurrentUmbracoPage();
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
                    var createMember = membershipService.CreateMember(model.RegisterBuyer.EmailAddress, model.RegisterBuyer.EmailAddress, model.RegisterBuyer.FirstName + " " + model.RegisterBuyer.LastName, "buyer");

                    //Set the verified email to false
                    createMember.Properties["hasVerifiedEmail"].Value = false;

                    //Save the changes, if we do not do so, we cannot save the password.
                    membershipService.Save(createMember);

                    //Set password on the newly created member
                    membershipService.SavePassword(createMember, model.RegisterBuyer.Password);
                }
                catch (Exception ex)
                {
                    //EG: Duplicate email address - already exists
                    ModelState.AddModelError("memberCreation", ex.Message);

                    return CurrentUmbracoPage();
                }

                //Create temporary GUID
                var tempGUID = Guid.NewGuid();

                //Fetch our new member we created by their email
                var updateMember = membershipService.GetByEmail(model.RegisterBuyer.EmailAddress);

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

                    //Save changes
                    membershipService.Save(updateMember);
                }

                //Send out verification email, with GUID in it
                EmailHelper email = new EmailHelper();
                email.SendVerifyEmail(model.RegisterBuyer.EmailAddress, tempGUID.ToString());

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
                    var createMember = membershipService.CreateMember(model.RegisterPrivateSeller.EmailAddress, model.RegisterPrivateSeller.EmailAddress, model.RegisterPrivateSeller.FirstName + " " + model.RegisterPrivateSeller.LastName, "private");

                    //Set the verified email to false
                    createMember.Properties["hasVerifiedEmail"].Value = false;

                    //Save the changes, if we do not do so, we cannot save the password.
                    membershipService.Save(createMember);

                    //Set password on the newly created member
                    membershipService.SavePassword(createMember, model.RegisterPrivateSeller.Password);
                }
                catch (Exception ex)
                {
                    //EG: Duplicate email address - already exists
                    ModelState.AddModelError("memberCreation", ex.Message);

                    return CurrentUmbracoPage();
                }

                //Create temporary GUID
                var tempGUID = Guid.NewGuid();

                //Fetch our new member we created by their email
                var updateMember = membershipService.GetByEmail(model.RegisterPrivateSeller.EmailAddress);

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

                    //Save changes
                    membershipService.Save(updateMember);
                }

                //Send out verification email, with GUID in it
                EmailHelper email = new EmailHelper();
                email.SendVerifyEmail(model.RegisterPrivateSeller.EmailAddress, tempGUID.ToString());

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
                    var createMember = membershipService.CreateMember(model.RegisterAgent.EmailAddress, model.RegisterAgent.EmailAddress, model.RegisterAgent.FirstName + " " + model.RegisterAgent.LastName, "agent");

                    //Set the verified email to false
                    createMember.Properties["hasVerifiedEmail"].Value = false;

                    //Save the changes, if we do not do so, we cannot save the password.
                    membershipService.Save(createMember);

                    //Set password on the newly created member
                    membershipService.SavePassword(createMember, model.RegisterAgent.Password);
                }
                catch (Exception ex)
                {
                    //EG: Duplicate email address - already exists
                    ModelState.AddModelError("memberCreation", ex.Message);

                    return CurrentUmbracoPage();
                }

                //Create temporary GUID
                var tempGUID = Guid.NewGuid();

                //Fetch our new member we created by their email
                var updateMember = membershipService.GetByEmail(model.RegisterAgent.EmailAddress);

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

                    //Save changes
                    membershipService.Save(updateMember);
                }

                //Send out verification email, with GUID in it
                EmailHelper email = new EmailHelper();
                email.SendVerifyEmail(model.RegisterAgent.EmailAddress, tempGUID.ToString());

                //Update success flag (in a TempData key)
                TempData["IsSuccessful"] = true;

                TempData.Add("CustomMessage", "Your form was successfully submitted at " + DateTime.Now);

                //All done - redirect back to page
                return CurrentUmbracoPage();

            }

            Boolean agencyCheck = false;

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
                    var createMember = membershipService.CreateMember(model.RegisterAgency.EmailAddress, model.RegisterAgency.EmailAddress, model.RegisterAgency.Agency, "agency");

                    //Set the verified email to false
                    createMember.Properties["hasVerifiedEmail"].Value = false;

                    //Save the changes, if we do not do so, we cannot save the password.
                    membershipService.Save(createMember);

                    //Set password on the newly created member
                    membershipService.SavePassword(createMember, model.RegisterAgent.Password);
                }
                catch (Exception ex)
                {
                    //EG: Duplicate email address - already exists
                    ModelState.AddModelError("memberCreation", ex.Message);

                    return CurrentUmbracoPage();
                }

                //Create temporary GUID
                var tempGUID = Guid.NewGuid();

                //Fetch our new member we created by their email
                var updateMember = membershipService.GetByEmail(model.RegisterAgency.EmailAddress);

                //Just to be sure...
                if (updateMember != null)
                {
                    //Set the verification email GUID value on the member
                    updateMember.Properties["emailVerifyGUID"].Value = tempGUID.ToString();

                    //Set the Joined Date label on the member
                    updateMember.Properties["joinedDate"].Value = DateTime.Now.ToString("dd/MM/yyyy @ HH:mm:ss");

                    updateMember.Properties["cellNumber"].Value = model.RegisterAgency.CellNumber;

                    updateMember.Properties["agencyPin"].Value = model.RegisterAgency.AgencyPin;

                    updateMember.Properties["agency"].Value = model.RegisterAgency.Agency;

                    //Put the registration of the agent admin after agent member has been created. Here temporarilly for testing.
                    updateMember.Properties["agent"].Value = model.RegisterAgent.EmailAddress;

                    //Save changes
                    membershipService.Save(updateMember);
                }

                //And cleanup for both agency and agent...------------------------------------------------

                //Send out verification email, with GUID in it
                EmailHelper email = new EmailHelper();
                email.SendVerifyEmail(model.RegisterAgent.EmailAddress, tempGUID.ToString());

                //Update success flag (in a TempData key)
                TempData["IsSuccessful"] = true;

                TempData.Add("CustomMessage", "Your form was successfully submitted at " + DateTime.Now);

                agencyCheck = true;
            }

            if (agencyCheck)
            {
                var membershipService = ApplicationContext.Current.Services.MemberService;

                if (!ModelState.IsValid)
                {
                    //return PartialView("Register", model);
                    return CurrentUmbracoPage();
                }


                //Then Create an Admin Agent...---------------------------------------------------------

                //Model valid let's create the member
                try
                {
                    //Member createMember = Member.MakeNew(model.Name, model.EmailAddress, model.EmailAddress, umbJobMemberType, umbUser);
                    // WARNING: update to your desired MembertypeAlias...
                    var createMember = membershipService.CreateMember(model.RegisterAgent.EmailAddress, model.RegisterAgent.EmailAddress, model.RegisterAgent.Agency, "agent");

                    //Set the verified email to false
                    createMember.Properties["hasVerifiedEmail"].Value = false;

                    //Save the changes, if we do not do so, we cannot save the password.
                    membershipService.Save(createMember);

                    //Set password on the newly created member
                    membershipService.SavePassword(createMember, model.RegisterAgent.Password);
                }
                catch (Exception ex)
                {
                    //EG: Duplicate email address - already exists
                    ModelState.AddModelError("memberCreation", ex.Message);

                    return CurrentUmbracoPage();
                }

                //Create temporary GUID
                var tempGUID = Guid.NewGuid();

                //Fetch our new member we created by their email
                var updateMember = membershipService.GetByEmail(model.RegisterAgent.EmailAddress);

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

                    updateMember.Properties["agencyPin"].Value = model.RegisterAgency.AgencyPin;

                    updateMember.Properties["agency"].Value = model.RegisterAgency.Agency;

                    updateMember.Properties["isAdmin"].Value = true;

                    //Save changes
                    membershipService.Save(updateMember);
                }

                //And cleanup for both agency and agent...------------------------------------------------

                //Send out verification email, with GUID in it
                EmailHelper email = new EmailHelper();
                email.SendVerifyEmail(model.RegisterAgent.EmailAddress, tempGUID.ToString());

                //Update success flag (in a TempData key)
                TempData["IsSuccessful"] = true;

                TempData.Add("CustomMessage", "Your form was successfully submitted at " + DateTime.Now);

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
            return PartialView("VerifyEmail");
        }


        //REMOTE Validation
        /// <summary>
        /// Used with jQuery Validate to check when user registers that email address not already used
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public JsonResult CheckEmailIsUsed(string emailAddress)
        {
            //Try and get member by email typed in
            var checkEmail = Members.GetByEmail(emailAddress);

            if (checkEmail != null)
            {
                return Json(String.Format("The email address '{0}' is already in use.", emailAddress), JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}
