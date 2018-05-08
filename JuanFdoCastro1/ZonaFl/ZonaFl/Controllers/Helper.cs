using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ZonaFl.Controllers
{
    public static class Helper
    {
        public static string ReadSetting(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                string result = appSettings[key] ?? "Not Found";
                return result;
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error reading app settings");
            }
            return "";
        }
        public static string ConvertDtFormat(string dateTimeString)
        {
            dateTimeString = dateTimeString.Trim();
            while (dateTimeString.Contains("  "))
            {
                dateTimeString = dateTimeString.Replace("  ", " ");
            }

            if (dateTimeString == null || dateTimeString.Length == 0)
            {
                return string.Empty;
            }
            else
            {
                DateTime convertedDateTime = new DateTime();
                string userDateFormat = ReadSetting("FormatDate");

                try
                {
                    if (userDateFormat.Trim().Contains("dd/MM/yyyy"))
                        convertedDateTime = DateTime.ParseExact(dateTimeString, userDateFormat, CultureInfo.InvariantCulture,
                                                                DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite);
                    else if (userDateFormat.Trim().Contains("MM/dd/yyyy"))
                        convertedDateTime = DateTime.ParseExact(dateTimeString, userDateFormat, CultureInfo.InvariantCulture,
                                                                DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite);

                    return convertedDateTime.ToString("MMM dd, yyyy hh:mm tt");
                }
                catch
                {
                    return "Invalid DateTime";
                }
            }
        }

    }
}