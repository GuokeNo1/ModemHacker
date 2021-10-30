using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

namespace LibModemBase
{
    public class EasyWeb
    {
        public class EasyResponse
        {
            //public string cookies;
            public string body;
        }

        public static EasyResponse Post(string url, string data = "", string cookies = "")
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "POST";
            if (cookies.Trim() != "")
                request.Headers["Cookie"] = cookies;
            if (data.Trim() != "")
            {
                using (StreamWriter sw = new StreamWriter(request.GetRequestStream()))
                {
                    sw.Write(data);
                }
            }
            EasyResponse responseResult = new EasyResponse();
            using (var response = request.GetResponse())
            {
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    responseResult.body = sr.ReadToEnd();
                }
            }
            return responseResult;
        }
        public static EasyResponse Get(string url, string cookies = "")
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            if (cookies.Trim() != "")
                request.Headers["Cookie"] = cookies;
            EasyResponse responseResult = new EasyResponse();
            using (var response = request.GetResponse())
            {
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    responseResult.body = sr.ReadToEnd();
                }
            }
            return responseResult;
        }
    }
}
