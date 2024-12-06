using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoorUtil
{
    internal class RandomUtil
    {
        public static Random Instance { get; } = new Random();

        public static string GetRandomString(int length, string chars = null)
        {
            if (string.IsNullOrEmpty(chars))
            {
                chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghizklmnopqrstuvwxyz0123456789";
            }
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[Instance.Next(s.Length)]).ToArray());
        }
    }
}
