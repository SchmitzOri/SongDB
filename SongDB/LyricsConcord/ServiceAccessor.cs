using CommonDTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;

namespace LyricsConcord
{
    public static class ServiceAccessor
    {
        public static S MakeRequest<T, S>(object request, string Function)
        {
            try
            {
                HttpWebRequest req = WebRequest.Create(new Uri(ConfigurationManager.AppSettings["ServiceURL"] + Function)) as HttpWebRequest;
                req.Method = "POST";
                req.ContentType = "application/json";
                req.Accept = "application/json";

                MemoryStream stream1 = new MemoryStream();
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));

                ser.WriteObject(stream1, request);

                stream1.Position = 0;
                StreamReader sr = new StreamReader(stream1);

                byte[] requestData = UTF8Encoding.UTF8.GetBytes(sr.ReadToEnd());
                req.ContentLength = requestData.Length;

                // Send the request:
                using (Stream post = req.GetRequestStream())
                {
                    post.Write(requestData, 0, requestData.Length);
                }

                // Pick up and deserialize the response:
                DataContractJsonSerializer responseSerializer = new DataContractJsonSerializer(typeof(S));
                S response = default(S);

                using (HttpWebResponse resp = req.GetResponse() as HttpWebResponse)
                {
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        response = (S)responseSerializer.ReadObject(resp.GetResponseStream());
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                return default(S);
            }
        }

        public static Guid UploadSong(byte[] fileContent)
        {
            try
            {
                using (WebClient uploader = new WebClient())
                {
                    uploader.UploadData(new Uri(ConfigurationManager.AppSettings["ServiceURL"] + "UploadSong"), fileContent);

                    return Guid.Empty;
                }
            }
            catch (Exception ex)
            {
                return Guid.Empty;
            }
        }

        public static Guid UploadMultipleSongs(byte[] fileContent)
        {
            try
            {
                using (WebClient uploader = new WebClient())
                {
                    uploader.UploadData(new Uri(ConfigurationManager.AppSettings["ServiceURL"] + "UploadMultipleSongs"), fileContent);

                    return Guid.Empty;
                }
            }
            catch (Exception ex)
            {
                return Guid.Empty;
            }
        }

        public static bool UploadBackup(byte[] fileContent)
        {
            try
            {
                using (WebClient uploader = new WebClient())
                {
                    uploader.UploadData(new Uri(ConfigurationManager.AppSettings["ServiceURL"] + "ImportXML"), fileContent);

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}