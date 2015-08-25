using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kartforandring.Utility.Regex
{
	internal static class Patterns
	{
        internal static string Guid
        {
            get { return @"^[A-Z0-9]{8}-[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{12}$"; }
        }


        internal static string FastighetId
        {
            get { return @"^\d{9}$"; }
        }


        internal static string Diarie
        {
            get { return @"^(19\d{2}\.\d{1,4})$|^(20\d{2}\.\d{1,4})$"; }
        }
	}
}