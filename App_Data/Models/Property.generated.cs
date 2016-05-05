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
	/// <summary>Property</summary>
	[PublishedContentModel("property")]
	public partial class Property : Suburb
	{
#pragma warning disable 0109 // new is redundant
		public new const string ModelTypeAlias = "property";
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
#pragma warning restore 0109

		public Property(IPublishedContent content)
			: base(content)
		{ }

#pragma warning disable 0109 // new is redundant
		public new static PublishedContentType GetModelContentType()
		{
			return PublishedContentType.Get(ModelItemType, ModelTypeAlias);
		}
#pragma warning restore 0109

		public static PublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<Property, TValue>> selector)
		{
			return PublishedContentModelUtility.GetModelPropertyType(GetModelContentType(), selector);
		}

		///<summary>
		/// Bathrooms
		///</summary>
		[ImplementPropertyType("bathrooms")]
		public int Bathrooms
		{
			get { return this.GetPropertyValue<int>("bathrooms"); }
		}

		///<summary>
		/// Bedrooms
		///</summary>
		[ImplementPropertyType("bedrooms")]
		public int Bedrooms
		{
			get { return this.GetPropertyValue<int>("bedrooms"); }
		}

		///<summary>
		/// Description
		///</summary>
		[ImplementPropertyType("description")]
		public string Description
		{
			get { return this.GetPropertyValue<string>("description"); }
		}

		///<summary>
		/// Filters
		///</summary>
		[ImplementPropertyType("filters")]
		public object Filters
		{
			get { return this.GetPropertyValue("filters"); }
		}

		///<summary>
		/// Garages
		///</summary>
		[ImplementPropertyType("garages")]
		public int Garages
		{
			get { return this.GetPropertyValue<int>("garages"); }
		}

		///<summary>
		/// Images
		///</summary>
		[ImplementPropertyType("images")]
		public string Images
		{
			get { return this.GetPropertyValue<string>("images"); }
		}

		///<summary>
		/// P24Url
		///</summary>
		[ImplementPropertyType("p24Url")]
		public string P24Url
		{
			get { return this.GetPropertyValue<string>("p24Url"); }
		}

		///<summary>
		/// Selling Price
		///</summary>
		[ImplementPropertyType("sellingPrice")]
		public int SellingPrice
		{
			get { return this.GetPropertyValue<int>("sellingPrice"); }
		}

		///<summary>
		/// Shedule
		///</summary>
		[ImplementPropertyType("shedule")]
		public object Shedule
		{
			get { return this.GetPropertyValue("shedule"); }
		}

		///<summary>
		/// Title
		///</summary>
		[ImplementPropertyType("title")]
		public string Title
		{
			get { return this.GetPropertyValue<string>("title"); }
		}
	}
}
