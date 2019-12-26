using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using OneCalc.Domain.Enums;
using OneCalc.Domain.Validators;

namespace OneCalc.Infrastructure.Validators
{
    public class OperationValidate : IOperationValidate
    {
        /// <summary>
        /// Проверка на разрешенные операции
        /// </summary>
        /// <param name="input"></param>
        /// <param name="allowOperations"></param>
        /// <returns></returns>
        public (bool, string) IsValidAllow(string input, OperationEnum allowOperations)
        {
            var enums = (OperationEnum[]) Enum.GetValues(typeof(OperationEnum));

            foreach (OperationEnum operation in enums)
            {
                var symbolOperation = GetMetaData(operation)?.Name ?? string.Empty;

                if (input.IndexOf(symbolOperation, StringComparison.CurrentCultureIgnoreCase) != -1 &&
                    (allowOperations & operation) != operation)
                    return (false, symbolOperation);
            }

            return (true, null);
        }

        public bool IsValid(OperationEnum checkOperation, string input, OperationEnum userOperations)
        {
            // берем значение операции из атрибута
            var symbolOperation = GetMetaData(checkOperation).Name ?? string.Empty;

            // не нашли такую операцию, не проверяем дальше
            if (input.IndexOf(symbolOperation, StringComparison.CurrentCultureIgnoreCase) == -1)
                return true;

            return (userOperations & checkOperation) == checkOperation;
        }

        private DisplayAttribute GetMetaData(OperationEnum operation)
        {
           return operation.GetType().GetMember(operation.ToString()).FirstOrDefault()?.GetCustomAttribute<DisplayAttribute>();
        }
    }
}
