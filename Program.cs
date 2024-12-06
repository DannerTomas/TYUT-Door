using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoorUtil
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var a = Door.ExecuteOpenDoor("学生证ID", "校园e卡通学生证密码(一般6位数)", "房间ID(抓包获取，一般4位数)").Result;
            Console.WriteLine(a);
        }
    }
}
