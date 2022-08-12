using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SG_WM_zadatak.Config
{
    public class Konekcija
    {
        public static string GetKonekcija(){
            return ConfigurationManager.ConnectionStrings["baza"].ConnectionString;
}
    }
}