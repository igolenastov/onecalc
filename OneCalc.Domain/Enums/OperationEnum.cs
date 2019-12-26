using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OneCalc.Domain.Enums
{
    [Flags]
    public enum OperationEnum
    {
        Non = 1,
        [Display(Name = "/")]
        Div = 2,
        [Display(Name = "*")]
        Mul = 4,
        [Display(Name = "+")]
        Plus = 8,
        [Display(Name = "-")]
        Minus = 16,
        [Display(Name = "(")]
        ParanthesesLeft = 32,
        [Display(Name = ")")]
        ParanthesesRight = 64

    }
}
