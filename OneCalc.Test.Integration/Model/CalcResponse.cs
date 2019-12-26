using System;
using System.Collections.Generic;
using System.Text;

namespace OneCalc.Test.Integration.Model
{
    public class CalcResponse : BaseResponse
    {
        public string Calculate { get; set; }
        public string Result { get; set; }
    }
}
