using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace NAGGLE.Common.Utility
{
    public  class CommonFunction
    {
        private CommonFunction()
        {
        }

        public static string DatetimeFormat(string dateFormat)
        {
            //string[] splitDate = dateFormat.Split(' ');
            string year = dateFormat.Substring(0, 4); //string.Empty;
            string month = dateFormat.Substring(4, 2);//string.Empty;
            string day = dateFormat.Substring(6, 2);//string .Empty;
            //for (int i = 0; i <= splitDate.Length; i++)
            //{
            //    if (i <= 4)
            //        year += splitDate[i];
            //    else if (i > 4 && i <= 6)
            //        month += splitDate[i];
            //    else
            //        day += splitDate[i];
            //}
            string date = month + "/" + day + "/" + year;
            return date;
        }
        public static string GetWebSiteUrl()
        {
            Uri _url = HttpContext.Current.Request.Url;
            return _url.Scheme + "://" + _url.Authority;
        }
        public static string GetConfirmAccountUrl(string userid, string code)
        {
            string relativePath = System.Web.VirtualPathUtility.ToAbsolute("~/");
            return GetWebSiteUrl() + relativePath + "Home/Index?userid=" + userid + "&code=" + code;
        }

        public static string ConfigureConfirmAccountMailBodyDb(string url)
        {
            string body = string.Empty;

            body += "<html><body><p> Your Account has been successfully registered on Naggle.<p> </br> Please confirm your account "
                  + "and set your password by <a href=" + url + "> clicking here</a></body> </html>";
            return body;
        }

    }

    


    public static class Helper
    {
        private static string[] formats = new string[]
    {
        "MM/dd/yyyy",
        "MM-dd-yyyy",
        "yyyy-MM-dd",
        "yyyy-MM-dd H:mm:ss tt",
         "yyyy-MM-dd HH:mm:ss tt",
          "yyyy-MM-dd HH:mm:ss",
        "yyyy-MM-dd hh:mm:ss tt",
        "yyyy-MM-dd h:mm:ss tt",
        "yyyy-m-dd h:mm:ss tt",
          "yyyy-mm-d h:mm:ss tt",
           "yyyy-m-d h:mm:ss tt",
        "MM/dd/yyyy H:mm:ss tt",
         //"mm-dd-yyyy",
         // "MM-dd-yyyy",
         "MM/dd/yyyy hh:mm:ss tt",
         "M/d/yyyy h:mm:ss tt",
          "M/dd/yyyy h:mm:ss tt",
          "M-dd-yyyy h:mm:ss tt",
          "M-d-yyyy h:mm:ss tt",
           "MM-dd-yyyy hh:mm:ss tt",
            //"dd-MM-yyyy hh:mm:ss tt",
            // "dd-MM-yyyy h:mm:ss tt",
            // "d-MM-yyyy hh:mm:ss tt",
            //  "dd-M-yyyy h:mm:ss tt",
            //  "dd-mm-yyyy"
    };

        public static string ToYear(this DateTime date)
        {
            return date.ToString("yyyy");
        }

        public static string ToMonthDay(this DateTime date)
        {
            return date.ToString("MMMM dd");
        }


        //It's using on Order listing page
        public static string ToMonthYear(this DateTime date)
        {
            return date.ToString("MMM yyyy");
        }


        public static string ToYear(this string date)
        {
            return DateTime.ParseExact(date, formats, CultureInfo.InvariantCulture, DateTimeStyles.None).ToString("yyyy");
        }

        public static string ToMonthDay(this string date)
        {
            return DateTime.ParseExact(date, formats, CultureInfo.InvariantCulture, DateTimeStyles.None).ToString("MMMM dd");
        }


        //It's using on Order listing page
        public static string ToMonthYear(this string date)
        {
            return DateTime.ParseExact(date, formats, CultureInfo.InvariantCulture, DateTimeStyles.None).ToString("MMM yyyy");
        }


        public static string ToMMDDYYYY(this string date)
        {
            return DateTime.ParseExact(date, formats, CultureInfo.InvariantCulture, DateTimeStyles.None).ToString("MM/dd/yyyy");
        }

        public static string ToYYYYMMDD(this string date)
        {
            return DateTime.ParseExact(date, formats, CultureInfo.InvariantCulture, DateTimeStyles.None).ToString("yyyy-MM-dd");
        }

        public static string ToUPSTime(this TimeSpan time)
        {
            return new DateTime(time.Ticks).ToString("HHmm");
        }
        public static string ToUPSDate(this DateTime date)
        {
            return date.ToString("yyyyMMdd");
        }
        public static TimeSpan ToTimeSpan(this string timeString)
        {
            var dt = DateTime.ParseExact(timeString, "h:mm tt", System.Globalization.CultureInfo.InvariantCulture);
            return dt.TimeOfDay;
        }


        public static DateTime ToDateTime(this TimeSpan timeSpan)
        {
            return DateTime.Parse(timeSpan.ToString());
        }

        public static DateTime ToDateTime(this string date)
        {
            return DateTime.ParseExact(date, formats, CultureInfo.InvariantCulture, DateTimeStyles.None);
        }
        public static string Encrypt(this Int32 toEncrypt)
        {
            if (toEncrypt == 0)
                return "0";

            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt.ToString());

            System.Configuration.AppSettingsReader settingsReader =
                                                new AppSettingsReader();
            // Get the key from config file

            string key = "App";
            // (string)settingsReader.GetValue("SecurityKey",
            //                                 typeof(String));
            //System.Windows.Forms.MessageBox.Show(key);
            ////If hashing use get hashcode regards to your key
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            //Always release the resources and flush data
            // of the Cryptographic service provide. Best Practice

            //hashmd5.Clear();
            //else
            //keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)

            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray =
              cTransform.TransformFinalBlock(toEncryptArray, 0,
              toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format

            return Convert.ToBase64String(resultArray, 0, resultArray.Length).Replace("/", "^").Replace("+", "~");

        }

        public static int Decrypt(this string cipherString)
        {
            int checkInt = 0;
            if (string.IsNullOrEmpty(cipherString) || (int.TryParse(cipherString, out checkInt) && checkInt == 0))
                return 0;

            byte[] keyArray;
            //get the byte code of the string

            byte[] toEncryptArray = Convert.FromBase64String(cipherString.Replace("^", "/").Replace("~", "+"));

            System.Configuration.AppSettingsReader settingsReader =
                                                new AppSettingsReader();
            //Get your key from config file to open the lock!
            string key = "App";
            //(string)settingsReader.GetValue("SecurityKey",
            //                                       typeof(String));

            //if hashing was used get the hash code with regards to your key
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            ////release any resource held by the MD5CryptoServiceProvider

            //hashmd5.Clear();

            //else
            //{
            //if hashing was not implemented get the byte code of the key
            // keyArray = UTF8Encoding.UTF8.GetBytes(key);
            //}

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes. 
            //We choose ECB(Electronic code Book)

            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(
                                 toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor                
            tdes.Clear();
            //return the Clear decrypted TEXT
            string result = UTF8Encoding.UTF8.GetString(resultArray);

            int iresult = 0;
            if (int.TryParse(result, out iresult))
                return iresult;

            return -1;
        }



        //public static string PDFBase64String(this string base64)
        //{
        //    var bytes = Convert.FromBase64String(base64);
        //    var uid = "/Content/"+Guid.NewGuid().ToString() + ".pdf";
        //    File.WriteAllBytes(HttpContext.Current.Server.MapPath(uid), bytes);
        //    base64= Convert.ToBase64String(File.ReadAllBytes(HttpContext.Current.Server.MapPath(uid)));
        //    File.Delete(HttpContext.Current.Server.MapPath(uid));
        //    return base64;
        //}
    }




}
