using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrystalCalc
{
    public interface CrystalOperator
    {
        bool IsSupported(BaseNumber n1, BaseNumber n2);
    }
}
