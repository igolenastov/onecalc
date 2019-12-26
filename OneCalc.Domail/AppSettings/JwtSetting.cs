using System;
using System.Collections.Generic;
using System.Text;

namespace OneCalc.Domain.AppSettings
{
    public class JwtSetting
    {
        /// <summary>
        /// Секретный ключ для генерации JWT токена
        /// </summary>
        public string SecretKey { get; set; }
    }
}
