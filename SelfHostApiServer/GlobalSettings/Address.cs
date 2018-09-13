using System;
namespace SelfHostApiServer.GlobalSettings
{
    public class Address
    {
        public string IP { get; set; }
        public string Port { get; set; }

        public Address(string ip, string port)
        {
            IP = ip;
            Port = port;
        }
    }
}
