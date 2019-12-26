using System;
using System.Collections.Generic;
using System.Text;

namespace OneCalc.Test.Integration.Model
{
    public class BaseResponse
    {
        public int ErrorCode { get; set; }
        public string ErrorTitle { get; set; }
        public string ErrorMessage { get; set; }
    }
}
