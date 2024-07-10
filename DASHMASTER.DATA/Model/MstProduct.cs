using System;
using System.Collections.Generic;
using Vleko.DAL.Interface;


namespace DASHMASTER.DATA.Model 
{
    public partial class MstProduct : IEntity
    {
        public MstProduct()
        {
            Inventory = new HashSet<Inventory>();
            Reviews = new HashSet<Reviews>();
            TrsTransactionItems = new HashSet<TrsTransactionItems>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; } = null!;
        public DateTime? UpdateDate { get; set; }
        public string? UpdateBy { get; set; }
        public Guid CategoryId { get; set; }

        public virtual MstCategory Category { get; set; } = null!;
        public virtual ICollection<Inventory> Inventory { get; set; }
        public virtual ICollection<Reviews> Reviews { get; set; }
        public virtual ICollection<TrsTransactionItems> TrsTransactionItems { get; set; }
    }
}
