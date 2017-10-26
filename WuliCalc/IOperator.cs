using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WuliCalc
{
    public interface IWuliOperator
    {
        bool IsSupported(BaseNumber n1, BaseNumber n2);
    }
}
