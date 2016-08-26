using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
 
using Sodao.FastSocket.Server;
using Sodao.FastSocket.Server.Config;
using Sodao.FastSocket.Server.Command;
using Sodao.FastSocket.SocketBase;
 
using Snowflake.Protocol;


namespace Snowflake.Server
{
    class Program
    {
        static void Main(string[] args)
        {

            // setup a snowflake server with this ID
            Generation.Init(Configuration.Instance.LocationID, Configuration.Instance.NodeID);

            var config = ConfigurationManager.GetSection("socketServer") as SocketServerConfig;            
            config.Servers[0].Port = Configuration.Instance.Port;
            
            // Start a socket server which will respond
            SocketServerManager.Init();
            SocketServerManager.Start();

            
            // prevent the sever from exiting                
            bool running = true;
            do
            {
                var command = Console.ReadLine();
                running = ProcessCommand(command);
            }
            while (running);
             

            // 1. Server runs on a port from a config file
            // 2. The server uses protobuf messages
            // 3. The server connects to zookeeper and enters a barrier limited
            //    to 32 nodes
            // 3. a) if the server can enter the barier it adds itself to zookeeper
            //       with it's port and IP (input endpoint with ACL)            
            //    b) if the server cannot join it watches the parent waiting for a 
            //       change in children and will then join
        }


        private static bool ProcessCommand(string line)
        {
            line = line.ToLower();
            if (line == "quit" || line == "exit" || line == "!q")
                return false;

            // example commands 
            // -> Clear Cache
            if (line == "next")
                Console.WriteLine(Generation.Next());

            return true;
        }
    }



}
