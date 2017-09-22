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
        protected bool _Signed = false;

        public int Width => _Width;
        public bool Signed => _Signed;

        public BaseNumber()
        {
            _N = 0.0f;
        }

        public BaseNumber(UInt64 n, int width)
        {
            _Width = width;
            _N = n;
        }
        
        public BaseNumber(UInt64 n, int width, bool signed)
        {
            _Width = width;
            _Signed = signed;
            SetBaseData(n);
        }

        protected void SetBaseData(UInt64 n)
        {
            _N = Convert.ToSingle(n);
        }

        public UInt64 GetData()
        {
            return GetBaseData();
        }

        public UInt64 GetData(int width)
        {
            string s = GetBaseData().ToString();
            char[] bs = s.ToCharArray();
            return 0;
        }

        protected UInt64 GetBaseData()
        {
            UInt64 n = Convert.ToUInt64(Math.Round(_N));
            return n;
        }

        /// <summary>
        /// Get mask(1) from 0 to WIDTH
        /// </summary>
        /// <param name="width"></param>
        /// <returns></returns>
        public static UInt64 GetMask(int width)
        {
            UInt64 res = 0;

            if (width > 64) width = 64;

            for(int i = 0; i < width; i++)
            {
                res = (res << 1) ^ 1;
            }
            return res;
        }

        /// <summary>
        /// Get mask(1) from LSB to LSB + WIDTH
        /// </summary>
        /// <param name="lsb"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static UInt64 GetMask(int lsb, int width)
        {
            if (lsb >= 64) lsb = 63;
            if (lsb + width > 64) width = 64 - lsb;

            UInt64 m = GetMask(lsb + width);
            UInt64 inv = ~GetMask(lsb);
            return m & inv;
        }

        protected char[] GetFixedWidthDecChars(UInt64 n, int msb, int lsb)
        {
            string s = n.ToString();
            return new char[10];
        }

    }
}
