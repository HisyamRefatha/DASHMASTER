using System;
using System.Collections.Generic;
using Vleko.DAL.Interface;


namespace DASHMASTER.DATA.Model 
{
    public partial class Reviews : IEntity
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } = null!;
        public DateTime CreateDate { get; set; }

        public virtual MstProduct Product { get; set; } = null!;
        public virtual MstUser User { get; set; } = null!;
    }
}
