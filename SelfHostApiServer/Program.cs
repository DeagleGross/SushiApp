using System;
using System.Data.SqlClient;
using Microsoft.Owin.Hosting;
using SelfHostApiServer.GlobalSettings;
using SelfHostApiServer.Models;

namespace SelfHostApiServer
{
    class Program
    {
        static void ServerStart()
        {

            using (WebApp.Start<Startup>($"http://{GSettings.Adress.IP}:{GSettings.Adress.Port}"))
            {
                do
                {
                    Console.WriteLine("************************************************");
                    Console.WriteLine($"Web Server is running under " +
                                      $"{GSettings.Adress.IP}:{GSettings.Adress.Port}");
                    Console.WriteLine("------------------------------------------------");

                    Console.WriteLine("Press ESC to quit.");
                } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
            }
        }

        static void Main(string[] args)
        {
            ServerStart();
        }
    }
}