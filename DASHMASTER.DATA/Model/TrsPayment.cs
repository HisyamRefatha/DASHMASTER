using System;
using System.Collections.Generic;
using Vleko.DAL.Interface;


namespace DASHMASTER.DATA.Model 
{
    public partial class TrsPayment : IEntity
    {
        public Guid Id { get; set; }
        public Guid TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; } = null!;
        public DateTime? UpdateDate { get; set; }
        public string? UpdateBy { get; set; }

        public virtual TrsTransaction Transaction { get; set; } = null!;
    }
}
