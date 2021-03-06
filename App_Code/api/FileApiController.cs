﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Umbraco.Core.Models;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

/// <summary>
/// Summary description for FileSurfaceController
/// </summary>
/// 
namespace Inshow.Controllers
{
    public class UploadDataModel
    {
        public string Type { get; set; }
        public string NodeId { get; set; }
        public string PropertyAlias { get; set; }
        public string DataType { get; set; }
    }

    public class FileApiController : UmbracoApiController
    {
        [HttpPost] // This is from System.Web.Http, and not from System.Web.Mvc
            public async Task<HttpResponseMessage> Upload()
            {
                if (!Request.Content.IsMimeMultipartContent())
                {
                    this.Request.CreateResponse(HttpStatusCode.UnsupportedMediaType);
                }

                var provider = GetMultipartProvider();
                var result = await Request.Content.ReadAsMultipartAsync(provider);

                // On upload, files are given a generic name like "BodyPart_26d6abe1-3ae1-416a-9429-b35f15e6e5d5"
                // so this is how you can get the original file name
                var originalFileName = GetDeserializedFileName(result.FileData.First());

                // uploadedFileInfo object will give you some additional stuff like file length,
                // creation time, directory name, a few filesystem methods etc..
                var uploadedFileInfo = new FileInfo(result.FileData.First().LocalFileName);

                // Remove this line as well as GetFormData method if you're not
                // sending any form data with your upload request
                dynamic fileUploadObj = GetFormData<UploadDataModel>(result);

            var ms = Services.MediaService;
            MemoryStream uploadFile = new MemoryStream();
            
            using (FileStream fs = uploadedFileInfo.OpenRead())
            {
              fs.CopyTo(uploadFile);
                var parent = ms.GetById(1781);
                var media = ms.CreateMedia(originalFileName, parent, "Image");
                media.SetValue("umbracoFile",originalFileName, fs);
                //TODO AO Save method hangs
                ms.Save(media,0,true);
                var memberservice = Services.MemberService;

                IMember member = memberservice.GetById(fileUploadObj.NodeId);

                member.SetValue(fileUploadObj.PropertyAlias, media.Path);
                memberservice.Save(member);


            }



















            var returnData = "ReturnTest";
                return this.Request.CreateResponse(HttpStatusCode.OK, new { returnData });
            }

            // You could extract these two private methods to a separate utility class since
            // they do not really belong to a controller class but that is up to you
            private MultipartFormDataStreamProvider GetMultipartProvider()
            {
                // IMPORTANT: replace "(tilde)" with the real tilde character
                // (our editor doesn't allow it, so I just wrote "(tilde)" instead)
                var uploadFolder = "(tilde)/App_Data/Tmp/FileUploads"; // you could put this to web.config
                var root = HttpContext.Current.Server.MapPath(uploadFolder);
                Directory.CreateDirectory(root);
                return new MultipartFormDataStreamProvider(root);
            }

            // Extracts Request FormatData as a strongly typed model
            private object GetFormData<T>(MultipartFormDataStreamProvider result)
            {
                if (result.FormData.HasKeys())
                {
                NameValueCollection nvc = result.FormData;
                UploadDataModel mySettingsClass = new UploadDataModel();
                foreach (string kvp in nvc.AllKeys)
                {
                    PropertyInfo pi = mySettingsClass.GetType().GetProperty(kvp, BindingFlags.Public | BindingFlags.Instance);
                    if (pi != null)
                    {
                        pi.SetValue(mySettingsClass, nvc[kvp], null);
                    }
                }
                return mySettingsClass;
            }

                return null;
            }

            private string GetDeserializedFileName(MultipartFileData fileData)
            {
                var fileName = GetFileName(fileData);
                return JsonConvert.DeserializeObject(fileName).ToString();
            }

            public string GetFileName(MultipartFileData fileData)
            {
                return fileData.Headers.ContentDisposition.FileName;
            }
        }
    }
