using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ProtoBuf;

namespace Snowflake
{
    [ProtoContract]
    public sealed class NextRequest
    {

        #region // Constructors //

        public NextRequest()
            : this(1)
        { }

        public NextRequest(int count)
        {
            _count = count;
        }

        #endregion

        #region // Fields //
        [ProtoMember(1)]
        int _count;
        #endregion

        public int Count
        {
            get { return _count; }
        }
    }
}
