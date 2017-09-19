using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eBookGenerator
{
    class ModelEqualityComparer2 : IEqualityComparer<ModelsToLit>
    {
        #region IEqualityComparer<ModelsToLit> Members

        public bool Equals(ModelsToLit x, ModelsToLit y)
        {
            return x.ManufacturingNumber == y.ManufacturingNumber;
        }

        public int GetHashCode(ModelsToLit obj)
        {
            return obj.ManufacturingNumber.GetHashCode();
        }

        #endregion
    }
}
