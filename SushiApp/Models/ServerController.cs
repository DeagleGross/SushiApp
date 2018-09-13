using System;


namespace SushiApp.Models
{
    public class ServerController
    {
        public string Api { get; set; }
        public string Name { get; set; }

        public ServerController(string api, string name)
        {
            Api = api;
            Name = name;
        }
    }
}
