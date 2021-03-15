using System;

namespace GameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Game Server - Cypheri Studios (v0.1.0b)";
            Server.Start(50, 1234);
            Console.ReadKey();
        }
    }
}
