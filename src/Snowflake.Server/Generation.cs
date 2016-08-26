using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Server
{

    public sealed class Generation
    {
        private static Generation instance;
        private static Generation Instance
        {
            get { return instance; }
        }

        private Generation(int locationID, int nodeID)
        {
            LocationID = locationID;
            NodeID = nodeID;
        }

        private int LocationID
        {
            get;
            set;
        }

        private int NodeID
        {
            get;
            set;
        }

        // private long epoch = 1288834974657L;
        private long lastTimestamp = -1L;
        private long sequence = 0L;

        // Bit shifting numbering scheme to provide as many 
        const int bitShiftMinute = 22;//18;
        const int bitShiftDataCenterID = 17;//15;
        const int bitShiftServerID = 12;//10;

        const int maxLocationID = 31;//6;
        const int maxNodeID = 31;

        private object sync = new object();

        public static void Debug()
        {
            instance.DebugStuff();
        }

        private void DebugStuff()
        {
            var id = default(long);
            var test = new Dictionary<long, int>();
            var timestamp = 1288834974657L; // 34789015209L;
            timestamp = GetTimestamp();

            for (var j = 0; j < 10; j++)
            {
                for (var i = 0; i < 10000; i++)
                {
                    id = NextID(timestamp);
                    test.Add(id, 0);
                }

                timestamp += 1;
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="locationID">Between 0 - 6, identifies the location of the server, this is hard coded by the config.</param>
        /// <param name="nodeID">Between 0 - 31, identifies </param>
        public static void Init(int locationID, int nodeID)
        {
            if (locationID > maxLocationID)
                throw new Exception("Invalid location ID, only 31 locations are available");

            if (nodeID > maxNodeID)
                throw new Exception("Invalid node ID, only 32 nodes per location are available");

            instance = new Generation(locationID, nodeID);

        }

        public static long Next()
        {
            if (instance == null)
                throw new Exception("Snowflake not setup - call Snowflake.Setup first.");

            return Instance.NextID();
        }

        private long NextID()
        {
            var timestamp = GetTimestamp();
            return NextID(timestamp);
        }

        private long NextID(long timestamp)
        {
            lock (sync)
            {
                long localSequence = 0;

                if (System.Threading.Interlocked.Read(ref sequence) == 10000)
                    timestamp = WaitNextTimestamp(1000);

                if (lastTimestamp == timestamp)
                    localSequence = System.Threading.Interlocked.Increment(ref sequence);
                else
                {
                    System.Threading.Interlocked.Exchange(ref sequence, localSequence);
                }

                if (timestamp < lastTimestamp)
                {
                    // exceptionCounter.incr(1);
                    // log.error("clock is moving backwards.  Rejecting requests until %d.", lastTimestamp);
                    throw new Exception(string.Format("Clock moved backwards.  Refusing to generate id for {0} milliseconds", lastTimestamp - timestamp));
                }

                // if (localSequence >= 1024)
                // System.Diagnostics.Debug.WriteLine("Snowflake Sequence: " + localSequence);

                lastTimestamp = timestamp;
#pragma warning disable 675
                return (timestamp << bitShiftMinute) |
                       (LocationID << bitShiftDataCenterID) |
                       (NodeID << bitShiftServerID) |
                       localSequence;
            }
        }

        private long WaitNextTimestamp(int milli)
        {
            using (var handle = new System.Threading.ManualResetEventSlim(false))
                handle.Wait(milli);

            return GetTimestamp();
        }

        public static long GetLastFromDate(DateTime datetime)
        {
            if (instance == null)
                throw new Exception("Snowflake not setup - call Snowflake.Setup first.");

            return Instance.GetLastFromDateTime(datetime);
        }

        private long GetLastFromDateTime(DateTime datetime)
        {

            var timestamp = GetTimestamp(datetime);

#pragma warning disable 675
            return (timestamp << bitShiftMinute) |
                   (maxLocationID << bitShiftDataCenterID) |
                   (maxNodeID << bitShiftServerID) |
                   1024;
        }

        /*
        private long WaitUntiltilNextMillis(long lastTimestamp)
        {
            var timestamp = timeGen();
            while (timestamp <= lastTimestamp)
            {
                timestamp = timeGen();
            }
            return timestamp;
        }
        */

        static readonly DateTime applicationEpoch = new DateTime(2011, 1, 1).ToUniversalTime();

        private long GetTimestamp()
        {
            return Convert.ToInt64((DateTime.UtcNow - applicationEpoch).TotalSeconds / 10);
        }

        private long GetTimestamp(DateTime datetime)
        {
            return Convert.ToInt64((datetime.ToUniversalTime() - applicationEpoch).TotalSeconds / 10);
        }

        /// <summary>
        /// Finds the minimum and maximum IDs which could be possibily generated 
        /// for the date range in question, this assists in querying any sequence
        /// data by the time it was created.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private Tuple<long, long> ConvertDateTimeToNextID(DateTime start, DateTime stop)
        {

            return new Tuple<long, long>(0L, 0L);
        }

    }
}
