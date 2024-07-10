using System;
using System.Collections.Generic;
using Vleko.DAL.Interface;


namespace DASHMASTER.DATA.Model 
{
    public partial class TrsTransaction : IEntity
    {
        public TrsTransaction()
        {
            TrsPayment = new HashSet<TrsPayment>();
            TrsTransactionItems = new HashSet<TrsTransactionItems>();
        }

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = null!;
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; } = null!;
        public DateTime? UpdateDate { get; set; }
        public string? UpdateBy { get; set; }

        public virtual MstUser User { get; set; } = null!;
        public virtual ICollection<TrsPayment> TrsPayment { get; set; }
        public virtual ICollection<TrsTransactionItems> TrsTransactionItems { get; set; }
    }
}
