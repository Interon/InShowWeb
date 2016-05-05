//------------------------------------------------------------------------------
// <auto-generated>
//   This code was generated by a tool.
//
//    Umbraco.ModelsBuilder v3.0.2.93
//
//   Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Umbraco.ModelsBuilder;
using Umbraco.ModelsBuilder.Umbraco;

namespace InShow
{
	/// <summary>Agent</summary>
	[PublishedContentModel("agent")]
	public partial class Agent : PublishedContentModel
	{
#pragma warning disable 0109 // new is redundant
		public new const string ModelTypeAlias = "agent";
		public new const PublishedItemType ModelItemType = PublishedItemType.Member;
#pragma warning restore 0109

		public Agent(IPublishedContent content)
			: base(content)
		{ }

#pragma warning disable 0109 // new is redundant
		public new static PublishedContentType GetModelContentType()
		{
			return PublishedContentType.Get(ModelItemType, ModelTypeAlias);
		}
#pragma warning restore 0109

		public static PublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<Agent, TValue>> selector)
		{
			return PublishedContentModelUtility.GetModelPropertyType(GetModelContentType(), selector);
		}

		///<summary>
		/// Agency
		///</summary>
		[ImplementPropertyType("agency")]
		public object Agency
		{
			get { return this.GetPropertyValue("agency"); }
		}

		///<summary>
		/// Agency Name
		///</summary>
		[ImplementPropertyType("agencyName")]
		public string AgencyName
		{
			get { return this.GetPropertyValue<string>("agencyName"); }
		}

		///<summary>
		/// Agency Pin
		///</summary>
		[ImplementPropertyType("agencyPin")]
		public string AgencyPin
		{
			get { return this.GetPropertyValue<string>("agencyPin"); }
		}

		///<summary>
		/// Cell Number
		///</summary>
		[ImplementPropertyType("cellNumber")]
		public string CellNumber
		{
			get { return this.GetPropertyValue<string>("cellNumber"); }
		}

		///<summary>
		/// EmailVerifyGUID
		///</summary>
		[ImplementPropertyType("emailVerifyGUID")]
		public string EmailVerifyGuid
		{
			get { return this.GetPropertyValue<string>("emailVerifyGUID"); }
		}

		///<summary>
		/// First Name
		///</summary>
		[ImplementPropertyType("firstName")]
		public string FirstName
		{
			get { return this.GetPropertyValue<string>("firstName"); }
		}

		///<summary>
		/// hasVerifiedEmail
		///</summary>
		[ImplementPropertyType("hasVerifiedEmail")]
		public bool HasVerifiedEmail
		{
			get { return this.GetPropertyValue<bool>("hasVerifiedEmail"); }
		}

		///<summary>
		/// hostNameOfLastLogin
		///</summary>
		[ImplementPropertyType("hostNameOfLastLogin")]
		public string HostNameOfLastLogin
		{
			get { return this.GetPropertyValue<string>("hostNameOfLastLogin"); }
		}

		///<summary>
		/// InShow
		///</summary>
		[ImplementPropertyType("inShow")]
		public string InShow
		{
			get { return this.GetPropertyValue<string>("inShow"); }
		}

		///<summary>
		/// iPofLastLogin
		///</summary>
		[ImplementPropertyType("iPofLastLogin")]
		public string IPofLastLogin
		{
			get { return this.GetPropertyValue<string>("iPofLastLogin"); }
		}

		///<summary>
		/// Is Admin
		///</summary>
		[ImplementPropertyType("isAdmin")]
		public bool IsAdmin
		{
			get { return this.GetPropertyValue<bool>("isAdmin"); }
		}

		///<summary>
		/// JoinedDate
		///</summary>
		[ImplementPropertyType("joinedDate")]
		public string JoinedDate
		{
			get { return this.GetPropertyValue<string>("joinedDate"); }
		}

		///<summary>
		/// Last Name
		///</summary>
		[ImplementPropertyType("lastName")]
		public string LastName
		{
			get { return this.GetPropertyValue<string>("lastName"); }
		}

		///<summary>
		/// Listings
		///</summary>
		[ImplementPropertyType("listings")]
		public string Listings
		{
			get { return this.GetPropertyValue<string>("listings"); }
		}

		///<summary>
		/// numberOfLogins
		///</summary>
		[ImplementPropertyType("numberOfLogins")]
		public int NumberOfLogins
		{
			get { return this.GetPropertyValue<int>("numberOfLogins"); }
		}

		///<summary>
		/// Picture
		///</summary>
		[ImplementPropertyType("picture")]
		public object Picture
		{
			get { return this.GetPropertyValue("picture"); }
		}

		///<summary>
		/// Is Approved
		///</summary>
		[ImplementPropertyType("umbracoMemberApproved")]
		public bool UmbracoMemberApproved
		{
			get { return this.GetPropertyValue<bool>("umbracoMemberApproved"); }
		}

		///<summary>
		/// Comments
		///</summary>
		[ImplementPropertyType("umbracoMemberComments")]
		public string UmbracoMemberComments
		{
			get { return this.GetPropertyValue<string>("umbracoMemberComments"); }
		}

		///<summary>
		/// Failed Password Attempts
		///</summary>
		[ImplementPropertyType("umbracoMemberFailedPasswordAttempts")]
		public string UmbracoMemberFailedPasswordAttempts
		{
			get { return this.GetPropertyValue<string>("umbracoMemberFailedPasswordAttempts"); }
		}

		///<summary>
		/// Last Lockout Date
		///</summary>
		[ImplementPropertyType("umbracoMemberLastLockoutDate")]
		public string UmbracoMemberLastLockoutDate
		{
			get { return this.GetPropertyValue<string>("umbracoMemberLastLockoutDate"); }
		}

		///<summary>
		/// Last Login Date
		///</summary>
		[ImplementPropertyType("umbracoMemberLastLogin")]
		public string UmbracoMemberLastLogin
		{
			get { return this.GetPropertyValue<string>("umbracoMemberLastLogin"); }
		}

		///<summary>
		/// Last Password Change Date
		///</summary>
		[ImplementPropertyType("umbracoMemberLastPasswordChangeDate")]
		public string UmbracoMemberLastPasswordChangeDate
		{
			get { return this.GetPropertyValue<string>("umbracoMemberLastPasswordChangeDate"); }
		}

		///<summary>
		/// Is Locked Out
		///</summary>
		[ImplementPropertyType("umbracoMemberLockedOut")]
		public bool UmbracoMemberLockedOut
		{
			get { return this.GetPropertyValue<bool>("umbracoMemberLockedOut"); }
		}

		///<summary>
		/// Password Answer
		///</summary>
		[ImplementPropertyType("umbracoMemberPasswordRetrievalAnswer")]
		public string UmbracoMemberPasswordRetrievalAnswer
		{
			get { return this.GetPropertyValue<string>("umbracoMemberPasswordRetrievalAnswer"); }
		}

		///<summary>
		/// Password Question
		///</summary>
		[ImplementPropertyType("umbracoMemberPasswordRetrievalQuestion")]
		public string UmbracoMemberPasswordRetrievalQuestion
		{
			get { return this.GetPropertyValue<string>("umbracoMemberPasswordRetrievalQuestion"); }
		}
	}
}