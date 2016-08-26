using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Sodao.FastSocket.Server;
using Sodao.FastSocket.Server.Config;
using Sodao.FastSocket.Server.Command;
using Sodao.FastSocket.SocketBase;

using Snowflake;
using Snowflake.Protocol;

namespace Snowflake.Server
{
    public sealed class NextCommand : ICommand<AsyncBinaryCommandInfo>
    {
        public string Name
        {
            get { return "next"; }
        }

        public void ExecuteCommand(IConnection connection, AsyncBinaryCommandInfo request)
        {
            try
            {
                if (request.Buffer == null || request.Buffer.Length == 0)
                {
                    // do some logging here
                    connection.BeginDisconnect();
                    return;
                }

                NextRequest nr = request.Buffer.Deserialize<NextRequest>();
                NextResponse response = new NextResponse();
                for (var i = 0; i < nr.Count; i++)
                    response.Sequence.Add(Generation.Next());

                // get from protocol buffers
                request.Reply(connection, response.Serialize());
            }
            catch (Exception exp)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine(exp.Message);
                Console.Error.WriteLine(exp.StackTrace);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
    }
}
