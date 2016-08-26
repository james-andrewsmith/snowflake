using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ProtoBuf;

namespace Snowflake
{
    [ProtoContract]
    public sealed class NextResponse
    {

        #region // Constructors //

        public NextResponse()
        {
            _sequence = new List<long>();
        }

        #endregion

        #region // Fields //
        [ProtoMember(1)]
        List<long> _sequence;
        #endregion

        #region // Properties //

        public List<long> Sequence
        {
            get { return _sequence; }
        }         

        #endregion

    }
}
