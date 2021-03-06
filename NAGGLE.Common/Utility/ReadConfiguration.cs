﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace NAGGLE.Common.Utility
{
    public class ReadConfiguration
    {
        public static string WebsiteUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["WebsiteUrl"];
            }
        }
        public static TimeSpan EmailTokenExpirationTime
        {
            get
            {
                return TimeSpan.FromMinutes(Convert.ToDouble(ConfigurationManager.AppSettings["EmailTokenExpirationMinute"]));
            }
        }

        public static string MailTemplateFolder { get { return ConfigurationManager.AppSettings["MailTemplateFolder"]; } }

        public static bool UserLockoutEnabled { get { return AccountLockoutTime.Minutes > 0; } }

        public static int MaxLoginAttempts { get { return Convert.ToInt32(ConfigurationManager.AppSettings["MaxFailedAccessAttemptsBeforeLockout"]); } }
        public static TimeSpan AccountLockoutTime
        {
            get
            {
                return TimeSpan.FromMinutes(Convert.ToDouble(ConfigurationManager.AppSettings["AccountLockoutMinute"]));
            }
        }
        public static string FacebookKey { get { return ConfigurationManager.AppSettings["fb_key"]; } }
        public static string FacebookSecret { get { return ConfigurationManager.AppSettings["fb_secret"]; } }
        public static string GoogleClientID { get { return ConfigurationManager.AppSettings["google_clientid"]; } }
        public static string GoogleClientSecret { get { return ConfigurationManager.AppSettings["google_clientsecret"]; } }
        public static string MicrosoftID { get { return ConfigurationManager.AppSettings["microsoft_clientid"]; } }
        public static string MicrosoftSecret { get { return ConfigurationManager.AppSettings["microsoft_clientsecret"]; } }

        //Smtp Email keys
        public static string HostName { get { return ConfigurationManager.AppSettings["HostName"]; } }
        public static string FromName { get { return ConfigurationManager.AppSettings["FromName"]; } }
        public static string FromEmail { get { return ConfigurationManager.AppSettings["FromEmail"]; } }
        public static string SmtpAccount { get { return ConfigurationManager.AppSettings["SmtpAccount"]; } }
        public static string SmtpPassword { get { return ConfigurationManager.AppSettings["SmtpPassword"]; } }
        public static string PageSize { get { return ConfigurationManager.AppSettings["PageSize"]; } }
        public static string StorageConnection
        {
            get
            {
                return ConfigurationManager.AppSettings["StorageConnectionString"];
            }
        }

    }
}
