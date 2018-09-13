using System;
using System.Text;
using Newtonsoft.Json;

namespace SushiApp.Helpers
{
    public static class SerializationHelper
    {
        public static byte[] SerializeToBytes(this object target)
        {
            var serializedString = JsonConvert.SerializeObject(target);
            return Encoding.UTF8.GetBytes(serializedString);
        }

        public static string SerializeToJson(this object target)
        {
            return JsonConvert.SerializeObject(target);
        }
    }
}
