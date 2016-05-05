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
	/// <summary>Email template</summary>
	[PublishedContentModel("EmailTemplate")]
	public partial class EmailTemplate : Perplexmail
	{
#pragma warning disable 0109 // new is redundant
		public new const string ModelTypeAlias = "EmailTemplate";
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
#pragma warning restore 0109

		public EmailTemplate(IPublishedContent content)
			: base(content)
		{ }

#pragma warning disable 0109 // new is redundant
		public new static PublishedContentType GetModelContentType()
		{
			return PublishedContentType.Get(ModelItemType, ModelTypeAlias);
		}
#pragma warning restore 0109

		public static PublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<EmailTemplate, TValue>> selector)
		{
			return PublishedContentModelUtility.GetModelPropertyType(GetModelContentType(), selector);
		}

		///<summary>
		/// CSS
		///</summary>
		[ImplementPropertyType("css")]
		public string Css
		{
			get { return this.GetPropertyValue<string>("css"); }
		}

		///<summary>
		/// Template: Use the tag [#content#] to specify where the content of the e-mail should be rendered
		///</summary>
		[ImplementPropertyType("template")]
		public string Template
		{
			get { return this.GetPropertyValue<string>("template"); }
		}
	}
}
