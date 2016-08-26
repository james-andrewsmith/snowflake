using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Client
{
    public class Program
    {
        private static SnowflakeClient client;
        public static void Main(string[] args)
        {
            client = new SnowflakeClient(new List<string> { "127.0.0.1:8401" });

            // client = new SnowflakeClusterClient("127.0.0.1:4001");
                        
            var id = client.Next();
            var ids = client.Next(5);

            bool running = true;
            do
            {
                var command = Console.ReadLine();
                running = ProcessCommand(command);
            }
            while (running);

        }

        private static bool ProcessCommand(string line)
        {
            line = line.ToLower().Trim();
            if (line == "quit" || line == "exit" || line == "!q")
                return false;

            if (line == "next")
                Console.WriteLine(client.Next());

            return true;
        }
         

    }
}
