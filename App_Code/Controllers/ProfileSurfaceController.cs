﻿using System;
using System.Linq;
using System.Web.Mvc;
using InShow.Models;
using umbraco.cms.businesslogic.member;
using Umbraco.Core.Persistence.Querying;
using Umbraco.Web.Mvc;
using System.Collections.Generic;
using System.Web;
using System.IO;

namespace InShow.Controllers
{
    public class ProfileSurfaceController : SurfaceController
    {






        [HttpPost]
        public ActionResult Multiple(IEnumerable<HttpPostedFileBase> files)
        {
            foreach (var file in files)
            {
                if (file != null && file.ContentLength > 0)
                {
                    file.SaveAs(Path.Combine(Server.MapPath("/uploads"), Guid.NewGuid() + Path.GetExtension(file.FileName)));
                }
            }
            return View();
        }
        /// <summary>
        /// Renders the Login view
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult RenderEditProfile()
        {
            ProfileViewModel profileModel = new ProfileViewModel();

            var membershipService = ApplicationContext.Services.MemberService;

            //If user is logged in then let's pre-populate the model
            if (Members.IsLoggedIn())
            {
                //Let's fill it up
                var currentMember = membershipService.GetById(Members.GetCurrentMemberId());

                profileModel.Name           = currentMember.Name;
                profileModel.EmailAddress   = currentMember.Email;
                profileModel.MemberID       = currentMember.Id;
                profileModel.Description    = currentMember.Properties["description"].Value.ToString();
                profileModel.ProfileURL     = currentMember.Properties["profileURL"].Value.ToString();
                profileModel.Twitter        = currentMember.Properties["twitter"].Value.ToString();
                profileModel.LinkedIn       = currentMember.Properties["linkedIn"].Value.ToString();
                profileModel.Skype          = currentMember.Properties["skype"].Value.ToString();
            }
            else
            {
                //They are not logged in, redirect to home
                return Redirect("/");
            }

            //Pass the model to the view
            return PartialView("EditProfile", profileModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HandleEditProfile(ProfileViewModel model)
        {
            var membershipService = ApplicationContext.Services.MemberService;

            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage(); //PartialView("EditProfile", model);
            }

            //Update the member with our data & save it down
            //Using member ID and not email address in case member has changed their email
            //Member updateMember = new Member(model.MemberID);
            var updateMember = membershipService.GetById(model.MemberID);

            updateMember.Name                               = model.Name;
            updateMember.Email                              = model.EmailAddress;
            updateMember.Properties["description"].Value    = model.Description;
            updateMember.Properties["profileURL"].Value     = model.ProfileURL;
            updateMember.Properties["twitter"].Value        = model.Twitter;
            updateMember.Properties["linkedIn"].Value       = model.LinkedIn;
            updateMember.Properties["skype"].Value          = model.Skype;

            //Save the member
            membershipService.Save(updateMember);

            //Update success flag (in a TempData key)
            TempData["IsSuccessful"] = true;

            //Return the view
            //return PartialView("EditProfile", model);
            //Return the view
            return RedirectToCurrentUmbracoPage();
        }

        public ActionResult RenderMemberProfile()
        {
            var membershipService = ApplicationContext.Services.MemberService;

            //Try and find member with the QueryString value ?profileURLtoCheck=warrenbuckley
            //Member findMember = Member.GetAllAsList().FirstOrDefault(x => x.getProperty("profileURL").Value.ToString() == profileURLtoCheck);
            var findMember = membershipService.GetById(Members.GetCurrentMemberId());

            //Create a view model
            RegisterViewModel profile = new RegisterViewModel();

            //Check if we found member
            if (findMember != null)
            {
                //Increment profile view counter by one
                //int noOfProfileViews = 0;
                //int.TryParse(findMember.Properties["numberOfProfileViews"].Value.ToString(), out noOfProfileViews);

                //Increment counter by one
                //findMember.Properties["numberOfProfileViews"].Value = noOfProfileViews + 1;

                //Save it down to the member
                //  membershipService.Save(findMember);

                //Got the member lets bind the data to the view model
                if (findMember.ContentTypeAlias == "agent")
                {
                    profile.Name = findMember.Name;
                    
                    profile.RegisterAgent.EmailAddress = findMember.Email;
                    profile.MemberType =  findMember.ContentTypeAlias; //Groups.Values.Cast<MemberGroup>().First().Text;
                    profile.RegisterAgent.FirstName = findMember.Properties["firstName"].Value.ToString();
                    profile.RegisterAgent.LastName = findMember.Properties["lastName"].Value.ToString();
                    profile.RegisterAgent.CellNumber = findMember.Properties["cellNumber"].Value.ToString();
               //     profile.RegisterAgent.Agency = findMember.Properties["agency"].Value ?? "";
                    profile.RegisterAgent.AgencyPin = findMember.Properties["agencyPin"].Value.ToString();
                    profile.RegisterAgent.Picture = findMember.Properties["picture"].Value.ToString();
                }
                if (findMember.ContentTypeAlias == "agency")
                {
                    profile.Name = findMember.Name;

                    profile.RegisterAgency.EmailAddress = findMember.Email;
                    profile.MemberType = findMember.ContentTypeAlias; //Groups.Values.Cast<MemberGroup>().First().Text;
                 //   profile.RegisterAgency.Name = findMember.Properties["agencyName"].Value.ToString();
                    profile.RegisterAgency.CellNumber = findMember.Properties["cellNumber"].Value.StringToNullSafe();
                    profile.RegisterAgency.Agents = findMember.Properties["agents"].Value.StringToNullSafe();
                    profile.RegisterAgency.AgencyPin = findMember.Properties["agencyPin"].Value.StringToNullSafe();
                    profile.RegisterAgency.Logo = findMember.Properties["logo"].Value.StringToNullSafe();
                }
                //  profile.Description = findMember.Properties["description"].Value.ToString();


            }
            else
            {
                //Couldn't find the member return a 404
                return new HttpNotFoundResult("The member profile does not exist");
            }

            return PartialView("ViewProfile", profile);
        }

        //REMOTE Validation
        public JsonResult CheckEmailIsUsed(string emailAddress)
        {
            var membershipService = ApplicationContext.Services.MemberService;

            //Get Current Member
            var member = membershipService.GetById(Members.GetCurrentMember().Id);

            //Sometimes inconsistent results with GetCurrent Member, unsure why?!
            if (member != null)
            {

                //if the email is the same as the one stored then it's OK
                if (member.Email == emailAddress)
                {
                    //Email is the same as one currently stored on the member - so email ok to use & rule valid (return true, back to validator)
                    return Json(true, JsonRequestBehavior.AllowGet);
                }

                //Try and get member by email typed in
                var checkEmailMember = membershipService.GetByEmail(emailAddress);

                if (checkEmailMember != null)
                {
                    return Json(String.Format("The email address '{0}' is already in use.", emailAddress),
                                JsonRequestBehavior.AllowGet);
                }

                return Json(true, JsonRequestBehavior.AllowGet);
            }

            //Unable to get current member to check (just an OK for client side validation)
            //and let action in controller validate
            return Json(true, JsonRequestBehavior.AllowGet);
        }


        public JsonResult CheckProfileURLAvailable(string profileURL)
        {
            var membershipService = ApplicationContext.Services.MemberService;

            //Get Current Member
            var member = Members.GetCurrentMember();

            //Sometimes inconsistent results with GetCurrent Member, unsure why?!
            if (member != null)
            {
                if (member.GetProperty("profileURL").Value.ToString() == profileURL)
                {
                    //profile URL is the same as one currently stored - so it's ok to use & rule valid (return true, back to validator)
                    return Json(true, JsonRequestBehavior.AllowGet);
                }

                //Get all members where profileURL property = value from Model
                //Member checkProfileURL =
                //    Member.GetAllAsList().FirstOrDefault(x => x.getProperty("profileURL").Value.ToString() == profileURL);
                var checkProfileURL = membershipService.GetMembersByPropertyValue("profileURL", profileURL, StringPropertyMatchType.Exact).SingleOrDefault();

                //Check not null if not null then its got one in the system already
                if (checkProfileURL != null)
                {
                    return Json(String.Format("The profile URL '{0}' is already in use.", profileURL),
                                JsonRequestBehavior.AllowGet);
                }


                // no profile has this url so its all good in the hood
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            //Unable to get current member to check (just an OK for client side validation)
            //and let action in controller validate
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}