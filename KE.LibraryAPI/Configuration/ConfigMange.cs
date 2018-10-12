using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Web.Configuration;

namespace KE.LibraryAPI.Configuration
{
    public class ConfigMange
    {

        public static string GetAppSettings(string key)
        {
            return GetAppSettings(key, true);
        }

        public static string GetAppSettings(string key, bool IsWinApp)
        {
            if (IsWinApp)
            {
                return ConfigurationManager.AppSettings[key].ToString();
            }
            else
            {
                return WebConfigurationManager.AppSettings[key].ToString();
            }
        }
    }
}
