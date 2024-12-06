using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace DoorUtil
{
    public class Door
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public static string ClientGuid = Guid.NewGuid().ToString();
        public static string PasswordKey = "770504750825";
        public static string SchoolID = "25";

        public static async Task<bool> ExecuteOpenDoor(string id, string key, string roomId)
        {
            var session = await Door.LoginPlatform(id, key);
            var cardId = await Door.GetCardID(id, session);
            var token = await Door.GetToken(cardId, id);
            var door = await Door.OpenDoor(roomId, token);
            return door;
        }

        public static async Task<string> LoginPlatform(string id, string key)
        {
            var req = new HttpRequestMessage
            {
                RequestUri = new Uri($"http://202.207.245.234:9090/1001.json?stucode={id}&stupsw={key}"),
            };

            var session = MD5.HashData(Encoding.UTF8.GetBytes(RandomUtil.GetRandomString(8))).ToHexString().ToUpper();

            req.Headers.TryAddWithoutValidation("Cookie", $"JSESSIONID={session}");

            var result = await httpClient.SendAsync(req);
            var ctx = await result.Content.ReadAsStringAsync();
            if (!ctx.Contains("true"))
                return string.Empty;

            return session;
        }

        public static async Task<string> GetCardID(string id, string session)
        {
            var req = new HttpRequestMessage
            {
                RequestUri = new Uri($"http://202.207.245.234:9090/0006.json?stucode={id}")
            };

            var result = await httpClient.SendAsync(req);
            var ctx = await result.Content.ReadAsStringAsync();

            JObject jobj = JObject.Parse(ctx.ToString());

            if (jobj["resultCode"].ToString() != "0000")
                return string.Empty;

            var cardId = jobj["value"][0]["cardID"].ToString();

            if (cardId == null || cardId == string.Empty)
                return string.Empty;

            return cardId;
        }

        public static async Task<string> GetToken(string cardId, string id)
        {
            var req = new HttpRequestMessage
            {
                RequestUri = new Uri($"http://202.207.247.133/api/app/login/1/gettoken"),
                Method = HttpMethod.Post
            };

            req.Content = new StringContent($"terminal=Android&clientid={ClientGuid}&checknum={CryptoUtil.GetChecksum(cardId, PasswordKey, SchoolID)}&studentId={id}&school={SchoolID}&qyid=poscard");
            req.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            var result = await httpClient.SendAsync(req);
            var ctx = await result.Content.ReadAsStringAsync();

            JObject jobj = JObject.Parse(ctx.ToString());

            if (jobj["resultMsg"].ToString() != "ok")
                return string.Empty;

            var token = jobj["result"]["token"].ToString();

            if(token == null || token == string.Empty)
                return string.Empty;

            return token;
        }

        public static async Task<bool> OpenDoor( string roomId, string token)
        {
            var req = new HttpRequestMessage
            {
                RequestUri = new Uri($"http://202.207.247.133/api/app/order/1/remoteopenbyid"),
                Method = HttpMethod.Post
            };

            req.Content = new StringContent($"roomid={roomId}&token={token}");
            req.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var result = await httpClient.SendAsync(req);
            var ctx = await result.Content.ReadAsStringAsync();

            JObject jobj = JObject.Parse(ctx.ToString());

           return jobj["resultMsg"].ToString() == "ok";
        }

    }
}
