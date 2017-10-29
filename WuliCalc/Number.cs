using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WuliCalc
{
    public class Number : BaseNumber
    {
        public Number(UInt64 n) 
            : base(n, 64)
        {
        }

        public Number(UInt32 n) 
            : base((UInt64)n, 32)
        {
        }

        public Number(UInt16 n)
            : base((UInt64)n, 16)
        {
        }

        public Number(Byte n) 
            : base((UInt64)n, 8)
        {
        }

        public Number(UInt64 n, int width) 
            : base(n, width)
        {
        }

    }
}
