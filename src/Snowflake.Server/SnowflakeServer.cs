using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Sodao.FastSocket.Server;
using Sodao.FastSocket.Server.Config;
using Sodao.FastSocket.Server.Command;
using Sodao.FastSocket.SocketBase;

namespace Snowflake.Server
{

    public sealed class SnowflakeServer : CommandSocketService<AsyncBinaryCommandInfo>
    {
        public override void OnConnected(IConnection connection)
        {
            base.OnConnected(connection);
            Console.WriteLine(connection.RemoteEndPoint.ToString() + " connected");
        }

        public override void OnDisconnected(IConnection connection, Exception ex)
        {
            base.OnDisconnected(connection, ex);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(connection.RemoteEndPoint.ToString() + " disconnected");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public override void OnException(IConnection connection, Exception ex)
        {
            base.OnException(connection, ex);
            Console.WriteLine("error: " + ex.ToString());
        }

        protected override void HandleUnKnowCommand(IConnection connection, AsyncBinaryCommandInfo commandInfo)
        {
            Console.WriteLine("unknow command: " + commandInfo.CmdName);
        }
    }
}
