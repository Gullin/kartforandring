using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace kartforandring
{
    public partial class dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void getModuls()
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            NameValueCollection modules = new NameValueCollection();
            foreach (string item in appSettings.AllKeys)
            {
                if (appSettings[item].Trim().ToUpper().StartsWith("MOD_"))
                {
                }
            }
            //var items = moduls.AllKeys.SelectMany(moduls.GetValues, (k, v) => new {key = k, value = v});
        }
    }

    public class Moduls
    {
        public string Modul { get; set; }
        public KeyValuePair<string, string> KeyValue { get; set; }
    }
}