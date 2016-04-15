﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using InShow.Models;

namespace InShow.Controllers {

    public class RegisterUserSurfaceController : SurfaceController
    {

        public ActionResult ShowRegistrationForm()
        {

            var model = new RegisterUserModel();
            return PartialView("RegisterUserFormPartial", model);

        }

        public ActionResult HandleRegistrationPost(RegisterUserModel model)
        {

            var newUser = Services.ContentService.CreateContent(model.Id + " " + model.FirstName + " " + model.LastName + " - " + DateTime.Now.ToString("dd-MM-yyyy HH:mm"), CurrentPage.Id, "registerUserFormula");
            newUser.SetValue("firstName", model.FirstName);
            newUser.SetValue("lastName", model.LastName);
            newUser.SetValue("email", model.Email);
            newUser.SetValue("password", model.Password);
            Services.ContentService.SaveAndPublishWithStatus(newUser);
            return RedirectToCurrentUmbracoPage();

        }

    }
}