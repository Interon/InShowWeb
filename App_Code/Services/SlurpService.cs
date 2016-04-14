using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

/// <summary>
/// Summary description for SlurpService
/// </summary>
public class SlurpService
{
    public SlurpService(string url)
    {
        GetAllImages(url);
    }
    private List<string> imageList2 = new List<string>();
    private List<string> imageList = new List<string>();
    public List<string> DivList = new List<string>();
    String innerHtml = "";

    public List<string> ImageList
    {
        get
        {
            return imageList.Distinct().ToList();
        }

        set
        {
            imageList = value;
        }
    }

    public string InnerHtml
    {
        get
        {
            return innerHtml;
        }

        set
        {
            innerHtml = value;
        }
    }

    public void GetAllImages(string url)
    {
        // Declaring 'x' as a new WebClient() method
        WebClient x = new WebClient();
        // Setting the URL, then downloading the data from the URL.
        string source = x.DownloadString(url);
        // Declaring 'document' as new HtmlAgilityPack() method
        HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
        
        // Loading document's source via HtmlAgilityPack
        document.LoadHtml(source);
        // For every tag in the HTML containing the node img.
        foreach (var link in document.DocumentNode
                             .Descendants("img")
                             .Select(s => s.Attributes["src"]))
        {
            // Storing all links found in an array. You can declare this however you want.]
            try
            {
                var divlist = link.OwnerNode.Ancestors();
                foreach(var d in divlist)
                {
                    if(d.Id == "MainGalleryImageList")
                    {
                        if (link.Value.Contains("FetchImage.ashx"))
                        {
                            var myarray = link.Value.Split('&');

                            imageList.Add(myarray[0]);
                        }
                    }
                }
              
            }
            catch
            { }


        }
        foreach (var link in document.DocumentNode
                             .Descendants("img")
                             .Select(s => s.Attributes["lazy-src"]))
        {
            // Storing all links found in an array. You can declare this however you want.]
            try
            {
                if (link.Value.Contains("FetchImage.ashx"))
                {
                    var myarray = link.Value.Split('&');

                    imageList.Add(myarray[0]);
                }
            }
            catch
            { }


        }
       
        foreach (var content in document.DocumentNode
                             .Descendants("div")
                             .Select(s => s.Attributes["class"]))
        {
            // Storing all links found in an array. You can declare this however you want.]
            try
            {
                if (content.Value == "p24_details")
                {
                    InnerHtml = content.OwnerNode.InnerHtml;
                }

            }
            catch
            { }


        }
    }



}