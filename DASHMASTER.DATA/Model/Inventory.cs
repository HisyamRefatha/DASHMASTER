using System;
using System.Collections.Generic;
using Vleko.DAL.Interface;


namespace DASHMASTER.DATA.Model 
{
    public partial class Inventory : IEntity
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public int QuantityChange { get; set; }
        public string Reason { get; set; } = null!;
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; } = null!;
        public DateTime? UpdateDate { get; set; }
        public string? UpdateBy { get; set; }

        public virtual MstProduct Product { get; set; } = null!;
    }
}
