using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrystalCalc
{
    public class BaseNumber : IComparable<BaseNumber>
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

        #region Base Function
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
            throw new NotImplementedException();
        }

        protected UInt64 TruncateTo(int width)
        {
            throw new NotImplementedException();
        }

        protected UInt64 SaturateTo(int width)
        {
            throw new NotImplementedException();
        }

        public void Truncate(int width)
        {
            SetBaseData(TruncateTo(width));
        }

        public void Saturate(int width)
        {
            SetBaseData(SaturateTo(width));
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
            else if (width < 1) width = 1;

            if(width == 64)
            {
                res = UInt64.MaxValue;
            }
            else
            {
                res = (1ul << (width + 1)) - 1;
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

        #endregion

        #region OPERATOR
        public static UInt64 operator +(BaseNumber lhs, BaseNumber rhs)
        {
            return lhs.GetBaseData() + rhs.GetBaseData();
        }

        public static UInt64 operator -(BaseNumber lhs, BaseNumber rhs)
        {
            return lhs.GetBaseData() - rhs.GetBaseData();
        }

        public static UInt64 operator >>(BaseNumber lhs, int shift)
        {
            return lhs.GetBaseData() >> shift;
        }

        public static UInt64 operator <<(BaseNumber lhs, int shift)
        {
            return lhs.GetBaseData() << shift;
        }

        public static bool operator ==(BaseNumber lhs, BaseNumber rhs)
        {
            return lhs.GetBaseData() == rhs.GetBaseData();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator !=(BaseNumber lhs, BaseNumber rhs)
        {
            return lhs.GetBaseData() != rhs.GetBaseData();
        }

        public static int operator >(BaseNumber lhs, BaseNumber rhs)
        {
            return lhs.CompareTo(rhs);
        }

        public static int operator <(BaseNumber lhs, BaseNumber rhs)
        {
            return rhs.CompareTo(lhs);
        }
        #endregion

        public int CompareTo(BaseNumber cmp)
        {
            int res = 0;
            if(this.GetBaseData() > cmp.GetBaseData())
            {
                res = 1;
            }
            else if(this.GetBaseData() > cmp.GetBaseData())
            {
                res = 0;
            }
            else
            {
                res = -1;
            }
            return res;
        }

        #region String Fomart

        protected char[] GetFixedWidthDecChars(int msb, int lsb)
        {
            if(lsb < 0)
            {
                throw new ArgumentOutOfRangeException("LSB value is less than 0");
            }
            if(msb >= _Width)
            {
                throw new ArgumentOutOfRangeException("MSB value is larger than data width");
            }
            UInt64 mask = GetMask(lsb, msb - lsb);
            UInt64 masked = this.GetBaseData() & mask;
            if(lsb > 0)
            {
                masked = masked >> lsb;
            }
            return masked.ToString().ToCharArray();
        }

        protected char[] GetFixedWidthHexChars(int msb, int lsb)
        {
            char[] cs = GetFixedWidthDecChars(msb, lsb);
            UInt64 n = UInt64.Parse(cs.ToString());
            UInt64 div = n;
            UInt64 rem = 0;
            List<int> hex = new List<int>();
            while(div > 0)
            {
                rem = div % 16;
                hex.Add((int)rem);
                div = div / 16;
            }
            string res = "";
            foreach(int h in hex)
            {
                string s = h.ToString("X");
                res.Insert(0, s);
            }
            return res.ToCharArray();
        }

        public override string ToString()
        {
            return GetFixedWidthDecChars(this.Width - 1, 0).ToString();
        }

        public string ToString(string format)
        {
            string res = "";
            if(format.Equals("D"))
            {
                res = this.GetFixedWidthDecChars(this.Width - 1, 0).ToString();
            }
            else if(format == "X")
            {
                res = this.GetFixedWidthHexChars(this.Width - 1, 0).ToString();
            }
            return res;
        }

        #endregion
    }
}
