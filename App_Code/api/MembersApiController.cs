using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Umbraco.Core.Models;
using Umbraco.Web.WebApi;


namespace InshowControllers
{



    //MVC Models

    public class passwordModel
    {

        public string password;
    }

    public class addAgentModel
    {
        public int Id;
        public string UserName;
        public string FirstName;
        public string LastName;
        public string CellNumber;
        public string Agency;
        public string AgencyPin;
        public string EmailAddress;
        public string Password;

    }


    //MVC Controllers

    public class MembersController : UmbracoApiController
    {
        [HttpGet]
        public IEnumerable<IMember> GetAllmembers()
        {
            var memberService = ApplicationContext.Services.MemberService;
            return memberService.GetAllMembers();
        }

        [HttpGet]
        public IMember GetCurrentMember()
        {
            var memberService = ApplicationContext.Services.MemberService;
            var memberId = Members.GetCurrentMemberId();
            return memberService.GetById(memberId);
        }

        [HttpPost]
        public bool UpdateMember(string id, dynamic o)
        {
            //TODO AO Find a better way to handle o
            try
            {
                var memberService = ApplicationContext.Services.MemberService;
                var mymember = memberService.GetById(int.Parse(id));
                foreach (var oo in o)
                {
                    mymember.Properties[oo.Name].Value = oo.Value.ToString();
                }
                memberService.Save(mymember, true);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        [HttpPost]
        public bool ChangePassword(string id, object o)
        {

            var json = JsonConvert.DeserializeObject<passwordModel>(o.ToString());
            try
            {
                var memberService = ApplicationContext.Services.MemberService;
                var mymember = memberService.GetById(int.Parse(id));

                memberService.SavePassword(mymember, json.password);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }


        [HttpPost]
        public bool AddAgent(string id, object o)
        {

            var json = JsonConvert.DeserializeObject<addAgentModel>(o.ToString());
            try
            {
                var memberService = ApplicationContext.Services.MemberService;
                var mymember = memberService.GetById(int.Parse(id));
                int AgencyId = mymember.Id;
                //memberService.SavePassword(mymember, json.Password);
                //return true;



                if (!ModelState.IsValid)
                {
                    //return page
                    return false;
                }

                //Model valid let's create the member
                try
                {



                        //We found the member with that email

                        //member loop
                        var members = memberService.GetAllMembers();

                        foreach (Member m in members)
                        {

                            if (m.Email == json.EmailAddress)
                            {

                                if (m.ContentTypeAlias == "agent")
                                {
                                    return false;
                                }

                            }

                        }

                   
                    var createMember = memberService.CreateMember(json.UserName, json.EmailAddress, json.FirstName + " " + json.LastName, "agent");

                    //Set the verified email to false
                    createMember.Properties["hasVerifiedEmail"].Value = false;

                    //Save the changes, if we do not do so, we cannot save the password.
                    memberService.Save(createMember);

                    //Set password on the newly created member
                    memberService.SavePassword(createMember, json.Password);

                    //Send verification Email

                    //Create temporary GUID
                    var tempGUID = Guid.NewGuid();

                    //Fetch our new member we created by their email
                    var updateMember = memberService.GetById(createMember.Id);

                    //Just to be sure...
                    if (updateMember != null)
                    {
                        //Set the verification email GUID value on the member
                        updateMember.Properties["emailVerifyGUID"].Value = tempGUID.ToString();

                        //Set the Joined Date label on the member
                        updateMember.Properties["joinedDate"].Value = DateTime.Now.ToString("dd/MM/yyyy @ HH:mm:ss");

                        updateMember.Properties["firstName"].Value = json.FirstName;

                        updateMember.Properties["lastName"].Value = json.LastName;

                        updateMember.Properties["cellNumber"].Value = json.CellNumber;

                        updateMember.Properties["agencyPin"].Value = json.AgencyPin;

                        updateMember.Properties["agency"].Value = AgencyId;

                        updateMember.Properties["email"].Value = json.EmailAddress;

                        updateMember.Properties["numberOfLogins"].Value = 0;


                        //Save changes
                        memberService.Save(updateMember);
                    }


                    String AgentName = json.FirstName + " " + json.LastName;
                    //Verify link
                    string baseURL = System.Web.HttpContext.Current.Request.Url.AbsoluteUri.Replace(System.Web.HttpContext.Current.Request.Url.AbsolutePath, string.Empty);
                    var verifyURL = baseURL + "/verify-email?verifyGUID=" + tempGUID.ToString();

                    List<PerplexMail.EmailTag> listOfTags = new List<PerplexMail.EmailTag>();
                    listOfTags.Add(new PerplexMail.EmailTag("[#email#]", json.EmailAddress));
                    listOfTags.Add(new PerplexMail.EmailTag("[#name#]", AgentName));
                    listOfTags.Add(new PerplexMail.EmailTag("[#GUID#]", verifyURL));
                    listOfTags.Add(new PerplexMail.EmailTag("[#buysell#]", "selling"));

                    //                    //create email id ???
                    var contentService = ApplicationContext.Services.ContentService;
                    var Emails = contentService.GetRootContent().Where(x => x.Name.ToString() == "PerplexMail (1)").First().Descendants().Where(x => x.Name == "Register Member Email (2)").First();
                    int emailNodeToSend = Emails.Id; //umbraco Email node ID.

                    //int emailNodeToSend = 1903;

                    PerplexMail.Email.SendUmbracoEmail(emailNodeToSend, listOfTags);



                }
                catch (Exception ex)
                {
                    //EG: Duplicate email address - already exists
                    ModelState.AddModelError("memberCreation", ex.Message);

                    return true;
                }

                //Send out verification email, with GUID in it
                //EmailHelper email = new EmailHelper();
                //email.SendVerifyEmail(model.RegisterAgent.EmailAddress, tempGUID.ToString());

                //Update success flag (in a TempData key)
                //TempData["IsSuccessful"] = true;

                //TempData.Add("CustomMessage", "Your form was successfully submitted at " + DateTime.Now);

                //All done - redirect back to page
                //return CurrentUmbracoPage();

                return true;

            }
            catch (Exception e)
            {
                return false;
            }


        }


    }
}