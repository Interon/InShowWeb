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

namespace InShow.GenereatedModels
{
	/// <summary>Buyer</summary>
	[PublishedContentModel("buyer")]
	public partial class Buyer : PublishedContentModel
	{
#pragma warning disable 0109 // new is redundant
		public new const string ModelTypeAlias = "buyer";
		public new const PublishedItemType ModelItemType = PublishedItemType.Member;
#pragma warning restore 0109

		public Buyer(IPublishedContent content)
			: base(content)
		{ }

#pragma warning disable 0109 // new is redundant
		public new static PublishedContentType GetModelContentType()
		{
			return PublishedContentType.Get(ModelItemType, ModelTypeAlias);
		}
#pragma warning restore 0109

		public static PublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<Buyer, TValue>> selector)
		{
			return PublishedContentModelUtility.GetModelPropertyType(GetModelContentType(), selector);
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
		/// Email
		///</summary>
		[ImplementPropertyType("email")]
		public string Email
		{
			get { return this.GetPropertyValue<string>("email"); }
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
		/// Has Verified Cell
		///</summary>
		[ImplementPropertyType("hasVerifiedCell")]
		public bool HasVerifiedCell
		{
			get { return this.GetPropertyValue<bool>("hasVerifiedCell"); }
		}

		///<summary>
		/// Has Verified Email
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
		/// iPofLastLogin
		///</summary>
		[ImplementPropertyType("iPofLastLogin")]
		public string IPofLastLogin
		{
			get { return this.GetPropertyValue<string>("iPofLastLogin"); }
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
		/// numberOfLogins
		///</summary>
		[ImplementPropertyType("numberOfLogins")]
		public int NumberOfLogins
		{
			get { return this.GetPropertyValue<int>("numberOfLogins"); }
		}

		///<summary>
		/// OTP
		///</summary>
		[ImplementPropertyType("oTP")]
		public int OTP
		{
			get { return this.GetPropertyValue<int>("oTP"); }
		}

		///<summary>
		/// Profile Url
		///</summary>
		[ImplementPropertyType("profileUrl")]
		public string ProfileUrl
		{
			get { return this.GetPropertyValue<string>("profileUrl"); }
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
