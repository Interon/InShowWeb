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
        public IMember GetCurrentMember()
        {
            var memberService = ApplicationContext.Services.MemberService;
            var memberId = Members.GetCurrentMemberId();
            return memberService.GetById(memberId);
        }

    }
}