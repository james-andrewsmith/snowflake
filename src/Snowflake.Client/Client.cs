using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Snowflake.Protocol;
using Sodao.FastSocket.Client;

namespace Snowflake.Client
{
    public sealed class SnowflakeClient : IDisposable
    {

        #region // Constructor //
        public SnowflakeClient(IEnumerable<string> endpoints)
        {
            #if DEBUG
            _socket = new AsyncBinarySocketClient(8192, 8192, 30000, 30000);
            #else
            _socket = new AsyncBinarySocketClient(8192, 8192, 3000, 3000);
            #endif
            _nodes = new List<string>();

            foreach (var endpoint in endpoints)
                AddNode(endpoint);
        }
        #endregion

        #region // Privates //
        private AsyncBinarySocketClient _socket;
        private List<string> _nodes;
        #endregion

        #region // Cluster & Server Management //

        public void AddNode(string endpoint)
        {
            if (_nodes.Contains(endpoint))
                throw new Exception("Node already exists: " + endpoint);

            string ip = endpoint;
            int port = 8401;
            if (endpoint.Contains(':'))
            {
                ip = endpoint.Substring(0, endpoint.IndexOf(':'));
                port = Convert.ToInt32(endpoint.Substring(endpoint.IndexOf(':') + 1));
            }

            _socket.RegisterServerNode(endpoint, new IPEndPoint(IPAddress.Parse(ip), port));
            _nodes.Add(endpoint);
        } 
        #endregion 

        public long Next()
        {
            var next = NextAsync();
            Task.WaitAll(next);
            return next.Result;
        }

        public Task<long> NextAsync()
        {
            var request = new NextRequest();
            return _socket.Send("next", request.Serialize(), data => data.Buffer.Deserialize<NextResponse>())
                          .ContinueWith(t =>
                          {
                              return t.Result.Sequence.FirstOrDefault();
                          });                           
        }

        public List<long> Next(int count)
        {
            var next = NextAsync(count);
            Task.WaitAll(next);
            return next.Result;
        }

        public Task<List<long>> NextAsync(int count)
        {
            var request = new NextRequest(count);
            return _socket.Send("next", request.Serialize(), data => data.Buffer.Deserialize<NextResponse>())
                          .ContinueWith(t =>
                          {
                              return t.Result.Sequence;
                          });
        }

        public void Dispose()
        {
            if (_socket != null)
            {
                _socket.Stop();
            }
        }

    }
    // Connect to zookeeper
    // -> Watch for changes and keep local config up to date
    // -> Zookeeper nodes can be passed in at runtime, they 
    //    don't need to be part of the config
    // -> A service higher up can connect to azure and supply 
    //    the IP/address combo

    // Connect to a random server    

    // Request the next n id's

}
