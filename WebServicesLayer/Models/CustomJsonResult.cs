using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;
namespace WebServicesLayer.Models
{
    public class CustomJsonResult
    {
        public int typeResult { set; get; }
        public int codeResult { set; get; }
        public object result { set; get; }
        public string message { set; get; }

        public CustomJsonResult()
        {

        }

        public CustomJsonResult(object oResult)
        {
            result = oResult;
        }

        public CustomJsonResult(int iTypeResult, int iCodeResult, object oResult, string sMessage)
        {
            result = oResult;
            typeResult = iTypeResult;
            codeResult = iCodeResult;
            message = sMessage;

        }
    }
}
