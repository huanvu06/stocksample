using NLog;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CrossOverStock.Data.Helper
{

    public sealed class HttpHelper
    {
        private static readonly ILogger log = LogManager.GetLogger("HttpHelper");

        public static byte[] StreamToByteArray(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
        public static byte[] ReadAllBytes(string fileName)
        {
            byte[] buffer = null;
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                buffer = new byte[fs.Length];
                fs.Read(buffer, 0, (int)fs.Length);
            }
            return buffer;
        }
        public static string DictToString(Dictionary<string, string> dict)
        {
            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<string, string> kvp in dict)
                builder.Append(kvp.Key + "=" + kvp.Value + "&");

            if (builder.Length > 0) builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }

        public static string DictToString(NameValueCollection collection)
        {
            StringBuilder builder = new StringBuilder();
            foreach (string key in collection.AllKeys)
                builder.Append(key + "=" + collection[key] + "&");

            if (builder.Length > 0) builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }

        public static string UrlParamCombine(string url, string paramsString)
        {
            if (url.Contains("?"))
                url = string.Concat(url, "&", paramsString);
            else
                url = string.Concat(url, "?", paramsString);
            return url;
        }

        public static string UrlParamCombine(string url, Dictionary<string, string> urlparams)
        {
            return UrlParamCombine(url, DictToString(urlparams));
        }

        public static string UrlPathCombine(string left, params object[] right)
        {
            if (right.Length == 0 || string.IsNullOrWhiteSpace(left) || string.IsNullOrWhiteSpace(right[0].ToString()))
                return left;

            string curRight = right[0].ToString();

            // Add "/" to last char of "left"
            if (left.LastIndexOf("/") != left.Length - 1)
                left += "/";

            // Remove "/" of first char of right
            if (curRight.IndexOf("/") == 0)
                curRight = curRight.Substring(1);

            return UrlPathCombine(string.Concat(left, curRight), right.Skip(1).ToArray());
        }

        #region Logs
        public static void WriteLogError(Exception ex)
        {
            string message = CursiveExceptionMessage(ex);
            WriteLogError(message);
        }
        public static void WriteLogError(string message)
        {
            log.Error(message);
        }

        public static string CursiveExceptionMessage(Exception ex)
        {
            string message = ex.Message + " --------------------- " + ex.StackTrace;
            while (ex.InnerException != null)
            {
                message += "\r\n\n" + ex.InnerException.Message + " --------------------- " + ex.InnerException.StackTrace;
                ex = ex.InnerException;
            }
            return message;
        }

        public static void WriteLogMessage(Exception ex)
        {
            string message = ex.Message + " --------------------- " + ex.StackTrace;
            while (ex.InnerException != null)
            {
                message += "\r\n\n" + ex.InnerException.Message + " --------------------- " + ex.InnerException.StackTrace;
                ex = ex.InnerException;
            }
            WriteLogMessage(message);
        }
        public static void WriteLogMessage(string message)
        {
            log.Info(message);
        }

        #endregion


        public static string CatchWebRequestException(WebException wexc)
        {
            string msg = wexc.Message ?? "Response is null!.";
            if (wexc != null && wexc.Response != null)
                using (WebResponse response = wexc.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    log.Error(String.Format("Error code: {0}", httpResponse.StatusCode));
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        msg = reader.ReadToEnd();
                    }
                }
            else if (wexc.InnerException != null)
                msg = wexc.InnerException.Message;

            return msg;
        }

        //private static string ConnectionStringBuilder(string DalPathName)
        //{

        //    var ecsbuilder = new System.Data.Entity.Core.EntityClient.EntityConnectionStringBuilder();
        //    ecsbuilder.Provider = "System.Data.SqlClient";
        //    ecsbuilder.ProviderConnectionString = ConfigurationManager.AppSettings[""];

        //    // Becare full the folder structure: General.Data or Data only in project folder tree.
        //    ecsbuilder.Metadata = @"res://*/" + DalPathName + ".csdl|res://*/" + DalPathName + ".ssdl|res://*/" + DalPathName + ".msl";
        //    return ecsbuilder.ToString();
        //}        
    }
}
