using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DoorUtil
{
    /// <summary>
    /// Reverse from com.campus.one.card
    /// At 2024.5
    /// </summary>
    public class CryptoUtil
    {
        private static string FixString(string str, int i)
        {
            int i2 = 0;
            string str2 = "";
            if (str == null || "".Equals(str))
            {
                while (i2 < i)
                {
                    str2 = str2 + "0";
                    i2++;
                }
                return str2;
            }
            if (str.Length >= i || str.Length <= 0)
            {
                return str.Length > i ? str.Substring(str.Length - i, i) : str;
            }
            while (i2 < i - str.Length)
            {
                str2 = str2 + "0";
                i2++;
            }
            return str2 + str;
        }

        public static string DesEncrypt(string encryptString, string key)
        {
            DESCryptoServiceProvider provider = new()
            {
                Key = key.FromHexString(),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.None
            };

            var buff = encryptString.FromHexString();
            return provider.CreateEncryptor().TransformFinalBlock(buff, 0, buff.Length).ToHexString();
        }

        public static string GetCode(long id, string pwdkey, string schoolId)
        {
            var time = DateTime.Now;
            var yymmdd = time.ToString("yyMMdd");
            var hhmm = time.ToString("HHmm");
            var stra = pwdkey + hhmm;

            var strb = FixString(BitConverter.GetBytes(id).Take(4).Reverse().ToArray().ToHexString(), 8) + yymmdd + FixString(schoolId, 2);
            return DesEncrypt(strb, stra) + hhmm;
        }

        public static string GetChecksum(string cardId, string pwdKey, string schoolId)
        {
            return FixString(GetCode(long.Parse(FixString(cardId, 10)), pwdKey, schoolId), 20).ToUpper();
        }
    }
}
