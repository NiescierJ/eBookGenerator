using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eBookGenerator
{
    class ModelEqualityComparer : IEqualityComparer<ModelsToLit>
    {
        #region IEqualityComparer<ModelsToLit> Members

        public bool Equals(ModelsToLit x, ModelsToLit y)
        {
            return x.ModelNo == y.ModelNo && x.ProdType == y.ProdType;
        }

        public int GetHashCode(ModelsToLit obj)
        {
            return obj.ModelNo.GetHashCode() + obj.ProdType.GetHashCode();
        }

        #endregion
    }
}
