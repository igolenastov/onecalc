using OneCalc.Domain.Enums;

namespace OneCalc.Domain.Validators
{
    public interface IOperationValidate
    {
        (bool, string) IsValidAllow(string input, OperationEnum allowOperations);
        bool IsValid(OperationEnum checkOperation, string input, OperationEnum userOperations);
    }
}
