using System;
using System.Collections.Generic;
using Vleko.DAL.Interface;


namespace DASHMASTER.DATA.Model 
{
    public partial class TrsTransactionItems : IEntity
    {
        public Guid Id { get; set; }
        public Guid TransactionId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; } = null!;
        public DateTime? UpdateDate { get; set; }
        public string? UpdateBy { get; set; }

        public virtual MstProduct Product { get; set; } = null!;
        public virtual TrsTransaction Transaction { get; set; } = null!;
    }
}
