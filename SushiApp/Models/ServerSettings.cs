using System;

namespace SushiApp.Models
{
    public class ServerSettings
    {
        public string IP { get; set; }
        public string Port { get; set; }
        public ServerController Controller { get; set; }


        public ServerSettings(string ip, string port, ServerController controller)
        {
            IP = ip;
            Port = port;
            Controller = controller;
        }
    }
}
