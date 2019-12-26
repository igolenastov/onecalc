using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using OneCalc.Domain.Enums;

namespace OneCalc.Domain.Extensions
{
    public static class OperationExtension
    {
        /// <summary>
        /// Преобразует строку в операции
        /// </summary>
        /// <param name="val"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static OperationEnum? ConvertToOperations(this string val, char separator = ',')
        {
            // собираем справочник существующих операций
            Dictionary<string, OperationEnum> dicSymbols = new Dictionary<string, OperationEnum>();

            var enums = (OperationEnum[])Enum.GetValues(typeof(OperationEnum));
            foreach (OperationEnum operation in enums)
            {
                var symbolOperation = GetMetaData(operation)?.Name ?? string.Empty;
                dicSymbols.TryAdd(symbolOperation, operation);
            }

            var sOperations = val.Split(separator);

            // для проверки дублирования операций
            HashSet<OperationEnum> operations = new HashSet<OperationEnum>();

            OperationEnum result = OperationEnum.Non;

            foreach (var sOperation in sOperations)
            {
                if (dicSymbols.TryGetValue(sOperation, out var op) && !operations.Contains(op))
                {
                    operations.Add(op);

                    result |= op;
                }
            }

            if (result != OperationEnum.Non)
                return result;

            return null;
        }

        /// <summary>
        /// Преобразует список в enum flag
        /// </summary>
        /// <returns></returns>
        public static OperationEnum ToEnumFlags(this List<OperationEnum> operations)
        {
            OperationEnum result = OperationEnum.Non;

            foreach (var sOperation in operations)
            {
                result |= sOperation;
            }

            return result;
        }

        private static DisplayAttribute GetMetaData(OperationEnum operation)
        {
            return operation.GetType().GetMember(operation.ToString()).FirstOrDefault()?.GetCustomAttribute<DisplayAttribute>();
        }
    }
}
