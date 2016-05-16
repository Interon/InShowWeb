using System;
using System.Collections.Generic;
using System.Web.Http;
using Umbraco.Core.Models;
using Umbraco.Web.WebApi;

namespace InshowControllers
{
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
        public bool UpdateMember (string id,dynamic o)
        {
            //TODO - Find a better way to handle o
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

    }
}