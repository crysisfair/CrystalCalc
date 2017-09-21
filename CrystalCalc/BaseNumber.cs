using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrystalCalc
{
    public class BaseNumber
    {
        protected float _N;
        protected int _Width = 64;

        public int Width => _Width;
        public BaseNumber()
        {
            _N = 0.0f;
        }
        public long GetBaseInt()
        {
            return Convert.ToInt64(_N);
        }
    }
}
