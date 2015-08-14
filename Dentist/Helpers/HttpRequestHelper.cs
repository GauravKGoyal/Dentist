using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Dentist.Helpers
{
    public static class HttpRequestHelper
    {
        public static bool FormSaveAndCloseClicked(this HttpRequestBase httpRequestBase)
        {
            return httpRequestBase.Form["btnSubmit"] == "Save and Close";
        }
    }
}
