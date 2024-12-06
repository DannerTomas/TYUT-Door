using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoorUtil
{
    public static class StringExtension
    {
        public static string FromBase64ToString(this string data)
        {
            try
            {
                return Encoding.Default.GetString(Convert.FromBase64String(data));
            }
            catch
            {
                return string.Empty;
            }

        }

        public static string ToUTF8String(this string data)
        {
            try
            {
                return Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(data));
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string ToBase64String(this string data)
        {
            try
            {
                return Convert.ToBase64String(Encoding.Default.GetBytes(data));
            }
            catch
            {
                return string.Empty;
            }

        }

        public static byte[] FromHexString(this string hexString)
        {
            if (hexString == null)
            {
                throw new ArgumentNullException(nameof(hexString));
            }

            if (hexString.Length % 2 != 0)
            {
                throw new FormatException("Invalid length for a hexadecimal string.");
            }

            var bytes = new byte[hexString.Length / 2];

            for (int i = 0; i < hexString.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            }

            return bytes;
        }

        public static string ToHexString(this byte[] bytes)
        {
            return BitConverter.ToString(bytes, 0, bytes.Length).Replace("-", "").ToLower();
        }

        /// <summary>
        /// 取中间文本 + static string GetMiddleStr(string oldStr,string preStr,string nextStr)
        /// </summary>
        /// <param name="oldStr">原文</param>
        /// <param name="preStr">前文</param>
        /// <param name="nextStr">后文</param>
        /// <returns></returns>
        public static string SubString(this string oldStr, string preStr, string nextStr)
        {
            string tempStr = oldStr.Substring(oldStr.IndexOf(preStr) + preStr.Length);
            tempStr = tempStr.Substring(0, tempStr.IndexOf(nextStr));
            return tempStr;
        }
    }

}
