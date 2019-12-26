namespace OneCalc.Domain.Errors
{
    public interface IErrorCode
    {
        int ErrorCode { get; }
        string ErrorTitle { get; }
    }

    public static class ErrorCodes
    {
        public static class Core
        {
            public static IErrorCode Unrecognized => new ECode(-1, "Error");
            public static IErrorCode InfoError => new ECode(100, "Info");
            public static IErrorCode DatabaseError => new ECode(101, "Ошибка при сохранении изменений в базе данных");
            public static IErrorCode ValidationError => new ECode(102, "Ошибка проверки данных");
            public static IErrorCode AuthError => new ECode(205, "Auth error");
            public static IErrorCode CalcError => new ECode(305, "Calc error");
        }

        internal class ECode : IErrorCode
        {
            public ECode(int code, string title)
            {
                ErrorCode = code;
                ErrorTitle = title;
            }

            public int ErrorCode { get; }
            public string ErrorTitle { get; }
        }
    }
}
