using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrystalCalc
{
    public class Number : BaseNumber
    {
        public Number(UInt64 n) 
            : base(n, 64)
        {
            SetBaseData(n);
        }

        public Number(UInt32 n) 
            : base(n, 32)
        {
        }

        public Number(UInt16 n)
            : base(n, 16)
        {
        }

        public Number(Byte n) 
            : base(n, 8)
        {
        }

        public Number(UInt64 n, int width) 
            : base(n, width)
        {
        }

    }
}
