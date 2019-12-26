namespace OneCalc.WebApi.Controllers.Calculate.V1.Responses
{
    public class CalcResponse
    {
        public CalcResponse(string calculate, string result)
        {
            Calculate = calculate;
            Result = result;
        }

        public string Calculate { get;  }
        public string Result { get;  }
    }
}
