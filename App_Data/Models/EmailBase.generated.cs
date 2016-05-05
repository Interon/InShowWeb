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
	/// <summary>Email base</summary>
	[PublishedContentModel("EmailBase")]
	public partial class EmailBase : Perplexmail
	{
#pragma warning disable 0109 // new is redundant
		public new const string ModelTypeAlias = "EmailBase";
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
#pragma warning restore 0109

		public EmailBase(IPublishedContent content)
			: base(content)
		{ }

#pragma warning disable 0109 // new is redundant
		public new static PublishedContentType GetModelContentType()
		{
			return PublishedContentType.Get(ModelItemType, ModelTypeAlias);
		}
#pragma warning restore 0109

		public static PublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<EmailBase, TValue>> selector)
		{
			return PublishedContentModelUtility.GetModelPropertyType(GetModelContentType(), selector);
		}

		///<summary>
		/// Attachments
		///</summary>
		[ImplementPropertyType("attachments")]
		public string Attachments
		{
			get { return this.GetPropertyValue<string>("attachments"); }
		}

		///<summary>
		/// BCC
		///</summary>
		[ImplementPropertyType("bcc")]
		public Newtonsoft.Json.Linq.JToken Bcc
		{
			get { return this.GetPropertyValue<Newtonsoft.Json.Linq.JToken>("bcc"); }
		}

		///<summary>
		/// Body
		///</summary>
		[ImplementPropertyType("body")]
		public IHtmlString Body
		{
			get { return this.GetPropertyValue<IHtmlString>("body"); }
		}

		///<summary>
		/// CC
		///</summary>
		[ImplementPropertyType("cc")]
		public Newtonsoft.Json.Linq.JToken Cc
		{
			get { return this.GetPropertyValue<Newtonsoft.Json.Linq.JToken>("cc"); }
		}

		///<summary>
		/// Disable automated logging
		///</summary>
		[ImplementPropertyType("disableAutomatedLogging")]
		public bool DisableAutomatedLogging
		{
			get { return this.GetPropertyValue<bool>("disableAutomatedLogging"); }
		}

		///<summary>
		/// Email template: Select the template that this e-mail will use for basic layout and content.    Take note that you can only select published templates
		///</summary>
		[ImplementPropertyType("emailTemplate")]
		public string EmailTemplate
		{
			get { return this.GetPropertyValue<string>("emailTemplate"); }
		}

		///<summary>
		/// From
		///</summary>
		[ImplementPropertyType("from")]
		public Newtonsoft.Json.Linq.JToken From
		{
			get { return this.GetPropertyValue<Newtonsoft.Json.Linq.JToken>("from"); }
		}

		///<summary>
		/// Log expiration: If this value is left blank, the global (parent's) value will be used instead
		///</summary>
		[ImplementPropertyType("logExpiration")]
		public object LogExpiration
		{
			get { return this.GetPropertyValue("logExpiration"); }
		}

		///<summary>
		/// Reply to
		///</summary>
		[ImplementPropertyType("replyTo")]
		public Newtonsoft.Json.Linq.JToken ReplyTo
		{
			get { return this.GetPropertyValue<Newtonsoft.Json.Linq.JToken>("replyTo"); }
		}

		///<summary>
		/// Send test e-mail: Enter the recipient's e-mailaddress.    Please take note that test emails will only be sent to the specified receiver here and not to BCC or CC receivers.
		///</summary>
		[ImplementPropertyType("sendTestEMail")]
		public Newtonsoft.Json.Linq.JToken SendTestEmail
		{
			get { return this.GetPropertyValue<Newtonsoft.Json.Linq.JToken>("sendTestEMail"); }
		}

		///<summary>
		/// Statistics
		///</summary>
		[ImplementPropertyType("statistics")]
		public Newtonsoft.Json.Linq.JToken Statistics
		{
			get { return this.GetPropertyValue<Newtonsoft.Json.Linq.JToken>("statistics"); }
		}

		///<summary>
		/// Subject
		///</summary>
		[ImplementPropertyType("subject")]
		public string Subject
		{
			get { return this.GetPropertyValue<string>("subject"); }
		}

		///<summary>
		/// Text version: (Optional) Provide an alternate text-only (non-HTML) version of your e-mail that is read by recipient devices that do not support complex HTML.
		///</summary>
		[ImplementPropertyType("textVersion")]
		public string TextVersion
		{
			get { return this.GetPropertyValue<string>("textVersion"); }
		}

		///<summary>
		/// To
		///</summary>
		[ImplementPropertyType("to")]
		public Newtonsoft.Json.Linq.JToken To
		{
			get { return this.GetPropertyValue<Newtonsoft.Json.Linq.JToken>("to"); }
		}
	}
}