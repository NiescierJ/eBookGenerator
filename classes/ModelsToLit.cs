using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eBookGenerator
{
    public class ModelsToLit
    {
        public String ModelNo { get; set; }
        public String ManufacturingNumber { get; set; }
        public String ProdType { get; set; }
        public String PartNo { get; set; }
        public String TypeOfLit { get; set; }
        public String Ecomments { get; set; }
        public String LitCD { get; set; }
        public String Lang { get; set; }

        public ModelsToLit Clone()
        {
            ModelsToLit clone = new ModelsToLit
            {
                ModelNo = this.ModelNo,
                ManufacturingNumber = this.ManufacturingNumber,
                ProdType = this.ProdType,
                PartNo = this.PartNo,
                TypeOfLit = this.TypeOfLit,
                Ecomments = this.Ecomments,
                LitCD = this.LitCD,
                Lang = this.Lang
            };

            return clone;
        }
    }
}
